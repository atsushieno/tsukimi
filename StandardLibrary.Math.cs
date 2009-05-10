using System;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		public int min (int value1, int value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		public int min (int value1, int value2, int value3)
		{
			return min (min (value1, value2), value3);
		}

		public int min (int [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			int x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x > arr [i])
					x = arr [i];
			return x;
		}

		public double min (double value1, double value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		public double min (double value1, double value2, double value3)
		{
			return min (min (value1, value2), value3);
		}

		public double min (double [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			double x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x > arr [i])
					x = arr [i];
			return x;
		}

		public int max (int value1, int value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		public int max (int value1, int value2, int value3)
		{
			return max (max (value1, value2), value3);
		}

		public int max (int [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			int x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x < arr [i])
					x = arr [i];
			return x;
		}

		public double max (double value1, double value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		public double max (double value1, double value2, double value3)
		{
			return max (max (value1, value2), value3);
		}

		public double max (double [] arr)
		{
			if (arr.Length == 0)
				throw new ArgumentException ("no array item");
			double x = arr [0];
			for (int i = 1; i < arr.Length; i++)
				if (x < arr [i])
					x = arr [i];
			return x;
		}

		public int round (double value)
		{
			return (int) Math.Round (value);
		}

		/*
		public int dist (int x1, int y1, int x2, int y2)
		{
			return (int) Math.Sqrt ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
		}

		public int dist (int x1, int y1, int z1, int x2, int y2, int z2)
		{
			throw new NotImplementedException ();
		}
		*/

		public double dist (double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
		}

		public double dist (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			throw new NotImplementedException ();
		}

		public double exp (double value)
		{
			return Math.Exp (value);
		}

		public double pow (double x, double y)
		{
			return Math.Pow (x, y);
		}

		public int floor (double value)
		{
			return (int) Math.Floor (value);
		}

		public double sqrt (double value)
		{
			return Math.Sqrt (value);
		}

		public int abs (int value)
		{
			return value < 0 ? -value : value;
		}

		public double abs (double value)
		{
			return value < 0 ? -value : value;
		}

		public int constrain (int value, int min, int max)
		{
			if (value < min)
				return min;
			if (max < value)
				return max;
			return value;
		}

		public double constrain (double value, double min, double max)
		{
			if (value < min)
				return min;
			if (max < value)
				return max;
			return value;
		}

		public double norm (double value, double low, double high)
		{
			return map (value, low, high, 0, 1);
		}

		public double lerp (double v1, double v2, double amt)
		{
			return v1 + (v2 - v1) * amt;
		}
/*
		mag()
		log()
*/

		public int sq (int value)
		{
			return value * value;
		}

		public double sq (double value)
		{
			return value * value;
		}

		public int ceil (double value)
		{
			return (int) Math.Ceiling (value);
		}

		public double map (double value, double low1, double high1, double low2, double high2)
		{
			return (value - low1) * (high2 - low2) / (high1 - low1) + low2;
		}

		public double acos (double d)
		{
			return Math.Acos (d);
		}

		public double tan (double d)
		{
			return Math.Tan (d);
		}

		public double sin (double d)
		{
			return Math.Sin (d);
		}

		public double cos (double d)
		{
			return Math.Cos (d);
		}

		public double degrees (double d)
		{
			throw new NotImplementedException ();
		}

		public double atan2 (double y, double x)
		{
			return Math.Atan2 (y, x);
		}

		public double atan (double d)
		{
			return Math.Atan (d);
		}

		public double radians (double d)
		{
			return PI * d / 180.0;
		}

		public double asin (double d)
		{
			return Math.Asin (d);
		}

/*
		noise()
		noiseSeed()
		randomSeed()
		noiseDetail()
*/
		readonly Random rnd = new Random ();

		public int random (int high)
		{
			lock (rnd)
				return rnd.Next (high);
		}

		public int random (int low, int high)
		{
			lock (rnd)
				return rnd.Next (low, high);
		}

		public double random (double high)
		{
			lock (rnd)
				return rnd.NextDouble () * high;
		}

		public double random (double low, double high)
		{
			lock (rnd)
				// FIXME: am I sure?
				return (high - low) * rnd.NextDouble () + low;
		}
	}
}
