using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

/*

Object mappings:
	color -> System.Windows.Media.Color
	char -> System.Char
	float -> System.Double
	int -> System.Int32
	boolean -> System.Boolean
	byte -> System.SByte
	String -> ProcessingDlr.String
	Array -> (no direct mapping. System.Array is implicitly used)
	Object -> System.Object

	When converting pde to XAML, it should filter every
	method invocation to reject invalid calls. For example,
	call to GetHashCode() in processing object must be rejected
	since it does not exist in processing.
*/

using String = ProcessingDlr.PString;
using ColorMode = System.Int32;

namespace ProcessingDlr
{
	public class PString
	{
		string s;

		public PString (string s)
		{
		}

		public static implicit operator String (string s)
		{
			return new String (s);
		}

		public static implicit operator string (String s)
		{
			return s.s;
		}

		public int charAt (int index)
		{
			return s [index];
		}

		public bool equals (String s)
		{
			return this.s == s.s;
		}

		public override bool Equals (object o)
		{
			String s = o as String;
			return s != null && s.s == this.s;
		}

		public override int GetHashCode ()
		{
			return s.GetHashCode ();
		}

		public int indexOf (String s)
		{
			return indexOf (s, 0);
		}

		public int indexOf (String s, int startIndex)
		{
			return this.s.IndexOf (s.s, startIndex, StringComparison.Ordinal);
		}

		public int length ()
		{
			return s.Length;
		}

		public string substring (int start)
		{
			return s.Substring (start);
		}

		public string substring (int start, int end)
		{
			return s.Substring (start, end - start);
		}

		public string ToLowerCase ()
		{
			return s.ToLower (CultureInfo.InvariantCulture);
		}

		public string ToUpperCase ()
		{
			return s.ToUpper (CultureInfo.InvariantCulture);
		}
	}

	public enum SizeMode
	{
		Processing2D,
		Processing3D,
		Java2D,
		OpenGL
	}

	public class ProcessingHostControl : UserControl
	{
		public const string ID = "c056f9b5-5fc9-46ef-a42c-ae316631ebd9";
	}

	public class ProcessingStandardFieldAttribute : Attribute
	{
	}

	public static partial class StandardLibrary
	{
		[ProcessingStandardField]
		public const double PI = System.Math.PI;
		[ProcessingStandardField]
		public const double HALF_PI = PI / 2.0;
		[ProcessingStandardField]
		public const double TWO_PI = PI * 2.0;

		#region UNDOCUMENTED
		[ProcessingStandardField]
		public const int RGB = 1;

		[ProcessingStandardField]
		public const int HSB = 2;
		#endregion

		public static readonly ProcessingHostControl Host =
			null;//	new ProcessingHostControl ();

		static SolidColorBrush stroke_brush;

		static Color? stroke_color {
			get { return stroke_brush != null ? (Color?) stroke_brush.Color : null; }
			set { stroke_brush = value != null ? new SolidColorBrush ((Color) value) : null; }
		}

		static Color? fill_color;

		[ProcessingStandardField]
		public static int width {
			get { return (int) Host.Width; }
		}

		[ProcessingStandardField]
		public static int height {
			get { return (int) Host.Height; }
		}

		static string [] all_field_names;

		public static string [] AllFieldNames { // FIXME: should not be public
			get {
				if (all_field_names != null)
					return all_field_names;
				var names = new List<string> ();
				foreach (var mi in typeof (StandardLibrary).GetMembers ())
					if (mi.GetCustomAttributes (typeof (ProcessingStandardFieldAttribute), false).Length > 0)
						names.Add (mi.Name);
				all_field_names = names.ToArray ();
				return all_field_names;
			}
		}

		// The functions below are not defined in the standard library.
		// They are resolved only internally.
		//
		//	- setup() : Application.Start
		//	- draw() : after setup()
		//
		// (It is possible that noLoop() and loop() could become
		// like them.)

		public static void exit ()
		{
			throw new NotImplementedException ();
		}

		public static void size (int width, int height)
		{
			size (width, height, SizeMode.Java2D);
		}

		public static void size (int width, int height, SizeMode sizeMode)
		{
			// FIXME: check sizeMode and reject some.

			Host.Width = width;
			Host.Height = height;
		}

		public static void noLoop ()
		{
			throw new NotImplementedException ();
		}

		public static void delay (int milliseconds)
		{
			Thread.Sleep (milliseconds);
		}

		public static void loop ()
		{
			throw new NotImplementedException ();
		}

		public static void redraw ()
		{
			throw new NotImplementedException ();
		}

/*
		// Array Functions

		shorten()
		concat()
		subset()
		append()
		sort()
		arraycopy()
		reverse()
		splice()
		expand()

*** Environment

	static variables
		online
		focused
		frameRate
		screen
		width
		height
		frameCount

	function:
		frameRate()
		noCursor()
		cursor()
*/

		public static void triangle (int x1, int y1, int x2, int y2, int x3, int y3)
		{
			triangle ((double) x1, (double) y1, (double) x2, (double) y2, (double) x3, (double) y3);
		}

		public static void triangle (double x1, double y1, double x2, double y2, double x3, double y3)
		{
			var p = new Polygon ();
			p.Points.Add (new Point (x1, y1));
			p.Points.Add (new Point (x2, y2));
			p.Points.Add (new Point (x3, y3));
			// FIXME: consider fill property
			p.Stroke = stroke_brush;
			// FIXME: append to host
		}

		public static void line (int x1, int y1, int x2, int y2)
		{
			line ((double) x1, (double) y1, (double) x2, (double) y2);
		}

		public static void line (double x1, double y1, double x2, double y2)
		{
			var l = new Line ();
			l.X1 = x1;
			l.Y1 = y1;
			l.X2 = x2;
			l.Y2 = y2;
			// FIXME: consider fill property
			l.Stroke = stroke_brush;
			// FIXME: append to host
		}

		public static void line (int x1, int y1, int z1, int x2, int y2, int z2)
		{
			line ((double) x1, (double) y1, (double) z1, (double) x2, (double) y2, (double) z2);
		}

		public static void line (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			throw new NotImplementedException ();
		}

/*
*** Shape

	function:
		triangle()
		line()
		arc()
		point()
		quad()
		ellipse()
		rect()

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
		strokeWeight()
		smooth()
		strokeJoin()
		noSmooth()
		ellipseMode()
		rectMode()
		strokeCap()

		vertex()
		bezierVertex()
		textureMode()
		beginShape()
		texture()
		curveVertex()
		endShape()

*** Input

	static variable:
		keyCode
		key
		keyPressed
		mouseButton
		mouseX
		pmouseX
		mouseY
		mousePressed
		pmouseY

	function:
		mouseDragged()
		mouseMoved()
		mouseReleased()
		mousePressed()
		mouseClicked()

		keyTyped()
		keyReleased()
		keyPressed()

		openStream()
		open()
		loadStrings()
		loadBytes()

		status()
		link()
		param()

		hour()
		millis()
		year()
		minute()
		month()
		day()
		second()

*** Output

	type:
		PrintWriter

	function:
		println()
		print()

		saveFrame()
		save()

		endRecord()
		saveStrings()
		saveBytes()
		createWriter()
		createReader()
		beginRecord()

*** Transform

	function:
		rotateY()
		printMatrix()
		pushMatrix()
		rotateZ()
		applyMatrix()
		scale()
		popMatrix()
		translate()
		resetMatrix()
		rotate()
		rotateX()

*** Lights, Camera

	function:
		noLights()
		directionalLight()
		lightFalloff()
		pointLight()
		lights()
		lightSpecular()
		ambientLight()
		normal()
		spotLight()

		camera()
		ortho()
		endCamera()
		printCamera()
		perspective()
		frustum()
		printProjection()
		beginCamera()

		modelZ()
		screenZ()
		modelX()
		screenX()
		modelY()
		screenY()

		shininess()
		specular()
		ambient()
		emissive()
*/

		public static void background (double gray)
		{
			background (color (gray));
		}

		public static void background (double gray, double alpha)
		{
			background (color (gray, alpha));
		}

		public static void background (double r, double g, double b)
		{
			background (color (r, g, b));
		}

		public static void background (double r, double g, double b, double a)
		{
			background (color (r, g, b, a));
		}

		public static void background (string hex)
		{
			background (color (hex));
		}

		public static void background (string hex, double alpha)
		{
			background (color (hex, alpha));
		}

		static void background (Color c)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range1, double range2, double range3)
		{
			throw new NotImplementedException ();
		}

		public static void colorMode (ColorMode mode, double range1, double range2, double range3, double range4)
		{
			throw new NotImplementedException ();
		}

		public static void stroke (double gray)
		{
			stroke_color = color (gray);
		}

		public static void stroke (double gray, double alpha)
		{
			stroke_color = color (gray, alpha);
		}

		public static void stroke (double r, double g, double b)
		{
			stroke_color = color (r, g, b);
		}

		public static void stroke (double r, double g, double b, double a)
		{
			stroke_color = color (r, g, b, a);
		}

		public static void stroke (string hex)
		{
			stroke_color = color (hex);
		}

		public static void stroke (string hex, double alpha)
		{
			stroke_color = color (hex, alpha);
		}

		public static void noFill ()
		{
			fill_color = null;
		}

		public static void noStroke ()
		{
			stroke_color = null;
		}

		public static void fill (double gray)
		{
			fill_color = color (gray);
		}

		public static void fill (double gray, double alpha)
		{
			fill_color = color (gray, alpha);
		}

		public static void fill (double r, double g, double b)
		{
			fill_color = color (r, g, b);
		}

		public static void fill (double r, double g, double b, double a)
		{
			fill_color = color (r, g, b, a);
		}

		public static void fill (string hex)
		{
			fill_color = color (hex);
		}

		public static void fill (string hex, double alpha)
		{
			fill_color = color (hex, alpha);
		}

/*
*** Color

	function:

		blendColor()
		red()
		brightness()
		blue()
		saturation()
		lerpColor()
		green()
		hue()
		alpha()
*/

		public static Color color (double gray)
		{
			return color (gray, 0xFF);
		}

		public static Color color (double gray, double alpha)
		{
			return color (gray, gray, gray, alpha);
		}

		public static Color color (double r, double g, double b)
		{
			return color (r, g, b, 0xFF);
		}

		public static Color color (double r, double g, double b, double a)
		{
			return Color.FromArgb ((byte) a, (byte) r, (byte) g, (byte) b);
		}

		public static Color color (string hex)
		{
			uint v = uint.Parse (hex, NumberStyles.HexNumber);
			return Color.FromArgb ((byte) (v & 0xFF0000 >> 16), (byte) (v & 0xFF00 >> 8), (byte) (v & 0xFF), (byte) (v >> 24));
		}

		public static Color color (string hex, double alpha)
		{
			Color c = color (hex);
			c.A = (byte) alpha;
			return c;
		}

/*
*** Image

	type:
		PImage

	static variable:
		pixels (of Array)

	function:
		createImage()

		loadImage()
		image()
		noTint()
		imageMode()
		tint()

		filter()
		copy()
		set()
		updatePixels()
		blend()
		loadPixels()
		get()

*** Rendering

	type:
		PGraphics

	function:
		hint()
		unhint()
		createGraphics()

*** Typography

	type:
		PFont

	function:
		text()
		createFont()
		loadFont()
		textFont()

		textMode()
		textSize()
		textAlign()
		textLeading()
		textWidth()

		textDescent()
		textAscent()
*/
	}
}
