using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PString = System.String;

namespace ProcessingCli
{
	public static partial class StandardLibrary
	{
		// This source contains the "hidden" APIs that are not
		// in the reference documentation.

		[ProcessingStandardField]
		public static readonly double MAX_FLOAT = float.MaxValue;
		[ProcessingStandardField]
		public static readonly double MIN_FLOAT = -MAX_FLOAT; // it indeed is.

		public static int parseInt (string s)
		{
			return int.Parse (s, NumberFormatInfo.InvariantInfo);
		}
		
		public static int [] parseInt (PString [] arr)
		{
			int [] ret = new int [arr.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = parseInt (arr [i]);
			return ret;
		}

		public static double parseFloat (string s)
		{
			return double.Parse (s, NumberFormatInfo.InvariantInfo);
		}
		
		public static double [] parseFloat (PString [] arr)
		{
			double [] ret = new double [arr.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = parseFloat (arr [i]);
			return ret;
		}
	}
}
