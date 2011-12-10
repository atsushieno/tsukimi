using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Android.Graphics;

using PString = System.String;
using ColorMode = System.Int32;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{

		/*
		public void background (int gray)
		{
			background (color (gray));
		}

		public void background (int gray, int alpha)
		{
			background (color (gray, alpha));
		}

		public void background (int r, int g, int b)
		{
			background (color (r, g, b));
		}

		public void background (int r, int g, int b, int a)
		{
			background (color (r, g, b, a));
		}
		*/

		public void background (double gray)
		{
			background (color (gray));
		}

		public void background (double gray, double alpha)
		{
			background (color (gray, alpha));
		}

		public void background (double r, double g, double b)
		{
			background (color (r, g, b));
		}

		public void background (double r, double g, double b, double a)
		{
			background (color (r, g, b, a));
		}

		public void background (string hex)
		{
			background (color (hex));
		}

		public void background (string hex, double alpha)
		{
			background (color (hex, alpha));
		}

		public void background (Color c)
		{
			HostPaint.BgColor = c.ToArgb ();
		}

		public void colorMode (ColorMode mode)
		{
			throw new NotImplementedException ();
		}

		public void colorMode (ColorMode mode, double range)
		{
			throw new NotImplementedException ();
		}

		public void colorMode (ColorMode mode, double range1, double range2, double range3)
		{
			throw new NotImplementedException ();
		}

		public void colorMode (ColorMode mode, double range1, double range2, double range3, double range4)
		{
			throw new NotImplementedException ();
		}

		public void stroke (double gray)
		{
			stroke_color = color (gray);
		}

		public void stroke (double gray, double alpha)
		{
			stroke_color = color (gray, alpha);
		}

		public void stroke (double r, double g, double b)
		{
			stroke_color = color (r, g, b);
		}

		public void stroke (double r, double g, double b, double a)
		{
			stroke_color = color (r, g, b, a);
		}

		public void stroke (string hex)
		{
			stroke_color = color (hex);
		}

		public void stroke (string hex, double alpha)
		{
			stroke_color = color (hex, alpha);
		}

		public void stroke (Color color)
		{
			stroke_color = color;
		}

		public void stroke (Color color, double alpha)
		{
			stroke_color = new Color (color.R, color.G, color.B, (byte) alpha);
		}

		public void noFill ()
		{
			fill_color = Color.Transparent;
		}

		public void noStroke ()
		{
			stroke_color = Color.Transparent;
		}

		/*
		public void fill (int gray)
		{
			fill_color = color (gray);
		}

		public void fill (int gray, int alpha)
		{
			fill_color = color (gray, alpha);
		}

		public void fill (int r, int g, int b)
		{
			fill_color = color (r, g, b);
		}

		public void fill (int r, int g, int b, int a)
		{
			fill_color = color (r, g, b, a);
		}
		*/

		public void fill (double gray)
		{
			fill (color (gray));
		}

		public void fill (double gray, double alpha)
		{
			fill (color (gray, alpha));
		}

		public void fill (double r, double g, double b)
		{
			fill (color (r, g, b));
		}

		public void fill (double r, double g, double b, double a)
		{
			fill (color (r, g, b, a));
		}

		public void fill (string hex)
		{
			fill (color (hex));
		}

		public void fill (string hex, double alpha)
		{
			fill (color (hex, alpha));
		}

		public void fill (Color color)
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
		public double alpha (Color value)
		{
			return value.A;
		}

		public double red (Color value)
		{
			return value.R;
		}

		public double green (Color value)
		{
			return value.G;
		}

		public double blue (Color value)
		{
			return value.B;
		}

		public Color color (double gray)
		{
			return color (gray, 0xFF);
		}

		public Color color (double gray, double alpha)
		{
			return color (gray, gray, gray, alpha);
		}

		public Color color (double r, double g, double b)
		{
			return color (r, g, b, 0xFF);
		}

		public Color color (double r, double g, double b, double a)
		{
			return new Color ((byte) (int) r, (byte) (int) g, (byte) (int) b, (byte) (int) a);
		}

		public Color color (string hex)
		{
			uint v = uint.Parse (hex.Substring (1), NumberStyles.HexNumber);
			if (hex.Length == 7)
				v |= 0xFF000000;
			return new Color ((byte) ((v & 0xFF0000) >> 16), (byte) ((v & 0xFF00) >> 8), (byte) (v & 0xFF), (byte) (v >> 24));
		}

		public Color color (string hex, double alpha)
		{
			Color c = color (hex);
			c.A = (byte) alpha;
			return c;
		}

		public Color lerpColor (Color c1, Color c2, double amt)
		{
			return new Color (
				(byte) lerp (c1.R, c2.R, amt),
				(byte) lerp (c1.G, c2.G, amt),
				(byte) lerp (c1.B, c2.B, amt),
				(byte) lerp (c1.A, c2.A, amt));
		}
	}
}
