using System;
using Android.Graphics;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		public void arc (int x, int y, int width, int height, int start, int stop)
		{
			arc ((double) x, (double) y, (double) width, (double) height, (double) start, (double) stop);
		}
		
		static RectF ToRectF (double x, double y, double width, double height)
		{
			return new RectF ((float) x, (float) y, (float) (x + width), (float) (y + height));
		}

		public void arc (double x, double y, double width, double height, double start, double stop)
		{
			Host.DrawArc (ToRectF (x, y, width, height), (float) start, (float) stop, false, HostPaint);
		}

		public void ellipse (double x, double y, double width, double height)
		{
			switch (ellipse_mode) {
			case Constants.Corner:
				Host.DrawArc (ToRectF (x, y, width + 1, height + 1), 0, 2.0F, false, HostPaint);
				break;
			case Constants.Corners:
				Host.DrawArc (ToRectF (x, y, width - x + 1, height - y + 1), 0, 2.0F, false, HostPaint);
				break;
			case Constants.Center:
				Host.DrawArc (ToRectF (x - (width / 2), y - (height / 2), width + 1, height + 1), 0, 2.0F, false, HostPaint);
				break;
			case Constants.Radius:
				Host.DrawArc (ToRectF (x - width, y - height, width * 2 + 1, height * 2 + 1), 0, 2.0F, false, HostPaint);
				break;
			}
		}

		Constants ellipse_mode = Constants.Center;

		public void ellipseMode (Constants mode)
		{
			ellipse_mode = mode;
		}

		public void line (int x1, int y1, int x2, int y2)
		{
			line ((double) x1, (double) y1, (double) x2, (double) y2);
		}

		public void line (double x1, double y1, double x2, double y2)
		{
			Host.DrawLine ((float) x1, (float) y1, (float) x2, (float) y2, HostPaint);
		}

		public void line (int x1, int y1, int z1, int x2, int y2, int z2)
		{
			line ((double) x1, (double) y1, (double) z1, (double) x2, (double) y2, (double) z2);
		}

		public void line (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			throw new NotImplementedException ();
		}

		public void point (double x, double y)
		{
			set (x, y, new Color (HostPaint.Color));
		}

		public void point (double x, double y, double z)
		{
			throw new NotImplementedException ();
		}

		public void rect (double x, double y, double width, double height)
		{
			switch (rect_mode) {
			case Constants.Corner:
				Host.DrawRect (ToRectF (x, y, width + 1, height + 1), HostPaint);
				break;
			case Constants.Corners:
				Host.DrawRect (ToRectF (x, y, width - x + 1, height - y + 1), HostPaint);
				break;
			case Constants.Center:
				Host.DrawRect (ToRectF (x - (width / 2), y - (height / 2), width + 1, height + 1), HostPaint);
				break;
			case Constants.Radius:
				Host.DrawRect (ToRectF (x - width, y - height, width * 2 + 1, height * 2 + 1), HostPaint);
				break;
			}
		}

		Constants rect_mode = Constants.Corner;

		public void rectMode (Constants mode)
		{
			this.rect_mode = mode;
		}

		public void triangle (int x1, int y1, int x2, int y2, int x3, int y3)
		{
			triangle ((double) x1, (double) y1, (double) x2, (double) y2, (double) x3, (double) y3);
		}

		public void triangle (double x1, double y1, double x2, double y2, double x3, double y3)
		{
			var path = new Path ();
			path.MoveTo ((float) x3, (float) y3);
			path.LineTo ((float) x1, (float) y1);
			path.LineTo ((float) x2, (float) y2);
			path.LineTo ((float) x3, (float) y3);
			path.Close ();
			Host.DrawPath (path, HostPaint);
		}

		public void strokeWeight (double size)
		{
			stroke_weight = size;
		}

		public void smooth ()
		{
			Console.WriteLine ("WARNING: no support for smooth() yet");
		}

		public void noSmooth ()
		{
			Console.WriteLine ("WARNING: no support for noSmooth() yet");
		}
		
		public void strokeJoin (Constants value)
		{
			switch (value) {
			case Constants.Round:
				stroke_join = Paint.Join.Round;
				break;
			case Constants.Miter:
				stroke_join = Paint.Join.Miter;
				break;
			case Constants.Bevel:
				stroke_join = Paint.Join.Bevel;
				break;
			default:
				throw new ArgumentException ("Invalid strokeJoin enumeration value: " + value);
			}
		}
		
		public void strokeCap (Constants value)
		{
			switch (value) {
			case Constants.Round:
				stroke_cap = Paint.Cap.Round;
				break;
			case Constants.Square:
				stroke_cap = Paint.Cap.Square;
				break;
			case Constants.Project:
				stroke_cap = Paint.Cap.Butt;
				break;
			default:
				throw new ArgumentException ("Invalid strokeCap enumeration argument: " + value);
			}
		}
/*
*** Shape

	function:
		arc()
		ellipse()
		*line()
		*point()
		quad()
		*rect()
		*triangle()

		bezierDetail()
		bezierTangent()
		curveTightness()
		bezierPoint()
		curveDetail()
		curvePoint()
		curve()
		bezier()

		box()
		sphere()
		sphereDetail()
		Attributes
		*strokeWeight()
		*smooth()
		*strokeJoin()
		*noSmooth()
		*ellipseMode()
		rectMode()
		*strokeCap()

		vertex()
		bezierVertex()
		textureMode()
		beginShape()
		texture()
		curveVertex()
		endShape()
*/
	}
}

