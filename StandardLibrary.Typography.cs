using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using PString = System.String;

namespace ProcessingCli
{
	public class PFont
	{
		static readonly PString [] default_fonts = {
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

		public static PString [] list ()
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
		
		public string Name { get; set; }
		public double Size { get; set; }
		public bool Smooth { get; set; }
		public char [] Charset { get; set; }
	}

	public static partial class StandardLibrary
	{
/*
*** Typography

	type:
		PFont

	function:
		*createFont()
		loadFont()
		text()
		textFont()

		textMode()
		textSize()
		textAlign()
		textLeading()
		textWidth()

		textDescent()
		textAscent()
*/
		static PFont text_font;

		public static PFont createFont (string name, double size)
		{
			return createFont (name, size, false, null);
		}

		public static PFont createFont (string name, double size, bool smooth)
		{
			return createFont (name, size, smooth, null);
		}

		public static PFont createFont (string name, double size, bool smooth, char [] charset)
		{
			return new PFont (name, size, smooth, charset);
		}

		static void ApplyTextFont (TextBlock tb)
		{
			// FIXME: support textFont.
		}

		static TextBlock CreateTextBlock (string data, double x, double y)
		{
			TextBlock tb = new TextBlock ();
			tb.Inlines.Add (data);
			Canvas.SetLeft (tb, x);
			Canvas.SetTop (tb, y);
			ApplyTextFont (tb);
			return tb;
		}

		public static void text (object data, double x, double y)
		{
			Host.Children.Add (CreateTextBlock (data.ToString (), x, y));
		}

		public static void text (string data, double x, double y, double width, double height)
		{
			TextBlock tb = CreateTextBlock (data, x, y);
			tb.Width = width;
			tb.Height = height;
			Host.Children.Add (tb);
		}

		public static void textFont (PFont font)
		{
			text_font = font;
		}
		
		public static void textFont (PFont font, double size)
		{
			text_font = new PFont (font.Name, size, font.Smooth, font.Charset);
		}

		// FIXME: they are not supported.
		static TextAlign text_align, text_y_align;
		static double text_leading;
		static double text_size;

		public static void textAlign (TextAlign align)
		{
			text_align = align;
		}

		public static void textAlign (TextAlign align, TextAlign yAlign)
		{
			text_align = align;
			text_y_align = yAlign;
		}

		public static void textLeading (double value)
		{
			text_leading = value;
		}

		public static void textSize (double value)
		{
			text_size = value;
		}

		public static double textWidth (char c)
		{
			return textWidth (c.ToString ());
		}

		public static double textWidth (string s)
		{
			TextBlock tb = CreateTextBlock (s, 0, 0);
			tb.Measure (new Size (Host.Width, Host.Height));
			return tb.DesiredSize.Width;
		}

		public static double textAscent ()
		{
			// FIXME: implement correctly.
			return text_font.Size;
		}

		public static double textDescent ()
		{
			// FIXME: implement correctly.
			return 0;
		}
	}
}
