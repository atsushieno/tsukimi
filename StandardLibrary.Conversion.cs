using System;
using System.Windows;
using System.Windows.Media;

namespace ProcessingDlr
{
	public partial class StandardLibrary
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
		}

		public char @char (int value)
		{
			return new char (value);
		}

		public char @char (int [] value)
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
		}

		public char @char (sbyte [] value)
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
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

		public int @int (double [] values)
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
		}

		public int @int (char [] values)
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
		}

		public int @int (sbyte [] values)
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
		}

		public int @int (string [] values) // including String
		{
			// FIXME: what should it do?
			throw new NotImplementedException ();
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
			return i.ToString (NumberStyles.HexNumber);
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
			return String.Format ("{0:X02}{1:X02}{2:X02}{3:X02}", c.Red, c.Green, c.Blue, c.Alpha).Substring (0, digits);
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
			if (value == int.MinValue)
				return "10000000000000000000000000000000";
			bool neg = false;
			if (value < 0) {
				neg = true;
				value = -value;
			}
			int digits = 0;
			for (int x = value; x > 0; x >>= 1)
				digits++;
			char [] arr = new char [digits];
			for (int i = 0; i < digits; i++)
				arr [i] = value & (1 << i) != 0 ? '1' : '0';
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

		public double @float (int v)
		{
			return (double) v;
		}
		public double @float (char v)
		{
			return (double) v;
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
			return double.Parse (CultureInfo.InvariantCulture);
		}
		public double @float (int [] v)
		{
			throw new NotImplementedException ();
		}
		public double @float (char [] v)
		{
			throw new NotImplementedException ();
		}
		public double @float (sbyte [] v)
		{
			throw new NotImplementedException ();
		}
		public double @float (bool [] v)
		{
			throw new NotImplementedException ();
		}
		public double @float (string [] v)
		{
			throw new NotImplementedException ();
		}
	}
}
