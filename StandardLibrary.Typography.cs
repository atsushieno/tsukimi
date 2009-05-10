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
		
		public string Name { get; set; }
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

		void ApplyTextFont (TextBlock tb)
		{
			// FIXME: support textFont.
		}

		TextBlock CreateTextBlock (string data, double x, double y)
		{
			TextBlock tb = new TextBlock ();
			tb.Inlines.Add (data);
			Canvas.SetLeft (tb, x);
			Canvas.SetTop (tb, y);
			ApplyTextFont (tb);
			tb.Foreground = fill_brush;
			return tb;
		}

		public void text (object data, double x, double y)
		{
			Host.Children.Add (CreateTextBlock (data.ToString (), x, y));
		}

		public void text (string data, double x, double y, double width, double height)
		{
			TextBlock tb = CreateTextBlock (data, x, y);
			tb.Width = width;
			tb.Height = height;
			Host.Children.Add (tb);
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
		Constants text_align, text_y_align;
		double text_leading;

		public void textAlign (Constants align)
		{
			text_align = align;
		}

		public void textAlign (Constants align, Constants yAlign)
		{
			text_align = align;
			text_y_align = yAlign;
		}

		public void textLeading (double value)
		{
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
			TextBlock tb = CreateTextBlock (s, 0, 0);
			tb.Measure (new Size (Host.Width, Host.Height));
			return tb.DesiredSize.Width;
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
