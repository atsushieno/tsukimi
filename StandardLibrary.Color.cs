using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using PString = System.String;
using ColorMode = System.Int32;

namespace ProcessingCli
{
	public partial class StandardLibrary
	{

		/*
		public static void background (int gray)
		{
			background (color (gray));
		}

		public static void background (int gray, int alpha)
		{
			background (color (gray, alpha));
		}

		public static void background (int r, int g, int b)
		{
			background (color (r, g, b));
		}

		public static void background (int r, int g, int b, int a)
		{
			background (color (r, g, b, a));
		}
		*/

		public static void background (double gray)
		{
			background (color (gray));
		}

		public static void background (double gray, double alpha)
		{
			background (color (gray, alpha));
		}

		public static void background (double r, double g, double b)
		{
			background (color (r, g, b));
		}

		public static void background (double r, double g, double b, double a)
		{
			background (color (r, g, b, a));
		}

		public static void background (string hex)
		{
			background (color (hex));
		}

		public static void background (string hex, double alpha)
		{
			background (color (hex, alpha));
		}

		static void background (Color c)
		{
			Host.Background = new SolidColorBrush (c);
		}

		public static void colorMode (ColorMode mode)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range1, double range2, double range3)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range1, double range2, double range3, double range4)
		{
			throw new NotImplementedException ();
		}

		public static void stroke (double gray)
		{
			stroke_color = color (gray);
		}

		public static void stroke (double gray, double alpha)
		{
			stroke_color = color (gray, alpha);
		}

		public static void stroke (double r, double g, double b)
		{
			stroke_color = color (r, g, b);
		}

		public static void stroke (double r, double g, double b, double a)
		{
			stroke_color = color (r, g, b, a);
		}

		public static void stroke (string hex)
		{
			stroke_color = color (hex);
		}

		public static void stroke (string hex, double alpha)
		{
			stroke_color = color (hex, alpha);
		}

		public static void stroke (Color color)
		{
			stroke_color = color;
		}

		public static void stroke (Color color, double alpha)
		{
			stroke_color = Color.FromArgb ((byte) alpha, color.R, color.G, color.B);
		}

		public static void noFill ()
		{
			fill_color = null;
		}

		public static void noStroke ()
		{
			stroke_color = null;
		}

		/*
		public static void fill (int gray)
		{
			fill_color = color (gray);
		}

		public static void fill (int gray, int alpha)
		{
			fill_color = color (gray, alpha);
		}

		public static void fill (int r, int g, int b)
		{
			fill_color = color (r, g, b);
		}

		public static void fill (int r, int g, int b, int a)
		{
			fill_color = color (r, g, b, a);
		}
		*/

		public static void fill (double gray)
		{
			fill (color (gray));
		}

		public static void fill (double gray, double alpha)
		{
			fill (color (gray, alpha));
		}

		public static void fill (double r, double g, double b)
		{
			fill (color (r, g, b));
		}

		public static void fill (double r, double g, double b, double a)
		{
			fill (color (r, g, b, a));
		}

		public static void fill (string hex)
		{
			fill (color (hex));
		}

		public static void fill (string hex, double alpha)
		{
			fill (color (hex, alpha));
		}

		public static void fill (Color color)
		{
			fill_color = color;
		}
/*
*** Color

	function:

		*alpha()
		*red()
		*blue()
		*green()
		*color()
		blendColor()
		brightness()
		saturation()
		lerpColor()
		hue()
*/
		public static double alpha (Color value)
		{
			return value.A;
		}

		public static double red (Color value)
		{
			return value.R;
		}

		public static double green (Color value)
		{
			return value.G;
		}

		public static double blue (Color value)
		{
			return value.B;
		}

		public static Color color (double gray)
		{
			return color (gray, 0xFF);
		}

		public static Color color (double gray, double alpha)
		{
			return color (gray, gray, gray, alpha);
		}

		public static Color color (double r, double g, double b)
		{
			return color (r, g, b, 0xFF);
		}

		public static Color color (double r, double g, double b, double a)
		{
			return Color.FromArgb ((byte) (int) a, (byte) (int) r, (byte) (int) g, (byte) (int) b);
		}

		public static Color color (string hex)
		{
			uint v = uint.Parse (hex.Substring (1), NumberStyles.HexNumber);
			if (hex.Length == 7)
				v |= 0xFF000000;
			return Color.FromArgb ((byte) (v >> 24), (byte) ((v & 0xFF0000) >> 16), (byte) ((v & 0xFF00) >> 8), (byte) (v & 0xFF));
		}

		public static Color color (string hex, double alpha)
		{
			Color c = color (hex);
			c.A = (byte) alpha;
			return c;
		}

		public static Color lerpColor (Color c1, Color c2, double amt)
		{
			return Color.FromArgb ((byte) lerp (c1.A, c2.A, amt),
								    (byte) lerp (c1.R, c2.R, amt),
								    (byte) lerp (c1.G, c2.G, amt),
								    (byte) lerp (c1.B, c2.B, amt));
		}
	}
}
