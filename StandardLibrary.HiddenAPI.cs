using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		// This source contains the "hidden" APIs that are not
		// in the reference documentation.

		[ProcessingStandardField]
		public const double MAX_FLOAT = float.MaxValue;
		[ProcessingStandardField]
		public const double MIN_FLOAT = -MAX_FLOAT; // it indeed is.

		public int parseInt (string s)
		{
			return int.Parse (s, NumberFormatInfo.InvariantInfo);
		}
		
		public int [] parseInt (PString [] arr)
		{
			int [] ret = new int [arr.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = parseInt (arr [i]);
			return ret;
		}

		public double parseFloat (string s)
		{
			return double.Parse (s, NumberFormatInfo.InvariantInfo);
		}
		
		public double [] parseFloat (PString [] arr)
		{
			double [] ret = new double [arr.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = parseFloat (arr [i]);
			return ret;
		}
	}
}
