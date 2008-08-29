using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ProcessingDlr
{
	class ParserException : ApplicationException
	{
		public ParserException (string message)
			: base (message)
		{
		}

		public ParserException (string message, Exception innerException)
			: base (message, innerException)
		{
		}
	}

	class Tokenizer : ProcessingDlr.yyParser.yyInput
	{
		TextReader source;
		bool should_dispose;
		int line = 1, column = 1, saved_line, saved_column;
		string base_uri;

		int current_token;
		object current_value;

		int peek_char;
		bool next_increment_line;

		public Tokenizer (string filename)
		{
			should_dispose = true;
			base_uri = filename;
			source = new StreamReader (filename);
		}

		public Tokenizer (TextReader reader, string baseUri)
		{
			this.source = reader;
		}

		public int Line {
			get { return saved_line; }
		}

		public int Column {
			get { return saved_column; }
		}

		public string BaseUri {
			get { return base_uri; }
		}

		public string Location {
			get { return String.Format ("{0} ({1},{2})", BaseUri, Line, Column); }
		}

		public void Dispose ()
		{
			if (should_dispose)
				source.Close ();
			should_dispose = false;
		}

		public int token ()
		{
			return current_token;
		}

		public bool advance ()
		{
			current_value = null;
			current_token = ParseToken (false);
			saved_line = line;
			saved_column = column;
			return current_token != Token.EOF;
		}

		public object value ()
		{
			return current_value;
		}

		// based on RncTokenizer

		private int PeekChar ()
		{
			if (peek_char == 0)
				peek_char = source.Read ();

			return peek_char;
		}

		private int ReadChar ()
		{
			int ret;
			if (peek_char != 0) {
				ret = peek_char;
				peek_char = 0;
			}
			else
				ret = source.Read ();

			if (next_increment_line) {
				line++;
				column = 1;
				next_increment_line = false;
			}
			switch (ret) {
			case '\r':
				break;
			case '\n':
				next_increment_line = true;
				goto default;
			default:
				column++;
				break;
			}

			return ret;
		}

		private void SkipWhitespaces ()
		{
			while (true) {
				switch (PeekChar ()) {
				case ' ':
				case '\t':
				case '\r':
				case '\n':
					ReadChar ();
					continue;
				default:
					return;
				}
			}
		}

		char [] nameBuffer = new char [30];

		private string ReadStringLiteral ()
		{
			int index = 0;
			bool loop = true;
			while (loop) {
				int c = ReadChar ();
				if (c == '"')
					break;
				if (c < 0)
					throw new ParserException ("Unterminated quoted literal.");
				AppendNameChar (c, ref index);
				break;
			}

			return new string (nameBuffer, 0, index);
		}

		private void AppendNameChar (int c, ref int index)
		{
			if (nameBuffer.Length == index) {
				char [] arr = new char [index * 2];
				Array.Copy (nameBuffer, arr, index);
				nameBuffer = arr;
			}
			if (c > 0x10000) {
				AppendNameChar ((c - 0x10000) / 0x400 + 0xD800, ref index);
				AppendNameChar ((c - 0x10000) % 0x400 + 0xDC00, ref index);
			}
			else
				nameBuffer [index++] = (char) c;
		}

		// taken from System.Json/JsonReader.cs
		double ReadNumericLiteral ()
		{
			bool negative = false;
			/*
			if (PeekChar () == '-') {
				negative = true;
				ReadChar ();
				if (PeekChar () < 0)
					throw ParserException ("Invalid numeric literal; extra negation");
			}
			*/

			int c;
			int val = 0;
			int x = 0;
			bool zeroStart = PeekChar () == '0';
			for (; ; x++) {
				c = PeekChar ();
				if (c < '0' || '9' < c)
					break;
				val = val * 10 + (c - '0');
				ReadChar ();
				if (zeroStart && x == 1 && c == '0')
					throw new ParserException ("leading multiple zeros are not allowed");
			}

			// fraction

			bool hasFrac = false;
			decimal frac = 0;
			int fdigits = 0;
			if (PeekChar () == '.') {
				hasFrac = true;
				ReadChar ();
				if (PeekChar () < 0)
					throw new ParserException ("Invalid numeric literal; extra dot");
				decimal d = 10;
				while (true) {
					c = PeekChar ();
					if (c < '0' || '9' < c)
						break;
					ReadChar ();
					frac += (c - '0') / d;
					d *= 10;
					fdigits++;
				}
				if (fdigits == 0)
					throw new ParserException ("Invalid numeric literal; extra dot");
			}
			frac = Decimal.Round (frac, fdigits);

			c = PeekChar ();
			if (c != 'e' && c != 'E') {
				if (!hasFrac)
					return negative ? -val : val;
				var v = val + frac;
				return (double) (negative ? -v : v);
			}

			// exponent

			ReadChar ();

			int exp = 0;
			if (PeekChar () < 0)
				throw new ParserException ("Invalid numeric literal; incomplete exponent");
			bool negexp = false;
			c = PeekChar ();
			if (c == '-') {
				ReadChar ();
				negexp = true;
			}
			else if (c == '+')
				ReadChar ();

			if (PeekChar () < 0)
				throw new ParserException ("Invalid numeric literal; incomplete exponent");
			while (true) {
				c = PeekChar ();
				if (c < '0' || '9' < c)
					break;
				exp = exp * 10 + (c - '0');
				ReadChar ();
			}
			// it is messy to handle exponent, so I just use Decimal.Parse() with assured format.
			int [] bits = Decimal.GetBits (val + frac);
			return (double) new Decimal (bits [0], bits [1], bits [2], negative, (byte) exp);
		}

		private string ReadOneName ()
		{
			int index = 0;
			bool loop = true;
			int c = PeekChar ();
			if (!IsFirstNameChar ((char) c))
				throw new ParserException (String.Format ("Invalid NCName start character: {0}", c));
			do {
				c = PeekChar ();
				switch (c) {
				case -1:
				case ' ':
				case '\t':
				case '\r':
				case '\n':
					ReadChar ();
					loop = false;
					break;
				default:
					if (!IsNameChar ((char) c)) {
						loop = false;
						break;
					}

					ReadChar ();
					if (nameBuffer.Length == index) {
						char [] arr = new char [index * 2];
						Array.Copy (nameBuffer, arr, index);
						nameBuffer = arr;
					}
					nameBuffer [index++] = (char) c;
					break;
				}
			} while (loop);

			return new string (nameBuffer, 0, index);
		}

		private string ReadLine ()
		{
			if (peek_char != 0)
				throw new ParserException ("Internal error: ReadLine() is called at inappropriate state");
			string s = source.ReadLine ();
			line++;
			column = 1;
			return s;
		}

		private int ParseToken (bool backslashed)
		{
			SkipWhitespaces ();
			int c = ReadChar ();
			string name;
			switch (c) {
			case -1:
				return Token.EOF;
			case '=':
				if (PeekChar () != '=')
					return Token.EQUAL;
				ReadChar ();
				return Token.EQUAL2;
			case ',':
				return Token.COMMA;
			case '.':
				return Token.DOT;
			case ':':
				return Token.COLON;
			case ';':
				return Token.SEMICOLON;
			case '!':
				if (PeekChar () != '=')
					return Token.EXCLAIM;
				ReadChar ();
				return Token.EXCLAIM_EQUAL;
			case '{':
				return Token.OPEN_CURLY;
			case '}':
				return Token.CLOSE_CURLY;
			case '(':
				return Token.OPEN_PAREN;
			case ')':
				return Token.CLOSE_PAREN;
			case '[':
				SkipWhitespaces ();
				if (PeekChar () != ']')
					return Token.OPEN_BRACE;
				ReadChar ();
				// it is special token to distinguish array declaration and array access.
				// FIXME: it should actually be implemented to
				// save a token cache and thus handle comments
				// (i.e. such as foo [/*snip*/] )
				return Token.OPEN_BRACE_CLOSE_BRACE;
			case ']':
				return Token.CLOSE_BRACE;
			case '&':
				if (PeekChar () != '&')
					return Token.AND;
				ReadChar ();
				return Token.AND2;
			case '|':
				if (PeekChar () != '|')
					return Token.BAR;
				ReadChar ();
				return Token.BAR2;
			case '?':
				return Token.QUESTION;
			case '\\':
				if (backslashed)
					return Token.ERROR;
				return ParseToken (true);
			case '+':
				switch (PeekChar ()) {
				case '+':
					ReadChar ();
					return Token.PLUS2;
				case '=':
					ReadChar ();
					return Token.PLUS_EQUAL;
				default:
					return Token.PLUS;
				}
			case '-':
				switch (PeekChar ()) {
				case '-':
					ReadChar ();
					return Token.MINUS2;
				case '=':
					ReadChar ();
					return Token.MINUS_EQUAL;
				default:
					return Token.MINUS;
				}
			case '*':
				return Token.ASTERISK;
			case '/':
				if (PeekChar () == '/') {
					// single line comment
					ReadChar ();
					ReadLine ();
					return ParseToken (false);
				}
				if (PeekChar () == '*') {
					ReadChar ();
					ConsumeMultilineComment ();
					return ParseToken (false);
				}
				return Token.SLASH;
			case '%':
				return Token.PERCENT;
			case '<':
				switch (PeekChar ()) {
				case '-':
					ReadChar ();
					return Token.OPEN_ANGLE2;
				case '=':
					ReadChar ();
					return Token.OPEN_ANGLE_EQUAL;
				default:
					return Token.OPEN_ANGLE;
				}
			case '>':
				switch (PeekChar ()) {
				case '-':
					ReadChar ();
					return Token.CLOSE_ANGLE2;
				case '=':
					ReadChar ();
					return Token.CLOSE_ANGLE_EQUAL;
				default:
					return Token.CLOSE_ANGLE;
				}
			case '\'':
				c = PeekChar ();
				if (c == '\'')
					throw new ParserException ("Invalid character literal");
				if (IsNumericStart ((char) c)) {
					double v = ReadNumericLiteral ();
					if (v != (double) (int) v)
						throw new ParserException ("Invalid character literal: float is not allowed");
					current_value = (int) v;
				} else
					current_value = c;
				return Token.CHARACTER_LITERAL;
			case '\"':
				current_value = ReadStringLiteral ();
				return Token.STRING_LITERAL;
			default:
				peek_char = c;
				if (IsNumericStart ((char) c)) {
					current_value = ReadNumericLiteral ();
					return Token.NUMERIC_LITERAL;
				}
				name = ReadOneName ();
				current_value = name;
				switch (name) {
				case "class":
					return Token.CLASS;
				case "extends":
					return Token.EXTENDS;
				case "implements":
					return Token.IMPLEMENTS;
				case "while":
					return Token.WHILE;
				case "break":
					return Token.BREAK;
				case "return":
					return Token.RETURN;
				case "if":
					return Token.IF;
				case "else":
					return Token.ELSE;
				case "for":
					return Token.FOR;
				case "switch":
					return Token.SWITCH;
				case "case":
					return Token.CASE;
				case "default":
					return Token.DEFAULT;
				case "new":
					return Token.NEW;
				case "true":
					return Token.TRUE;
				case "false":
					return Token.FALSE;
				case "this":
					return Token.THIS;
				case "super":
					return Token.SUPER;
				case "null":
					return Token.NULL;
				default:
					return Token.IDENTIFIER;
				}
			}
		}

		private void ConsumeMultilineComment ()
		{
			if (ReadChar () == '*') {
				// documentation comment. For now skip it.
				// If it's just "/**/", so no comment. Do not expect */ for this case.
				if (PeekChar () == '/') {
					ReadChar ();
					return;
				}
			}
			while (true) {
				int c = ReadChar ();
				if (c == '*') {
					if (PeekChar () == '/') {
						ReadChar ();
						return;
					}
				} else if (c < 0)
					throw new ParserException (String.Format ("Unmatched code comment started at line {0}, column {1}", saved_line, saved_column));
			}
		}

		// copied from cs-tokenizer.cs

		private bool IsFirstNameChar (char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || Char.IsLetter (c);
		}

		private bool IsNameChar (char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9') ||
				Char.IsLetter (c) || Char.GetUnicodeCategory (c) == UnicodeCategory.ConnectorPunctuation;
		}

		private bool IsNumericStart (char c)
		{
			return '0' <= c && c <= '9';
		}
	}
}
