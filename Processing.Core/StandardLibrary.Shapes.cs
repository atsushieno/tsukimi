using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
		public void arc (int x, int y, int width, int height, int start, int stop)
		{
			arc ((double) x, (double) y, (double) width, (double) height, (double) start, (double) stop);
		}

		public void arc (double x, double y, double width, double height, double start, double stop)
		{
			Path p = new Path ();
			PathGeometry pg = new PathGeometry ();
			// FIXME: moonlight should be fixed to automatically ceate Figures.
			pg.Figures = new PathFigureCollection ();

			PathFigure pf = new PathFigure ();
			pf.StartPoint = new Point (x, y);
			ArcSegment ars = new ArcSegment ();
			ars.Size = new Size (width, height);
			ars.SweepDirection = SweepDirection.Counterclockwise;
			ars.IsLargeArc = (stop - start > PI);

			pf.Segments.Add (ars);
			pg.Figures.Add (pf);

			p.Stroke = stroke_brush;
			p.StrokeThickness = stroke_weight;
			p.StrokeLineJoin = stroke_join;
			p.StrokeStartLineCap = p.StrokeEndLineCap = stroke_cap;
			p.Data = pg;
			Host.Children.Add (p);
		}

		public void ellipse (double x, double y, double width, double height)
		{
			Ellipse r = new Ellipse ();
			r.Width = width;
			r.Height = height;
			r.Stroke = stroke_brush;
			r.StrokeThickness = stroke_weight;
			r.StrokeLineJoin = stroke_join;
			r.StrokeStartLineCap = r.StrokeEndLineCap = stroke_cap;
			r.Fill = fill_brush;
			Canvas.SetLeft (r, x);
			Canvas.SetTop (r, y);
			Host.Children.Add (r);
		}

		public void line (int x1, int y1, int x2, int y2)
		{
			line ((double) x1, (double) y1, (double) x2, (double) y2);
		}

		public void line (double x1, double y1, double x2, double y2)
		{
			var l = new Line ();
			l.X1 = x1;
			l.Y1 = y1;
			l.X2 = x2;
			l.Y2 = y2;
			l.Stroke = stroke_brush;
			l.Fill = fill_brush;
			l.StrokeThickness = stroke_weight;
			l.StrokeLineJoin = stroke_join;
			l.StrokeStartLineCap = l.StrokeEndLineCap = stroke_cap;
			Host.Children.Add (l);
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
			set (x, y, Colors.Black);
		}

		public void point (double x, double y, double z)
		{
			throw new NotImplementedException ();
		}

		public void rect (double x, double y, double width, double height)
		{
			Rectangle r = new Rectangle ();
			switch (rect_mode) {
			case Constants.Corner:
				Canvas.SetLeft (r, x);
				Canvas.SetTop (r, y);
				r.Width = width + 1;
				r.Height = height + 1;
				break;
			case Constants.Corners:
				Canvas.SetLeft (r, x);
				Canvas.SetTop (r, y);
				r.Width = width - x + 1;
				r.Height = height - y + 1;
				break;
			case Constants.Center:
				Canvas.SetLeft (r, x - (width / 2));
				Canvas.SetTop (r, y - (height / 2));
				r.Width = width + 1;
				r.Height = height + 1;
				break;
			case Constants.Radius:
				Canvas.SetLeft (r, x - width);
				Canvas.SetTop (r, y - height);
				r.Width = width * 2 + 1;
				r.Height = height * 2 + 1;
				break;
			}
				
			r.Stroke = stroke_brush;
			r.StrokeThickness = stroke_weight;
			r.StrokeLineJoin = stroke_join;
			r.StrokeStartLineCap = r.StrokeEndLineCap = stroke_cap;
			r.Fill = fill_brush;
			Host.Children.Add (r);
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
			var p = new Polygon ();
			p.Points.Add (new Point (x1, y1));
			p.Points.Add (new Point (x2, y2));
			p.Points.Add (new Point (x3, y3));
			p.Stroke = stroke_brush;
			p.Fill = fill_brush;
			p.StrokeThickness = stroke_weight;
			p.StrokeLineJoin = stroke_join;
			p.StrokeStartLineCap = p.StrokeEndLineCap = stroke_cap;
			Host.Children.Add (p);
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

		public void ellipseMode (Constants mode)
		{
			Console.WriteLine ("WARNING: no support for ellipseMode() yet");
		}
		
		public void strokeJoin (Constants value)
		{
			switch (value) {
			case Constants.Round:
				stroke_join = PenLineJoin.Round;
				break;
			case Constants.Miter:
				stroke_join = PenLineJoin.Miter;
				break;
			case Constants.Bevel:
				stroke_join = PenLineJoin.Bevel;
				break;
			default:
				throw new ArgumentException ("Invalid strokeJoin enumeration value: " + value);
			}
		}
		
		public void strokeCap (Constants value)
		{
			switch (value) {
			case Constants.Round:
				stroke_cap = PenLineCap.Round;
				break;
			case Constants.Square:
				stroke_cap = PenLineCap.Square;
				break;
			case Constants.Project:
				stroke_cap = PenLineCap.Flat;
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

