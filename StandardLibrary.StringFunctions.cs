using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProcessingCli
{
	public static partial class StandardLibrary
	{
		// String functions

		public static PString [] split (string s, char token)
		{
			var a = s.Split (token);
			return a.Cast<PString> ().ToArray ();
		}

		public static PString [] split (string s, string token)
		{
			var a = s.Split (new string [] {token}, StringSplitOptions.None);
			return a.Cast<PString> ().ToArray ();
		}

		public static string join (PString [] arr, string joiner)
		{
			return String.Join (joiner, arr.Cast<string> ().ToArray ());
		}

		static readonly char [] wsChars = {' ', '\n', '\r', '\t'};

		public static PString [] splitTokens (string s)
		{
			return s.Split (wsChars).Cast<PString> ().ToArray ();
		}

		public static PString [] splitTokens (string s, string token)
		{
			return s.Split (token.ToCharArray ()).Cast<PString> ().ToArray ();
		}

		public static string nf (int n, int digits)
		{
			return n.ToString ("{0," + digits + "}", CultureInfo.InvariantCulture);
		}

		public static string nf (double n, int left, int right)
		{
			return n.ToString ("{0," + left + "," + right + "}", CultureInfo.InvariantCulture);
		}

		public static PString [] match (string str, string regexp)
		{
			var m = Regex.Matches (str, regexp);
			PString [] res = new PString [m.Count];
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
