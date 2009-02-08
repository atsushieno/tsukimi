using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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
			return createFont (name, size, smooth, charset);
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

		public static void text (string data, double x, double y)
		{
			Host.Children.Add (CreateTextBlock (data, x, y));
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
	}
}
