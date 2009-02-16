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
			if (stroke_weight != null)
				p.StrokeThickness = (double) stroke_weight;
			p.Data = pg;
			Host.Children.Add (p);
		}

		public void ellipse (double x, double y, double width, double height)
		{
			Ellipse r = new Ellipse ();
			r.Width = width;
			r.Height = height;
			r.Stroke = stroke_brush;
			if (stroke_weight != null)
				r.StrokeThickness = (double) stroke_weight;
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
			// FIXME: consider fill property
			l.Stroke = stroke_brush;
			if (stroke_weight != null)
				l.StrokeThickness = (double) stroke_weight;
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
			r.Width = width;
			r.Height = height;
			r.Stroke = stroke_brush;
			if (stroke_weight != null)
				r.StrokeThickness = (double) stroke_weight;
			r.Fill = fill_brush;
			Canvas.SetLeft (r, x);
			Canvas.SetTop (r, y);
			Host.Children.Add (r);
		}

		public void rectMode (Constants mode)
		{
			Console.WriteLine ("WARNING: no support for rectMode() yet");
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
			// FIXME: consider fill property
			p.Stroke = stroke_brush;
			if (stroke_weight != null)
				p.StrokeThickness = (double) stroke_weight;
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
		strokeJoin()
		*noSmooth()
		*ellipseMode()
		rectMode()
		strokeCap()

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

