using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/*

Object mappings:
	color -> System.Windows.Media.Color
	char -> System.Char
	float -> System.Double
	int -> System.Int32
	boolean -> System.Boolean
	byte -> System.SByte
	String -> ProcessingCli.String
	Array -> (no direct mapping. System.Array is implicitly used)
	Object -> System.Object

	When converting pde to XAML, it should filter every
	method invocation to reject invalid calls. For example,
	call to GetHashCode() in processing object must be rejected
	since it does not exist in processing.
*/

using String = ProcessingCli.PString;
using ColorMode = System.Int32;

namespace ProcessingCli
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

	public class PImage
	{
		Image img;
		List<PImage> masks;

		internal PImage (BitmapImage bitmap)
		{
			img = new Image () { Source = bitmap };
		}

		internal PImage (Color [] pixels)
		{
		}

		internal Image Image {
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

	public enum EllipseMode
	{
		Center,
		Radius,
		Corner,
		Corners
	}

	public static partial class StandardLibrary
	{
		[ProcessingStandardField]
		public const double PI = System.Math.PI;
		[ProcessingStandardField]
		public const double HALF_PI = PI / 2.0;
		[ProcessingStandardField]
		public const double TWO_PI = PI * 2.0;
		[ProcessingStandardFieldAttribute]
		public const EllipseMode Center = EllipseMode.Center;
		[ProcessingStandardFieldAttribute]
		public const EllipseMode RADIUS = EllipseMode.Radius;
		[ProcessingStandardFieldAttribute]
		public const EllipseMode CORNER = EllipseMode.Corner;
		[ProcessingStandardFieldAttribute]
		public const EllipseMode CORNERS = EllipseMode.Corners;

		#region UNDOCUMENTED
		[ProcessingStandardField]
		public const int RGB = 1;

		[ProcessingStandardField]
		public const int HSB = 2;
		#endregion

		public static Canvas Host; // set by application

		public static void SetHost (Canvas canvas)
		{
			Host = canvas;
			Host.MouseMove += delegate (object o, MouseEventArgs e) { current_mouse = e; };
		}

		static SolidColorBrush stroke_brush;
		static double? stroke_weight;

		static Color? stroke_color {
			get { return stroke_brush != null ? (Color?) stroke_brush.Color : null; }
			set { stroke_brush = value != null ? new SolidColorBrush ((Color) value) : null; }
		}

		static SolidColorBrush fill_brush;

		static Color? fill_color {
			get { return fill_brush != null ? (Color?) fill_brush.Color : null; }
			set { fill_brush = value != null ? new SolidColorBrush ((Color) value) : null; }
		}

		[ProcessingStandardField]
		public static int width {
			get { return (int) Host.Width; }
		}

		[ProcessingStandardField]
		public static int height {
			get { return (int) Host.Height; }
		}

		static string [] all_field_names, all_func_names;

		public static string [] AllFieldNames {
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

		public static string [] AllFunctionNames {
			get {
				if (all_func_names != null)
					return all_func_names;
				var names = new List<string> ();
				foreach (var mi in typeof (StandardLibrary).GetMethods ())
					if (true) // (mi.GetCustomAttributes (typeof (ProcessingStandardFieldAttribute), false).Length > 0)
						names.Add (mi.Name);
				all_func_names = names.ToArray ();
				return all_func_names;
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
			Console.WriteLine ("WARNING: not implemented function noLoop()");
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
		static MouseEventArgs current_mouse;

		[ProcessingStandardField]
		public static double mouseX {
			get { return current_mouse == null ? 0 : current_mouse.GetPosition (null).X; }
		}

		[ProcessingStandardField]
		public static double mouseY {
			get { return current_mouse == null ? 0 : current_mouse.GetPosition (null).Y; }
		}

/*
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

		/*
		public static void background (int gray)
		{
			background (color (gray));
		}

		public static void background (int gray, int alpha)
		{
			background (color (gray, alpha));
		}

		public static void background (int r, int g, int b)
		{
			background (color (r, g, b));
		}

		public static void background (int r, int g, int b, int a)
		{
			background (color (r, g, b, a));
		}
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
			Host.Background = new SolidColorBrush (c);
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

		/*
		public static void fill (int gray)
		{
			fill_color = color (gray);
		}

		public static void fill (int gray, int alpha)
		{
			fill_color = color (gray, alpha);
		}

		public static void fill (int r, int g, int b)
		{
			fill_color = color (r, g, b);
		}

		public static void fill (int r, int g, int b, int a)
		{
			fill_color = color (r, g, b, a);
		}
		*/

		public static void fill (double gray)
		{
			fill (color (gray));
		}

		public static void fill (double gray, double alpha)
		{
			fill (color (gray, alpha));
		}

		public static void fill (double r, double g, double b)
		{
			fill (color (r, g, b));
		}

		public static void fill (double r, double g, double b, double a)
		{
			fill (color (r, g, b, a));
		}

		public static void fill (string hex)
		{
			fill (color (hex));
		}

		public static void fill (string hex, double alpha)
		{
			fill (color (hex, alpha));
		}

		public static void fill (Color color)
		{
			fill_color = color;
		}
/*
*** Color

	function:

		blendColor()
		brightness()
		saturation()
		lerpColor()
		hue()
*/
		public static double alpha (Color value)
		{
			return value.A;
		}

		public static double red (Color value)
		{
			return value.R;
		}

		public static double green (Color value)
		{
			return value.G;
		}

		public static double blue (Color value)
		{
			return value.B;
		}

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
			return Color.FromArgb ((byte) (int) a, (byte) (int) r, (byte) (int) g, (byte) (int) b);
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
*/
		public static PImage loadImage (string uri)
		{
			return new PImage (new BitmapImage (new Uri (uri, UriKind.RelativeOrAbsolute)));
		}

		public static PImage loadImage (string uri, string extension)
		{
			// FIXME: extension is ignored so far
			var cli = new WebClient ();
			var wait = new ManualResetEvent (false);
			Stream result = null;
			cli.OpenReadCompleted += delegate (object sender, OpenReadCompletedEventArgs e) {
				result = e.Result;
				wait.Set ();
			};
			wait.WaitOne ();
			var img = new BitmapImage ();
			img.SetSource (result);
			return new PImage (img);
		}

		public static void image (PImage img, double x, double y)
		{
			image (img, x, y, img.width, img.height);
		}

		public static void image (PImage img, double x, double y, double width, double height)
		{
			var i = img.Image;
			if (img.width != width || img.height != height)
				i.Arrange (new Rect (0, 0, width, height));

			// FIXME: add mask

			Host.Children.Add (i);
		}

		public static void set (double x, double y, Color c)
		{
			var l = new Line ();
			l.X1 = x;
			l.Y1 = y;
			l.X2 = x + 1;
			l.Y2 = y + 1;
			// FIXME: consider fill property
			l.Stroke = new SolidColorBrush (c);;
			Host.Children.Add (l);
		}

/*
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
