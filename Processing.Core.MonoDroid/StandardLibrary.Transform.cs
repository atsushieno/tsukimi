using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using Android.Graphics;
using PString = System.String;

namespace ProcessingCli
{

	public partial class ProcessingApplication
	{
		public void applyMatrix (double n0, double n1, double n2, double n3,
		                         double n4, double n5, double n6, double n7,
		                         double n8, double n9, double n10, double n11,
		                         double n12, double n13, double n14, double n15)
		{
			throw new NotImplementedException ();
		}

		public void printMatrix ()
		{
			throw new NotImplementedException ();
		}
		
		Stack<Matrix> matrices = new Stack<Matrix> ();
		public void pushMatrix ()
		{
			var m = new Matrix (Host.Matrix);
			matrices.Push (host.Matrix);
			Host.Matrix = m;
		}
		public void popMatrix ()
		{
			if (matrices.Count == 0)
				throw new InvalidOperationException ("No stacked matrix anymore.");
			host.Matrix = matrices.Pop ();
		}
		public void translate (double x, double y)
		{
			Host.Translate ((float) x, (float) y);
		}
		public void translate (int x, int y)
		{
			translate ((double) x, (double) y);
		}
		public void translate (double x, double y, double z)
		{
			throw new NotImplementedException ();
		}
		public void translate (int x, int y, int z)
		{
			throw new NotImplementedException ();
		}
		public void rotate (double angle)
		{
			throw new NotImplementedException ();
		}
		public void rotateX (double angle)
		{
			throw new NotImplementedException ();
		}
		public void rotateY (double angle)
		{
			throw new NotImplementedException ();
		}
		public void rotateZ (double angle)
		{
			throw new NotImplementedException ();
		}
		public void scale (double size)
		{
			scale (size, size);
		}
		public void scale (double x, double y)
		{
			throw new NotImplementedException ();
		}
		public void scale (double x, double y, double z)
		{
			throw new NotImplementedException ();
		}
	}
}
