using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

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

		public static Array reverse (Array array)
		{
			var a = Create (array, array.Length);
			Array.Copy (array, a, array.Length);
			Array.Reverse (a, 0, a.Length);
			return a;
		}

		public static Array shorten (Array array)
		{
			var a = Create (array, array.Length - 1);
			Array.Copy (array, a, array.Length - 1);
			return a;
		}

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
	}
}
