using System;
using System.Globalization;
using System.Linq;
using Android.Graphics;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		public int unbinary (string str)
		{
			int v = 0;
			for (int i = 0; i < str.Length; i++) {
				v <<= 1;
				if (str [i] == '1')
					v++;
				else if (str [i] != '0')
					throw new FormatException (String.Format ("Invalid binary string: {0}", str));
			}
			return v;
		}

		public char @char (int value)
		{
			return (char) value;
		}

		public char [] @char (int [] value)
		{
			char [] arr = new char [value.Length];
			for (int i = 0; i < value.Length; i++)
				arr [i] = @char (value [i]);
			return arr;
		}

		public char [] @char (sbyte [] value)
		{
			char [] arr = new char [value.Length];
			for (int i = 0; i < value.Length; i++)
				arr [i] = @char (value [i]);
			return arr;
		}

		public int @int (double value)
		{
			return (int) value;
		}

		public int @int (char value)
		{
			return (int) value;
		}

		public int @int (sbyte value)
		{
			return (int) value;
		}

		public int @int (string value) // including String
		{
			return int.Parse (value, NumberStyles.Integer);
		}

		public int [] @int (double [] values)
		{
			int [] arr = new int [values.Length];
			for (int i = 0; i < values.Length; i++)
				arr [i] = @int (values [i]);
			return arr;
		}

		public int [] @int (char [] values)
		{
			int [] arr = new int [values.Length];
			for (int i = 0; i < values.Length; i++)
				arr [i] = @int (values [i]);
			return arr;
		}

		public int [] @int (sbyte [] values)
		{
			int [] arr = new int [values.Length];
			for (int i = 0; i < values.Length; i++)
				arr [i] = @int (values [i]);
			return arr;
		}

		public int [] @int (PString [] values) // including String
		{
			int [] arr = new int [values.Length];
			for (int i = 0; i < values.Length; i++)
				arr [i] = @int (values [i]);
			return arr;
		}

		public string hex (char c)
		{
			return hex ((int) c);
		}

		public string hex (sbyte c)
		{
			return hex ((int) c);
		}

		public string hex (int i)
		{
			return i.ToString ("X");
		}

		public string hex (Color c)
		{
			return hex (c, 8);
		}

		public string hex (char c, int digits)
		{
			return hex ((int) c, digits);
		}

		public string hex (sbyte c, int digits)
		{
			return hex ((int) c);
		}

		public string hex (int i, int digits)
		{
			// FIXME: there should be better way
			return String.Format ("{0:X" + digits + "}", i);
		}

		public string hex (Color c, int digits)
		{
			// FIXME: dunno how digits is treated here.
			return String.Format ("{0:X02}{1:X02}{2:X02}{3:X02}", c.R, c.G, c.B, c.A).Substring (0, digits);
		}

		public string binary (sbyte value)
		{
			return binary ((int) value);
		}
		public string binary (char value)
		{
			return binary ((int) value);
		}
		public string binary (int value)
		{
			bool neg = false;
			if (value < 0) {
				neg = true;
				value = ~value + 1;
			}
			int digits = 0;
			for (int x = value; x > 0; x >>= 1)
				digits++;
			char [] arr = new char [digits];
			for (int i = 0; i < digits; i++)
				arr [i] = (value & (1 << i)) != 0 ? '1' : '0';
			if (neg)
				arr [0] = '1';
			return new string (arr);
		}
		public string binary (Color value)
		{
			return binary (value, 8);
		}

		public string binary (sbyte value, int digits)
		{
			return binary ((int) value, digits);
		}
		public string binary (char value, int digits)
		{
			return binary ((int) value, digits);
		}
		public string binary (int value, int digits)
		{
			string s = binary (value);
			if (s.Length > digits)
				return s.Substring (s.Length - digits, digits);
			else
				return s;
		}
		public string binary (Color value, int digits)
		{
			if (digits == 6)
				return binary (value.R << 16 + value.G << 8 + value.B);
			else
				return binary (value.R << 24 + value.G << 16 + value.B << 8 + value.A);
		}

		public int unhex (string s)
		{
			return int.Parse (s, NumberStyles.HexNumber);
		}

		public sbyte @byte (char c)
		{
			return (sbyte) c;
		}

		public sbyte @byte (int i)
		{
			return (sbyte) i;
		}

		public sbyte @byte (char [] arr)
		{
			throw new NotImplementedException ();
		}

		public sbyte @byte (int [] arr)
		{
			throw new NotImplementedException ();
		}

		public string str (bool b)
		{
			return b ? "true" : "false";
		}

		public string str (sbyte b)
		{
			return b.ToString (CultureInfo.InvariantCulture);
		}

		public string str (char c)
		{
			return c.ToString ();
		}

		public string str (double d)
		{
			return d.ToString (CultureInfo.InvariantCulture);
		}

		public string str (int i)
		{
			return i.ToString (CultureInfo.InvariantCulture);
		}

		public string str (bool [] arr)
		{
			throw new NotImplementedException ();
		}

		public string str (sbyte [] arr)
		{
			throw new NotImplementedException ();
		}

		public string str (char [] arr)
		{
			throw new NotImplementedException ();
		}

		public string str (double [] arr)
		{
			throw new NotImplementedException ();
		}

		public string str (int [] arr)
		{
			throw new NotImplementedException ();
		}

		public bool boolean (string s)
		{
			switch (s) {
			case "true": return true;
			case "false": return false;
			}
			throw new FormatException (String.Format ("Invalid boolean string: {0}", s));
		}

		public bool boolean (int v)
		{
			return v != 0;
		}

		public bool boolean (double v)
		{
			return v != 0.0;
		}
		
		public double @float (int v)
		{
			return (double) v;
		}
		public double @float (char v)
		{
			return (double) (int) v;
		}
		public double @float (sbyte v)
		{
			return (double) v;
		}
		public double @float (bool v)
		{
			return v ? 1 : 0;
		}
		public double @float (string v)
		{
			return double.Parse (v, CultureInfo.InvariantCulture);
		}
		public double [] @float (int [] v)
		{
			var ret = new double [v.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = (double) v [i];
			return ret;
		}
		public double [] @float (char [] v)
		{
			var ret = new double [v.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = (double) v [i];
			return ret;
		}
		public double [] @float (sbyte [] v)
		{
			var ret = new double [v.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = (double) v [i];
			return ret;
		}
		public double [] @float (bool [] v)
		{
			var ret = new double [v.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = v [i] ? 1 : 0;
			return ret;
		}
		public double [] @float (PString [] v)
		{
			var ret = new double [v.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = double.Parse (v [i], NumberFormatInfo.InvariantInfo);
			return ret;
		}
	}
}
