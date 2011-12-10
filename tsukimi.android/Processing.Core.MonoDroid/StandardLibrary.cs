using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
#if !DESKTOP && !WINDOWS_PHONE
using System.Windows.Browser;
#endif

/*

Object mappings:
	color -> System.Windows.Media.Color
	char -> System.Char
	float -> System.Double
	int -> System.Int32
	boolean -> System.Boolean
	byte -> System.SByte
	String -> System.String
	Array -> (no direct mapping. System.Array is implicitly used)
	Object -> System.Object

	When converting pde to XAP, it should filter every
	method invocation to reject invalid calls. For example,
	call to GetHashCode() in processing object must be rejected
	since it does not exist in processing.
*/

using PString = System.String;
using ColorMode = System.Int32;

namespace ProcessingCli
{
	public partial class ProcessingApplication : Application
	{
		public static ProcessingApplication Current { get; set; }

		public ProcessingApplication (Action run)
		{
			Current = this;
			Startup += delegate {
				var c = new Canvas ();
				c.Loaded += delegate { run (); };
				this.RootVisual = c;
				ProcessingApplication.Current.SetHost (c);
			};
		}
		
		public static void RegisterDraw (Action action)
		{
#if DESKTOP || WINDOWS_PHONE
            action ();
#else
			DispatcherTimer timer = new DispatcherTimer ();
			timer.Interval = TimeSpan.FromMilliseconds (1000 / 60);
			timer.Tick += ((o, e) => System.Windows.Browser.HtmlPage.Window.Dispatcher.BeginInvoke (action));
			timer.Start ();
#endif
		}

		// FIXME: those enum constants are not valid approach,
		// since some enums overlap names in different types.
		[ProcessingStandardField]
		public const double PI = System.Math.PI;
		[ProcessingStandardField]
		public const double HALF_PI = PI / 2.0;
		[ProcessingStandardField]
		public const double TWO_PI = PI * 2.0;
		[ProcessingStandardField]
		public const Constants Center = Constants.Center;
		[ProcessingStandardField]
		public const Constants RADIUS = Constants.Radius;
		[ProcessingStandardField]
		public const Constants CORNER = Constants.Corner;
		[ProcessingStandardField]
		public const Constants CORNERS = Constants.Corners;
		[ProcessingStandardField]
		public const Constants LEFT = Constants.Left;
		[ProcessingStandardField]
		public const Constants CENTER = Constants.Center;
		[ProcessingStandardField]
		public const Constants RIGHT = Constants.Right;
		[ProcessingStandardField]
		public const Constants TOP = Constants.Top;
		[ProcessingStandardField]
		public const Constants BOTTOM = Constants.Bottom;
		[ProcessingStandardField]
		public const Constants BASELINE = Constants.Baseline;
		[ProcessingStandardField]
		public const Constants ROUND = Constants.Round;
		[ProcessingStandardField]
		public const Constants MITER = Constants.Miter;
		[ProcessingStandardField]
		public const Constants BEVEL = Constants.Bevel;
		[ProcessingStandardField]
		public const Constants PROJECT = Constants.Project;
		[ProcessingStandardField]
		public const Constants SQUARE = Constants.Square;

		// They are documented in colorMode() page...
		[ProcessingStandardField]
		public const int RGB = 1;

		[ProcessingStandardField]
		public const int HSB = 2;

		Stack<Panel> host_stack = new Stack<Panel> ();
		public Panel Host { // set by application
			get { return host_stack.Count == 0 ? null : host_stack.Peek (); }
		}
			
		public void SetHost (Panel panel)
		{
			if (host_stack.Count != 0)
				throw new InvalidOperationException ("Internal error: SetHost() must not be called twice");
			host_stack.Push (panel);
			Host.MouseMove += delegate (object o, MouseEventArgs e) { current_mouse = e; };
		}

		protected static void OnApplicationSetup (ProcessingApplication app)
		{
			app.OnApplicationSetup ();
		}

		protected virtual void OnApplicationSetup ()
		{
		}

		public TextWriter StandardOutput = Console.Out;

		SolidColorBrush stroke_brush;
		double stroke_weight = 1.0;
		PenLineJoin stroke_join = PenLineJoin.Miter;
		PenLineCap stroke_cap = PenLineCap.Round;

		Color? stroke_color {
			get { return stroke_brush != null ? (Color?) stroke_brush.Color : null; }
			set { stroke_brush = value != null ? new SolidColorBrush ((Color) value) : null; }
		}

		SolidColorBrush fill_brush;

		Color? fill_color {
			get { return fill_brush != null ? (Color?) fill_brush.Color : null; }
			set { fill_brush = value != null ? new SolidColorBrush ((Color) value) : null; }
		}

		[ProcessingStandardField]
		public int width {
			get { return (int) Host.Width; }
		}

		[ProcessingStandardField]
		public int height {
			get { return (int) Host.Height; }
		}

		// The functions below are not defined in the standard library.
		// They are resolved only internally.
		//
		//	- setup() : Application.Start
		//	- draw() : after setup()
		//
		// (It is possible that noLoop() and loop() could become
		// like them.)

		public void exit ()
		{
			throw new NotImplementedException ();
		}

		public void size (int width, int height)
		{
			size (width, height, SizeMode.Java2D);
		}

		public void size (int width, int height, SizeMode sizeMode)
		{
			// FIXME: check sizeMode and reject some.

			Host.Width = width;
			Host.Height = height;
		}

		public void noLoop ()
		{
			Console.WriteLine ("WARNING: not implemented function noLoop()");
		}

		public void delay (int milliseconds)
		{
			Thread.Sleep (milliseconds);
		}

		public void loop ()
		{
			throw new NotImplementedException ();
		}

		public void redraw ()
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
		[ProcessingStandardField]
		public int frameRateSetting = 60;
		// score of the previous second
		[ProcessingStandardField]
		public int frameRateResult = 10;
		// Incremented in each draw(). After one second passed, it is let to frameRateResult and reset.
		int frame_rate_counting = 0;

		public void frameRate (int value)
		{
			frameRateSetting = value;
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
*/
		[ProcessingStandardField]
		public char key;

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

		MouseEventArgs current_mouse;

		[ProcessingStandardField]
		public double mouseX {
			get { return current_mouse == null ? 0 : current_mouse.GetPosition (null).X; }
		}

		[ProcessingStandardField]
		public double mouseY {
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
*/
		public int year () { return DateTime.Now.Year; }
		public int month () { return DateTime.Now.Month; }
		public int day () { return DateTime.Now.Day; }
		public int hour () { return DateTime.Now.Hour; }
		public int minute () { return DateTime.Now.Minute; }
		public int second () { return DateTime.Now.Second; }
		public int millis () { return DateTime.Now.Millisecond; }

/*
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
		public PrintWriter createWriter (string filename)
		{
			return new PrintWriter (new StreamWriter (ProcessingUtility.OpenWrite (filename)));
		}

		public void print (object obj)
		{
			StandardOutput.Write (obj);
		}
		public void println (object obj)
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
		public PImage loadImage (string uri)
		{
			return loadImage (uri, uri.Substring (uri.LastIndexOf ('.') + 1));
		}

		public PImage loadImage (string uri, string extension)
		{
			// FIXME: extension is ignored so far
			var img = new BitmapImage ();
			img.SetSource (ProcessingUtility.OpenRead (uri));
			return new PImage (img);
		}

		public void image (PImage img, double x, double y)
		{
			image (img, x, y, img.width, img.height);
		}

		public void image (PImage img, double x, double y, double width, double height)
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

		public void set (double x, double y, Color c)
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
