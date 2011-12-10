using System;
using System.Collections.Generic;
using Android.Graphics;
using PString = System.String;

namespace ProcessingCli
{

	public class PImage
	{
		Bitmap img;
		List<PImage> masks;

		internal PImage (Bitmap bitmap)
		{
			img = bitmap;
		}

		internal PImage (Color [] pixels)
		{
		}

		internal Bitmap Image {
			get { return img; }
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
			set { throw new NotImplementedException (); }
		}

		public double height {
			get { return img.Height; }
			set { throw new NotImplementedException (); }
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
				maskAlpha [i] = new Color ((byte) 0, (byte) 0, (byte) 0, (byte) maskArray [i]);
			mask (new PImage (maskAlpha));
		}
	}
}

