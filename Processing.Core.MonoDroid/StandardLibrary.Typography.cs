using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using Android.Graphics;
using Android.Text;
using PString = System.String;

namespace ProcessingCli
{
	public class PFont
	{
		readonly PString [] default_fonts = {
			"Arial",
			"Arial Black",
			"Comic Sans MS",
			"Courier New",
			"Lucida Grande",
			"Lucida Sans Unicode",
			"Times New Roman",
			"Trebuchet MS",
			"Verdana",
		};

		public PString [] list ()
		{
			return default_fonts;
		}

		public PFont (string name, double size, bool smooth, char [] charset)
		{
			Name = name;
			Size = size;
			Smooth = smooth;
			Charset = charset;
		}
		
		public Typeface Family;
		
		public string Name {
			get { return Family.ToString (); }
			set { Family = Typeface.Create (value, TypefaceStyle.Normal); }
		}
		public double Size { get; set; }
		public bool Smooth { get; set; }
		public char [] Charset { get; set; }
	}

	public partial class ProcessingApplication
	{
/*
*** Typography

	type:
		*PFont

	function:
		* createFont()
		- loadFont()
		* text()
		* textFont()

		textMode()
		* textSize()
		* textAlign()
		textLeading()
		* textWidth()

		textDescent()
		textAscent()
*/
		PFont text_font;

		public PFont createFont (string name, double size)
		{
			return createFont (name, size, false, null);
		}

		public PFont createFont (string name, double size, bool smooth)
		{
			return createFont (name, size, smooth, null);
		}

		public PFont createFont (string name, double size, bool smooth, char [] charset)
		{
			return new PFont (name, size, smooth, charset);
		}

		public PFont loadFont (string name)
		{
			throw new NotSupportedException ("Use createFont instead");
		}

		void ApplyTextFont ()
		{
			HostPaint.TextSize = (float) text_font.Size;
			HostPaint.SetTypeface (text_font.Family);
		}

		public void text (object data, double x, double y)
		{
			ApplyTextFont ();
			Host.DrawText (data.ToString (), (float) x, (float) y, HostPaint);
		}

		public void text (string data, double x, double y, double width, double height)
		{
			ApplyTextFont ();
			// FIXME: height is not considered.
			HostPaint.TextScaleX = (float) (width / HostPaint.MeasureText (data));
			Host.DrawText (data, (float) x, (float) y, HostPaint);
		}

		public void textFont (PFont font)
		{
			text_font = font;
		}
		
		public void textFont (PFont font, double size)
		{
			text_font = new PFont (font.Name, size, font.Smooth, font.Charset);
		}

		// FIXME: they are not supported.
		double text_leading;

		public void textAlign (Constants align)
		{
			switch (align) {
			case Constants.Left: HostPaint.TextAlign = Paint.Align.Left; break;
			case Constants.Center: HostPaint.TextAlign = Paint.Align.Center; break;
			case Constants.Right: HostPaint.TextAlign = Paint.Align.Right; break;
			default: throw new ArgumentException ();
			}
		}

		public void textAlign (Constants align, Constants yAlign)
		{
			// FIXME: yAlign is not considered
			textAlign (align);
		}

		public void textLeading (double value)
		{
			// FIXME: text_leading is unused
			text_leading = value;
		}

		public void textSize (double value)
		{
			text_font = new PFont (text_font.Name, value, text_font.Smooth, text_font.Charset);
		}

		public double textWidth (char c)
		{
			return textWidth (c.ToString ());
		}

		public double textWidth (string s)
		{
			return HostPaint.MeasureText (s);
		}

		public double textAscent ()
		{
			// FIXME: implement correctly.
			return text_font.Size;
		}

		public double textDescent ()
		{
			// FIXME: implement correctly.
			return 0;
		}
	}
}
