using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

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

		public static double parseFloat (string s)
		{
			return double.Parse (s, NumberFormatInfo.InvariantInfo);
		}
	}
}
