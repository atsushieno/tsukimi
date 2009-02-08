using System;

namespace ProcessingCli
{
	public static partial class StandardLibrary
	{
		public static int min (int value1, int value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		public static int min (int value1, int value2, int value3)
		{
			return min (min (value1, value2), value3);
		}

		public static int min (int [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			int x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x > arr [i])
					x = arr [i];
			return x;
		}

		public static double min (double value1, double value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		public static double min (double value1, double value2, double value3)
		{
			return min (min (value1, value2), value3);
		}

		public static double min (double [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			double x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x > arr [i])
					x = arr [i];
			return x;
		}

		public static int max (int value1, int value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		public static int max (int value1, int value2, int value3)
		{
			return max (max (value1, value2), value3);
		}

		public static int max (int [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			int x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x < arr [i])
					x = arr [i];
			return x;
		}

		public static double max (double value1, double value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		public static double max (double value1, double value2, double value3)
		{
			return max (max (value1, value2), value3);
		}

		public static double max (double [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			double x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x < arr [i])
					x = arr [i];
			return x;
		}

		public static int round (double value)
		{
			return (int) Math.Round (value);
		}

		/*
		public static int dist (int x1, int y1, int x2, int y2)
		{
			return (int) Math.Sqrt ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
		}

		public static int dist (int x1, int y1, int z1, int x2, int y2, int z2)
		{
			throw new NotImplementedException ();
		}
		*/

		public static double dist (double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
		}

		public static double dist (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			throw new NotImplementedException ();
		}

		public static double exp (double value)
		{
			return Math.Exp (value);
		}

		public static double pow (double x, double y)
		{
			return Math.Pow (x, y);
		}

		public static int floor (double value)
		{
			return (int) Math.Floor (value);
		}

		public static double sqrt (double value)
		{
			return Math.Sqrt (value);
		}

		public static int abs (int value)
		{
			return value < 0 ? -value : value;
		}

		public static double abs (double value)
		{
			return value < 0 ? -value : value;
		}

		public static int constrain (int value, int min, int max)
		{
			if (value < min)
				return min;
			if (max < value)
				return max;
			return value;
		}

		public static double constrain (double value, double min, double max)
		{
			if (value < min)
				return min;
			if (max < value)
				return max;
			return value;
		}

		public static double norm (double value, double low, double high)
		{
			return map (value, low, high, 0, 1);
		}

		public static double lerp (double v1, double v2, double amt)
		{
			return v1 + (v2 - v1) * amt;
		}
/*
		mag()
		log()
		lerp()
*/

		public static int sq (int value)
		{
			return value * value;
		}

		public static double sq (double value)
		{
			return value * value;
		}

		public static int ceil (double value)
		{
			return (int) Math.Ceiling (value);
		}

		public static double map (double value, double low1, double high1, double low2, double high2)
		{
			return (value - low1) * (high2 - low2) / (high1 - low1) + low2;
		}

		public static double acos (double d)
		{
			return Math.Acos (d);
		}

		public static double tan (double d)
		{
			return Math.Tan (d);
		}

		public static double sin (double d)
		{
			return Math.Sin (d);
		}

		public static double cos (double d)
		{
			return Math.Cos (d);
		}

		public static double degrees (double d)
		{
			throw new NotImplementedException ();
		}

		public static double atan2 (double y, double x)
		{
			return Math.Atan2 (y, x);
		}

		public static double atan (double d)
		{
			return Math.Atan (d);
		}

		public static double radians (double d)
		{
			return PI * d / 180.0;
		}

		public static double asin (double d)
		{
			return Math.Asin (d);
		}

/*
		noise()
		noiseSeed()
		randomSeed()
		noiseDetail()
		random()
*/
	}
}
