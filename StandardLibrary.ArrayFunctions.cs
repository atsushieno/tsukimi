using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
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
		Array Create (Array src, int length)
		{
			return Array.CreateInstance (src.GetType ().GetElementType (), length);
		}

		public Array append (Array array, object element)
		{
			var a = Create (array, array.Length + 1);
			Array.Copy (array, a, array.Length);
			a.SetValue (element, a.Length - 1);
			return a;
		}

		// typed overloads
		public bool [] append (bool [] array, object element) { return (bool []) append ((Array) array, element); }
		public byte [] append (byte [] array, object element) { return (byte []) append ((Array) array, element); }
		public char [] append (char [] array, object element) { return (char []) append ((Array) array, element); }
		public int [] append (int [] array, object element) { return (int []) append ((Array) array, element); }
		public double [] append (double [] array, object element) { return (double []) append ((Array) array, element); }
		public PString [] append (PString [] array, object element) { return (PString []) append ((Array) array, element); }

		public void arrayCopy (Array source, Array dest)
		{
			arrayCopy (source, dest, source.Length);
		}

		public void arrayCopy (Array source, Array dest, int length)
		{
			arrayCopy (source, 0, dest, 0, length);
		}

		public void arrayCopy (Array source, int sourcePos, Array dest, int destPos, int length)
		{
			Array.Copy (source, sourcePos, dest, destPos, length);
		}

		public Array concat (Array a1, Array a2)
		{
			var a = Create (a1, a1.Length + a2.Length);
			Array.Copy (a1, a, a1.Length);
			Array.Copy (a2, 0, a, a1.Length, a2.Length);
			return a;
		}

		// typed overloads
		public bool [] concat (bool [] a1, bool [] a2) { return (bool []) concat ((Array) a1, a2); }
		public byte [] concat (byte [] a1, byte [] a2) { return (byte []) concat ((Array) a1, a2); }
		public char [] concat (char [] a1, char [] a2) { return (char []) concat ((Array) a1, a2); }
		public int [] concat (int [] a1, int [] a2) { return (int []) concat ((Array) a1, a2); }
		public double [] concat (double [] a1, double [] a2) { return (double []) concat ((Array) a1, a2); }
		public PString [] concat (PString [] a1, PString [] a2) { return (PString []) concat ((Array) a1, a2); }


		public Array expand (Array a1)
		{
			return expand (a1, a1.Length * 2);
		}

		public Array expand (Array a1, int size)
		{
			var a = Create (a1, size);
			Array.Copy (a1, a, a1.Length);
			return a;
		}

		// typed overloads
		public bool [] expand (bool [] a1) { return (bool []) expand ((Array) a1); }
		public byte [] expand (byte [] a1) { return (byte []) expand ((Array) a1); }
		public char [] expand (char [] a1) { return (char []) expand ((Array) a1); }
		public int [] expand (int [] a1) { return (int []) expand ((Array) a1); }
		public double [] expand (double [] a1) { return (double []) expand ((Array) a1); }
		public PString [] expand (PString [] a1) { return (PString []) expand ((Array) a1); }

		public bool [] expand (bool [] a1, int size) { return (bool []) expand ((Array) a1, size); }
		public byte [] expand (byte [] a1, int size) { return (byte []) expand ((Array) a1, size); }
		public char [] expand (char [] a1, int size) { return (char []) expand ((Array) a1, size); }
		public int [] expand (int [] a1, int size) { return (int []) expand ((Array) a1, size); }
		public double [] expand (double [] a1, int size) { return (double []) expand ((Array) a1, size); }
		public PString [] expand (PString [] a1, int size) { return (PString []) expand ((Array) a1, size); }

		public Array reverse (Array array)
		{
			var a = Create (array, array.Length);
			Array.Copy (array, a, array.Length);
			Array.Reverse (a, 0, a.Length);
			return a;
		}

		// typed overloads
		public bool [] reverse (bool [] array) { return (bool []) reverse ((Array) array); }
		public byte [] reverse (byte [] array) { return (byte []) reverse ((Array) array); }
		public char [] reverse (char [] array) { return (char []) reverse ((Array) array); }
		public int [] reverse (int [] array) { return (int []) reverse ((Array) array); }
		public double [] reverse (double [] array) { return (double []) reverse ((Array) array); }
		public PString [] reverse (PString [] array) { return (PString []) reverse ((Array) array); }

		public Array shorten (Array array)
		{
			var a = Create (array, array.Length - 1);
			Array.Copy (array, a, array.Length - 1);
			return a;
		}

		// typed overloads
		public bool [] shorten (bool [] array) { return (bool []) shorten ((Array) array); }
		public byte [] shorten (byte [] array) { return (byte []) shorten ((Array) array); }
		public char [] shorten (char [] array) { return (char []) shorten ((Array) array); }
		public int [] shorten (int [] array) { return (int []) shorten ((Array) array); }
		public double [] shorten (double [] array) { return (double []) shorten ((Array) array); }
		public PString [] shorten (PString [] array) { return (PString []) shorten ((Array) array); }

		public Array sort (Array array)
		{
			return sort (array, array.Length);
		}

		public Array sort (Array array, int count)
		{
			var a = Create (array, count);
			Array.Copy (array, a, count);
			Array.Sort (a, 0, a.Length, Comparer<object>.Default);
			return a;
		}

		// typed overloads
		public bool [] sort (bool [] array) { return (bool []) sort ((Array) array); }
		public byte [] sort (byte [] array) { return (byte []) sort ((Array) array); }
		public char [] sort (char [] array) { return (char []) sort ((Array) array); }
		public int [] sort (int [] array) { return (int []) sort ((Array) array); }
		public double [] sort (double [] array) { return (double []) sort ((Array) array); }
		public PString [] sort (PString [] array) { return (PString []) sort ((Array) array); }

		public bool [] sort (bool [] array, int count) { return (bool []) sort ((Array) array, count); }
		public byte [] sort (byte [] array, int count) { return (byte []) sort ((Array) array, count); }
		public char [] sort (char [] array, int count) { return (char []) sort ((Array) array, count); }
		public int [] sort (int [] array, int count) { return (int []) sort ((Array) array, count); }
		public double [] sort (double [] array, int count) { return (double []) sort ((Array) array, count); }
		public PString [] sort (PString [] array, int count) { return (PString []) sort ((Array) array, count); }
		
		public Array splice (Array array, object value, int index)
		{
			var a = Create (array, array.Length + 1);
			Array.Copy (array, a, index);
			a.SetValue (value, index);
			Array.Copy (array, index, a, index + 1, array.Length - index);
			return a;
		}

		public Array splice (Array a1, Array a2, int index)
		{
			var a = Create (a1, a1.Length + a2.Length);
			Array.Copy (a1, a, index);
			Array.Copy (a2, 0, a, index, a2.Length);
			Array.Copy (a1, index, a, index + a2.Length, a1.Length - index);
			return a;
		}
		
		// typed overloads
		public bool [] splice (bool [] a1, bool value, int index) { return (bool []) splice ((Array) a1, value, index); }
		public byte [] splice (byte [] a1, byte value, int index) { return (byte []) splice ((Array) a1, value, index); }
		public char [] splice (char [] a1, char value, int index) { return (char []) splice ((Array) a1, value, index); }
		public int [] splice (int [] a1, int value, int index) { return (int []) splice ((Array) a1, value, index); }
		public double [] splice (double [] a1, double value, int index) { return (double []) splice ((Array) a1, value, index); }
		public PString [] splice (PString [] a1, PString value, int index) { return (PString []) splice ((Array) a1, value, index); }

		public bool [] splice (bool [] a1, bool [] a2, int index) { return (bool []) splice ((Array) a1, a2, index); }
		public byte [] splice (byte [] a1, byte [] a2, int index) { return (byte []) splice ((Array) a1, a2, index); }
		public char [] splice (char [] a1, char [] a2, int index) { return (char []) splice ((Array) a1, a2, index); }
		public int [] splice (int [] a1, int [] a2, int index) { return (int []) splice ((Array) a1, a2, index); }
		public double [] splice (double [] a1, double [] a2, int index) { return (double []) splice ((Array) a1, a2, index); }
		public PString [] splice (PString [] a1, PString [] a2, int index) { return (PString []) splice ((Array) a1, a2, index); }

		public Array subset (Array array, int offset)
		{
			return subset (array, offset, array.Length - offset);
		}

		public Array subset (Array array, int offset, int length)
		{
			var a = Create (array, length);
			Array.Copy (array, offset, a, 0, length);
			return a;
		}
		
		// typed overloads
		public bool [] subset (bool [] array, int offset) { return (bool []) subset ((Array) array, offset); }
		public byte [] subset (byte [] array, int offset) { return (byte []) subset ((Array) array, offset); }
		public char [] subset (char [] array, int offset) { return (char []) subset ((Array) array, offset); }
		public int [] subset (int [] array, int offset) { return (int []) subset ((Array) array, offset); }
		public double [] subset (double [] array, int offset) { return (double []) subset ((Array) array, offset); }
		public PString [] subset (PString [] array, int offset) { return (PString []) subset ((Array) array, offset); }

		public bool [] subset (bool [] array, int offset, int length) { return (bool []) subset ((Array) array, offset, length); }
		public byte [] subset (byte [] array, int offset, int length) { return (byte []) subset ((Array) array, offset, length); }
		public char [] subset (char [] array, int offset, int length) { return (char []) subset ((Array) array, offset, length); }
		public int [] subset (int [] array, int offset, int length) { return (int []) subset ((Array) array, offset, length); }
		public double [] subset (double [] array, int offset, int length) { return (double []) subset ((Array) array, offset, length); }
		public PString [] subset (PString [] array, int offset, int length) { return (PString []) subset ((Array) array, offset, length); }
	}
}
