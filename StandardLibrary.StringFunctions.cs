using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProcessingCli
{
	public static partial class StandardLibrary
	{
		// String functions
		
		static PString [] Cast (string [] src)
		{
			PString [] arr = new PString[src.Length];
			for (int i = 0; i < arr.Length; i++)
				arr [i] = src [i];
			return arr;
		}

		public static PString [] split (string s, char token)
		{
			var a = s.Split (token);
			return Cast (a);
		}

		public static PString [] split (string s, string token)
		{
			var a = s.Split (new string [] {token}, StringSplitOptions.None);
			return Cast (a);
		}

		public static string join (PString [] arr, string joiner)
		{
			return String.Join (joiner, arr.Cast<string> ().ToArray ());
		}

		static readonly char [] wsChars = {' ', '\n', '\r', '\t'};

		public static PString [] splitTokens (string s)
		{
			var a = s.Split (wsChars);
			return Cast (a);
		}

		public static PString [] splitTokens (string s, string token)
		{
			var a = s.Split (token.ToCharArray ());
			return Cast (a);
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
