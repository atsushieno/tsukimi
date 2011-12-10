using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PString = System.String;

namespace ProcessingCli
{

	public class PImage
	{
		Image img;
		BitmapImage source;
		List<PImage> masks;

		internal PImage (BitmapImage bitmap)
		{
			img = new Image ();
			img.ImageFailed += delegate(object sender, ExceptionRoutedEventArgs e) {
				Console.WriteLine ("############ ImageFailed: {0} {1}", e.OriginalSource, e.ErrorException);
			};
			source = bitmap;
		}

		internal PImage (Color [] pixels)
		{
		}

		internal Image Image {
			get { return img; }
		}

		internal void SetSource ()
		{
			img.Source = source;
		}
		
		internal List<PImage> Masks {
			get {
				if (masks == null)
					masks = new List<PImage> ();
				return masks;
			}
		}

		public double width {
			get { return img.Width; }
			set { img.Width = value; }
		}

		public double height {
			get { return img.Height; }
			set { img.Height = value; }
		}

		public Color [] pixels {
			get { throw new NotSupportedException (); }
		}

		public Color @get (int x, int y)
		{
			throw new NotSupportedException ();
		}

		public Color @set (int x, int y, Color c)
		{
			throw new NotSupportedException ();
		}

		public void copy (int sx, int sy, int swidth, int sheight, int dx, int dy, int dwidth, int dheight)
		{
			copy (this, sx, sy, swidth, sheight, dx, dy, dwidth, dheight);
		}

		public void copy (PImage srcImg, int sx, int sy, int swidth, int sheight, int dx, int dy, int dwidth, int dheight)
		{
			throw new NotSupportedException ();
		}

		public void mask (PImage maskImg)
		{
			Masks.Add (maskImg);
		}

		public void mask (int [] maskArray)
		{
			var maskAlpha = new Color [maskArray.Length];
			for (int i = 0; i < maskAlpha.Length; i++)
				maskAlpha [i] = Color.FromArgb ((byte) maskArray [i], 0, 0, 0);
			mask (new PImage (maskAlpha));
		}
	}
}

