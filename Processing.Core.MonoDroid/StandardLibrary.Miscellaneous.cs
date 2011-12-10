using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Android.Graphics;

namespace ProcessingCli
{
	public static class ProcessingExtensions
	{
		public static int charAt (this string s, int index)
		{
			return s [index];
		}

		public static bool equals (this string ss, String s)
		{
			return ss == s;
		}

		public static int indexOf (this string ss, String s)
		{
			return indexOf (ss, s, 0);
		}

		public static int indexOf (this string ss, String s, int startIndex)
		{
			return ss.IndexOf (s, startIndex, StringComparison.Ordinal);
		}

		public static int length (this string s)
		{
			return s.Length;
		}

		public static string substring (this string s, int start)
		{
			return s.Substring (start);
		}

		public static string substring (this string s, int start, int end)
		{
			return s.Substring (start, end - start);
		}

		public static string toLowerCase (this string s)
		{
			return s.ToLower (CultureInfo.InvariantCulture);
		}

		public static string toUpperCase (this string s)
		{
			return s.ToUpper (CultureInfo.InvariantCulture);
		}

		// FIXME: the specification requires that String implements
		// all methods in J2SE java.lang.String.

		public static bool startsWith (this string ss, String s)
		{
			return ss.StartsWith (s, StringComparison.Ordinal);
		}

		public static bool endsWith (this string ss, String s)
		{
			return ss.EndsWith (s, StringComparison.Ordinal);
		}
		
		public static string replace (this string ss, char oldChar, char newChar)
		{
			return ss.Replace (oldChar, newChar);
		}

		public static string replaceAll (this string ss, String regex, String replacement)
		{
			return new Regex (regex).Replace (ss, replacement, int.MaxValue, 0);
		}
	}

	public class PrintWriter
	{
		TextWriter writer;

		internal PrintWriter (TextWriter writer)
		{
			this.writer = writer;
		}

		public void print (object obj)
		{
			writer.Write (obj);
		}

		public void println ()
		{
			writer.WriteLine ();
		}

		public void println (object obj)
		{
			writer.WriteLine (obj);
		}

		public void flush ()
		{
			writer.Flush ();
		}

		public void close ()
		{
			writer.Close ();
		}
	}
	
	public class ProcessingUtility
	{
		// e.g.:
		// a = new int [3][4][5] => a = new int [3][][];
		// for (int i=0;i<3;i++) a[i]=C(typeof(int),1, dims);
			// a = new int [4][];
			// for (int i=0;i<4;i++) a[i]=C(typeof(int), 2, dims);
				// a = new int[5];
		public static object CreateMultiDimentionArray (Type type, int dimIdx, params int [] dims)
		{
			if (dims [dimIdx] < 0)
				return null; // not initialized

			if (dimIdx + 1 == dims.Length)
				return Array.CreateInstance (type, dims [dimIdx]);
			else {
				Array a = Array.CreateInstance (type.MakeArrayType (), dims [dimIdx]);
				for (int i = 0; i < dims [dimIdx]; i++)
					a.SetValue (CreateMultiDimentionArray (type, dimIdx + 1, dims), i);
				return a;
			}
		}

		public static Stream OpenRead (string s)
		{
			return ProcessingApplication.Current.Assets.Open (s);
		}
		
		public static Stream OpenWrite (string s)
		{
			var isf = IsolatedStorageFile.GetUserStoreForApplication ();
			return new IsolatedStorageFileStream (s, FileMode.Create, isf);
		}
	}

	public enum SizeMode
	{
		Processing2D,
		Processing3D,
		Java2D,
		OpenGL
	}

	public enum Constants
	{
		// Align
		Left,
		Center,
		Right,
		// YAlign
		Top,
		Bottom,
		//Center, // identical!
		Baseline,
		// shape mode
		//Center, // identical
		Radius,
		Corner,
		Corners,
		// stroke join
		Miter,
		Bevel,
		Round, // also used for stroke cap
		// stroke cap
		Project,
		Square,
	}

	public class ProcessingStandardFieldAttribute : Attribute
	{
	}
}
