using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		// String functions
		
		PString [] Cast (string [] src)
		{
			PString [] arr = new PString[src.Length];
			for (int i = 0; i < arr.Length; i++)
				arr [i] = src [i];
			return arr;
		}

		public PString [] split (string s, char token)
		{
			var a = s.Split (token);
			return Cast (a);
		}

		public PString [] split (string s, string token)
		{
			var a = s.Split (new string [] {token}, StringSplitOptions.None);
			return Cast (a);
		}

		public string join (PString [] arr, string joiner)
		{
			return String.Join (joiner, arr.Cast<string> ().ToArray ());
		}

		readonly char [] wsChars = {' ', '\n', '\r', '\t'};

		public PString [] splitTokens (string s)
		{
			var a = s.Split (wsChars);
			return Cast (a);
		}

		public PString [] splitTokens (string s, string token)
		{
			var a = s.Split (token.ToCharArray ());
			return Cast (a);
		}

		public string nf (int n, int digits)
		{
			return n.ToString ("D0" + digits, CultureInfo.InvariantCulture);
		}

		public string nf (double n, int left, int right)
		{
			return n.ToString ("D0" + left + "," + right, CultureInfo.InvariantCulture);
		}

		public PString [] match (string str, string regexp)
		{
			var m = Regex.Matches (str, regexp);
			PString [] res = new PString [m.Count];
			for (int i = 0; i < m.Count; i++)
				res [i] = m [i].Value;
			return res;
		}

		public string trim (string str)
		{
			return str.Trim ();
		}

		public void trim (string [] arr)
		{
			for (int i = 0; i < arr.Length; i++)
				arr [i] = trim (arr [i]);
		}

		public string nfc (int i)
		{
			return i.ToString ("N");
		}

		public string nfc (double d, int right)
		{
			return d.ToString ("N");
		}
/*
		nfs()
		nfp()
*/
	}
}
