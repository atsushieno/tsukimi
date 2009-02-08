using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
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
	public static class ProcessingExtensions
	{
		public static int charAt (this string s, int index)
		{
			return s [index];
		}

		public static bool equals (this string ss, String s)
		{
			return ss == s;
		}

		public static int indexOf (this string ss, String s)
		{
			return indexOf (ss, s, 0);
		}

		public static int indexOf (this string ss, String s, int startIndex)
		{
			return ss.IndexOf (s, startIndex, StringComparison.Ordinal);
		}

		public static int length (this string s)
		{
			return s.Length;
		}

		public static string substring (this string s, int start)
		{
			return s.Substring (start);
		}

		public static string substring (this string s, int start, int end)
		{
			return s.Substring (start, end - start);
		}

		public static string toLowerCase (this string s)
		{
			return s.ToLower (CultureInfo.InvariantCulture);
		}

		public static string toUpperCase (this string s)
		{
			return s.ToUpper (CultureInfo.InvariantCulture);
		}

		// FIXME: the specification requires that String implements
		// all methods in J2SE java.lang.String.

		public static bool startsWith (this string ss, String s)
		{
			return ss.StartsWith (s, StringComparison.Ordinal);
		}

		public static bool endsWith (this string ss, String s)
		{
			return ss.EndsWith (s, StringComparison.Ordinal);
		}
	}

	public class PrintWriter
	{
		TextWriter writer;

		internal PrintWriter (TextWriter writer)
		{
			this.writer = writer;
		}

		public void print (object obj)
		{
			writer.Write (obj);
		}

		public void println ()
		{
			writer.WriteLine ();
		}

		public void println (object obj)
		{
			writer.WriteLine (obj);
		}

		public void flush ()
		{
			writer.Flush ();
		}

		public void close ()
		{
			writer.Close ();
		}
	}
	
	public class ProcessingUtility
	{
		// e.g.:
		// a = new int [3][4][5] => a = new int [3][][];
		// for (int i=0;i<3;i++) a[i]=C(typeof(int),1, dims);
			// a = new int [4][];
			// for (int i=0;i<4;i++) a[i]=C(typeof(int), 2, dims);
				// a = new int[5];
		public static object CreateMultiDimentionArray (Type type, int dimIdx, params int [] dims)
		{
			if (dims [dimIdx] < 0)
				return null; // not initialized

			if (dimIdx + 1 == dims.Length)
				return Array.CreateInstance (type, dims [dimIdx]);
			else {
				Array a = Array.CreateInstance (type.MakeArrayType (), dims [dimIdx]);
				for (int i = 0; i < dims [dimIdx]; i++)
					a.SetValue (CreateMultiDimentionArray (type, dimIdx + 1, dims), i);
				return a;
			}
		}

		public static Stream OpenRead (string s)
		{
			/*
			var cli = new WebClient ();
			var wait = new ManualResetEvent (false);
			Stream result = null;
			cli.OpenReadCompleted += delegate (object sender, OpenReadCompletedEventArgs e) {
				result = e.Result;
				wait.Set ();
			};
			cli.OpenReadAsync (new Uri (s, UriKind.RelativeOrAbsolute));
			wait.WaitOne ();
			return result;
			*/
			var sri = Application.GetResourceStream (new Uri (s, UriKind.RelativeOrAbsolute));
			if (sri == null)
				return null;
			return sri.Stream;
		}
		
		public static Stream OpenWrite (string s)
		{
			var isf = IsolatedStorageFile.GetUserStoreForApplication ();
			return new IsolatedStorageFileStream (s, FileMode.Create, isf);
		}
	}
	public class PString
	{
		string s;

		public PString (string s)
		{
			this.s = s;
		}

		public static implicit operator PString (string s)
		{
			return new PString (s);
		}

		public static implicit operator string (String s)
		{
			return s.s;
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
		[ProcessingStandardField]
		public const EllipseMode Center = EllipseMode.Center;
		[ProcessingStandardField]
		public const EllipseMode RADIUS = EllipseMode.Radius;
		[ProcessingStandardField]
		public const EllipseMode CORNER = EllipseMode.Corner;
		[ProcessingStandardField]
		public const EllipseMode CORNERS = EllipseMode.Corners;

		// They are documented in colorMode() page...
		[ProcessingStandardField]
		public const int RGB = 1;

		[ProcessingStandardField]
		public const int HSB = 2;

		
		public static Canvas Host; // set by application

		public static void SetHost (Canvas canvas)
		{
			Host = canvas;
			Host.MouseMove += delegate (object o, MouseEventArgs e) { current_mouse = e; };
		}
		
		public static TextWriter StandardOutput = Console.Out;

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
*/
		// They are written in "key" and "keyCode" pages...
		[ProcessingStandardField]
		public const char BACKSPACE = '\x8';

		[ProcessingStandardField]
		public const char TAB = '\x9';

		[ProcessingStandardField]
		public const char ENTER = '\xA';

		[ProcessingStandardField]
		public const char RETURN = '\xD';

		[ProcessingStandardField]
		public const char ESC = '\x1B';

		[ProcessingStandardField]
		public const char DELETE = '\x7F';

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
*/
		public static PrintWriter createWriter (string filename)
		{
			return new PrintWriter (new StreamWriter (ProcessingUtility.OpenWrite (filename)));
		}

		public static void print (object obj)
		{
			StandardOutput.Write (obj);
		}
		public static void println (object obj)
		{
			StandardOutput.WriteLine (obj);
		}
/*
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

		*alpha()
		*red()
		*blue()
		*green()
		*color()
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
			uint v = uint.Parse (hex.Substring (1), NumberStyles.HexNumber);
			if (hex.Length == 7)
				v |= 0xFF000000;
			return Color.FromArgb ((byte) (v >> 24), (byte) ((v & 0xFF0000) >> 16), (byte) ((v & 0xFF00) >> 8), (byte) (v & 0xFF));
		}

		public static Color color (string hex, double alpha)
		{
			Color c = color (hex);
			c.A = (byte) alpha;
			return c;
		}

		public static Color lerpColor (Color c1, Color c2, double amt)
		{
			return Color.FromArgb ((byte) lerp (c1.A, c2.A, amt),
								    (byte) lerp (c1.R, c2.R, amt),
								    (byte) lerp (c1.G, c2.G, amt),
								    (byte) lerp (c1.B, c2.B, amt));
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
			return loadImage (uri, uri.Substring (uri.LastIndexOf ('.') + 1));
		}

		public static PImage loadImage (string uri, string extension)
		{
			// FIXME: extension is ignored so far
			var img = new BitmapImage ();
			img.SetSource (ProcessingUtility.OpenRead (uri));
			return new PImage (img);
		}

		public static void image (PImage img, double x, double y)
		{
			image (img, x, y, img.width, img.height);
		}

		public static void image (PImage img, double x, double y, double width, double height)
		{
			var i = img.Image;
			Host.Children.Add (i);
			img.SetSource ();
			i.UpdateLayout ();
			// FIXME: width/height calculation is not done yet.
//			if (img.width != width || img.height != height)
//				i.Arrange (new Rect (0, 0, width, height));

			// FIXME: add mask (as alpha channel)
			//foreach (var mask in img.Masks)
			//	image (mask, x, y, width, height);
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
*/
	}
}
