using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using PString = System.String;

namespace ProcessingCli
{
	public static partial class StandardLibrary
	{
/*
		// Array Functions

		append()
		arraycopy()
		concat()
		expand()
		reverse()
		shorten()
		sort()
		splice()
		subset()
*/
		static Array Create (Array src, int length)
		{
			return Array.CreateInstance (src.GetType ().GetElementType (), length);
		}

		public static Array append (Array array, object element)
		{
			var a = Create (array, array.Length + 1);
			Array.Copy (array, a, array.Length);
			a.SetValue (element, a.Length - 1);
			return a;
		}

		// typed overloads
		public static bool [] append (bool [] array, object element) { return (bool []) append ((Array) array, element); }
		public static byte [] append (byte [] array, object element) { return (byte []) append ((Array) array, element); }
		public static char [] append (char [] array, object element) { return (char []) append ((Array) array, element); }
		public static int [] append (int [] array, object element) { return (int []) append ((Array) array, element); }
		public static double [] append (double [] array, object element) { return (double []) append ((Array) array, element); }
		public static PString [] append (PString [] array, object element) { return (PString []) append ((Array) array, element); }

		public static void arrayCopy (Array source, Array dest)
		{
			arrayCopy (source, dest, source.Length);
		}

		public static void arrayCopy (Array source, Array dest, int length)
		{
			arrayCopy (source, 0, dest, 0, length);
		}

		public static void arrayCopy (Array source, int sourcePos, Array dest, int destPos, int length)
		{
			Array.Copy (source, sourcePos, dest, destPos, length);
		}

		public static Array concat (Array a1, Array a2)
		{
			var a = Create (a1, a1.Length + a2.Length);
			Array.Copy (a1, a, a1.Length);
			Array.Copy (a2, 0, a, a1.Length, a2.Length);
			return a;
		}

		// typed overloads
		public static bool [] concat (bool [] a1, bool [] a2) { return (bool []) concat ((Array) a1, a2); }
		public static byte [] concat (byte [] a1, byte [] a2) { return (byte []) concat ((Array) a1, a2); }
		public static char [] concat (char [] a1, char [] a2) { return (char []) concat ((Array) a1, a2); }
		public static int [] concat (int [] a1, int [] a2) { return (int []) concat ((Array) a1, a2); }
		public static double [] concat (double [] a1, double [] a2) { return (double []) concat ((Array) a1, a2); }
		public static PString [] concat (PString [] a1, PString [] a2) { return (PString []) concat ((Array) a1, a2); }


		public static Array expand (Array a1)
		{
			return expand (a1, a1.Length * 2);
		}

		public static Array expand (Array a1, int size)
		{
			var a = Create (a1, size);
			Array.Copy (a1, a, a1.Length);
			return a;
		}

		// typed overloads
		public static bool [] expand (bool [] a1) { return (bool []) expand ((Array) a1); }
		public static byte [] expand (byte [] a1) { return (byte []) expand ((Array) a1); }
		public static char [] expand (char [] a1) { return (char []) expand ((Array) a1); }
		public static int [] expand (int [] a1) { return (int []) expand ((Array) a1); }
		public static double [] expand (double [] a1) { return (double []) expand ((Array) a1); }
		public static PString [] expand (PString [] a1) { return (PString []) expand ((Array) a1); }

		public static bool [] expand (bool [] a1, int size) { return (bool []) expand ((Array) a1, size); }
		public static byte [] expand (byte [] a1, int size) { return (byte []) expand ((Array) a1, size); }
		public static char [] expand (char [] a1, int size) { return (char []) expand ((Array) a1, size); }
		public static int [] expand (int [] a1, int size) { return (int []) expand ((Array) a1, size); }
		public static double [] expand (double [] a1, int size) { return (double []) expand ((Array) a1, size); }
		public static PString [] expand (PString [] a1, int size) { return (PString []) expand ((Array) a1, size); }

		public static Array reverse (Array array)
		{
			var a = Create (array, array.Length);
			Array.Copy (array, a, array.Length);
			Array.Reverse (a, 0, a.Length);
			return a;
		}

		// typed overloads
		public static bool [] reverse (bool [] array) { return (bool []) reverse ((Array) array); }
		public static byte [] reverse (byte [] array) { return (byte []) reverse ((Array) array); }
		public static char [] reverse (char [] array) { return (char []) reverse ((Array) array); }
		public static int [] reverse (int [] array) { return (int []) reverse ((Array) array); }
		public static double [] reverse (double [] array) { return (double []) reverse ((Array) array); }
		public static PString [] reverse (PString [] array) { return (PString []) reverse ((Array) array); }

		public static Array shorten (Array array)
		{
			var a = Create (array, array.Length - 1);
			Array.Copy (array, a, array.Length - 1);
			return a;
		}

		// typed overloads
		public static bool [] shorten (bool [] array) { return (bool []) shorten ((Array) array); }
		public static byte [] shorten (byte [] array) { return (byte []) shorten ((Array) array); }
		public static char [] shorten (char [] array) { return (char []) shorten ((Array) array); }
		public static int [] shorten (int [] array) { return (int []) shorten ((Array) array); }
		public static double [] shorten (double [] array) { return (double []) shorten ((Array) array); }
		public static PString [] shorten (PString [] array) { return (PString []) shorten ((Array) array); }

		public static Array sort (Array array)
		{
			return sort (array, array.Length);
		}

		public static Array sort (Array array, int count)
		{
			var a = Create (array, count);
			Array.Copy (array, a, count);
			Array.Sort (a, 0, a.Length, Comparer<object>.Default);
			return a;
		}

		// typed overloads
		public static bool [] sort (bool [] array) { return (bool []) sort ((Array) array); }
		public static byte [] sort (byte [] array) { return (byte []) sort ((Array) array); }
		public static char [] sort (char [] array) { return (char []) sort ((Array) array); }
		public static int [] sort (int [] array) { return (int []) sort ((Array) array); }
		public static double [] sort (double [] array) { return (double []) sort ((Array) array); }
		public static PString [] sort (PString [] array) { return (PString []) sort ((Array) array); }

		public static bool [] sort (bool [] array, int count) { return (bool []) sort ((Array) array, count); }
		public static byte [] sort (byte [] array, int count) { return (byte []) sort ((Array) array, count); }
		public static char [] sort (char [] array, int count) { return (char []) sort ((Array) array, count); }
		public static int [] sort (int [] array, int count) { return (int []) sort ((Array) array, count); }
		public static double [] sort (double [] array, int count) { return (double []) sort ((Array) array, count); }
		public static PString [] sort (PString [] array, int count) { return (PString []) sort ((Array) array, count); }
		
		public static Array splice (Array array, object value, int index)
		{
			var a = Create (array, array.Length + 1);
			Array.Copy (array, a, index);
			a.SetValue (value, index);
			Array.Copy (array, index, a, index + 1, array.Length - index);
			return a;
		}

		public static Array splice (Array a1, Array a2, int index)
		{
			var a = Create (a1, a1.Length + a2.Length);
			Array.Copy (a1, a, index);
			Array.Copy (a2, 0, a, index, a2.Length);
			Array.Copy (a1, index, a, index + a2.Length, a1.Length - index);
			return a;
		}
		
		// typed overloads
		public static bool [] splice (bool [] a1, bool value, int index) { return (bool []) splice ((Array) a1, value, index); }
		public static byte [] splice (byte [] a1, byte value, int index) { return (byte []) splice ((Array) a1, value, index); }
		public static char [] splice (char [] a1, char value, int index) { return (char []) splice ((Array) a1, value, index); }
		public static int [] splice (int [] a1, int value, int index) { return (int []) splice ((Array) a1, value, index); }
		public static double [] splice (double [] a1, double value, int index) { return (double []) splice ((Array) a1, value, index); }
		public static PString [] splice (PString [] a1, PString value, int index) { return (PString []) splice ((Array) a1, value, index); }

		public static bool [] splice (bool [] a1, bool [] a2, int index) { return (bool []) splice ((Array) a1, a2, index); }
		public static byte [] splice (byte [] a1, byte [] a2, int index) { return (byte []) splice ((Array) a1, a2, index); }
		public static char [] splice (char [] a1, char [] a2, int index) { return (char []) splice ((Array) a1, a2, index); }
		public static int [] splice (int [] a1, int [] a2, int index) { return (int []) splice ((Array) a1, a2, index); }
		public static double [] splice (double [] a1, double [] a2, int index) { return (double []) splice ((Array) a1, a2, index); }
		public static PString [] splice (PString [] a1, PString [] a2, int index) { return (PString []) splice ((Array) a1, a2, index); }

		public static Array subset (Array array, int offset)
		{
			return subset (array, offset, array.Length - offset);
		}

		public static Array subset (Array array, int offset, int length)
		{
			var a = Create (array, length);
			Array.Copy (array, offset, a, 0, length);
			return a;
		}
		
		// typed overloads
		public static bool [] subset (bool [] array, int offset) { return (bool []) subset ((Array) array, offset); }
		public static byte [] subset (byte [] array, int offset) { return (byte []) subset ((Array) array, offset); }
		public static char [] subset (char [] array, int offset) { return (char []) subset ((Array) array, offset); }
		public static int [] subset (int [] array, int offset) { return (int []) subset ((Array) array, offset); }
		public static double [] subset (double [] array, int offset) { return (double []) subset ((Array) array, offset); }
		public static PString [] subset (PString [] array, int offset) { return (PString []) subset ((Array) array, offset); }

		public static bool [] subset (bool [] array, int offset, int length) { return (bool []) subset ((Array) array, offset, length); }
		public static byte [] subset (byte [] array, int offset, int length) { return (byte []) subset ((Array) array, offset, length); }
		public static char [] subset (char [] array, int offset, int length) { return (char []) subset ((Array) array, offset, length); }
		public static int [] subset (int [] array, int offset, int length) { return (int []) subset ((Array) array, offset, length); }
		public static double [] subset (double [] array, int offset, int length) { return (double []) subset ((Array) array, offset, length); }
		public static PString [] subset (PString [] array, int offset, int length) { return (PString []) subset ((Array) array, offset, length); }
	}
}
