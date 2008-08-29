using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ProcessingDlr
{
	public static partial class StandardLibrary
	{
		// String functions

		public static string [] split (string s, char token)
		{
			return s.Split (token);
		}

		public static string [] split (string s, string token)
		{
			return s.Split (s, token);
		}

		public static string join (string [] arr, string joiner)
		{
			return String.Join (arr, joiner);
		}

		static readonly char [] wsChars = {' ', '\n', '\r', '\t'};

		public static string splitTokens (string s)
		{
			return s.Split (wsChars);
		}

		public static string splitTokens (string s, string token)
		{
			return s.Split (s, token);
		}

		public static string nf (int n, int digits)
		{
			return n.ToString (CultureInfo.InvariantCulture, "{0," + digits + "}");
		}

		public static string nf (double n, int left, int right)
		{
			return n.ToString (CultureInfo.InvariantCulture, "{0," + left + "," + right + "}");
		}

		public static string match (string str, string regexp)
		{
			var m = Regex.Matches (str, regexp);
			string [] res = new string [m.Count];
			for (int i = 0; i < m.Count; i++)
				res [i] = m [i].Value;
			return res;
		}

		public static string trim (string str)
		{
			return str.Trim ();
		}

		public static void trim (string [] arr)
		{
			for (int i = 0; i < arr.Length; i++)
				arr [i] = trim (arr [i]);
		}

		public static string nfc (int i)
		{
			return i.ToString ("N");
		}

		public static string nfc (double d, int right)
		{
			return d.ToString ("N");
		}
/*
		nfs()
		nfp()
*/
	}
}
