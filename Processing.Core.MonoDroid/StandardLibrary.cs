using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Android.App;
using Android.Graphics;
using Android.Text;
using Android.Views;

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
	public partial class ProcessingApplication : Activity
	{
		public static ProcessingApplication Current { get; set; }
		
		Action run;
		public ProcessingApplication (Action run)
		{
			this.run = run;
		}
		
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			run ();
		}
		
		Timer timer;
		
		public static void RegisterDraw (Action action)
		{
			ProcessingApplication.Current.RegisterDrawTimer (action);
		}
		
		void RegisterDrawTimer (Action action)
		{
			timer = new Timer ((state) => {
				host = view.Holder.LockCanvas ();
				action ();
				view.Holder.UnlockCanvasAndPost (host);
			});
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
		
		Canvas host;
		SurfaceView view;
		public Canvas Host { // set by application
			get { return host; }
		}
		TextPaint paint = new TextPaint ();
		public TextPaint HostPaint {
			get { return paint; }
		}
			
		public void SetHost (SurfaceView view)
		{
			if (this.view != null)
				throw new InvalidOperationException ("Internal error: SetHost() must not be called twice");
			this.view = view;
		}
		
		public override bool OnTouchEvent (MotionEvent e)
		{
			current_mouse = e;
			return base.OnTouchEvent (e);
		}

		protected static void OnApplicationSetup (ProcessingApplication app)
		{
			app.OnApplicationSetup ();
		}

		protected virtual void OnApplicationSetup ()
		{
		}

		public TextWriter StandardOutput = Console.Out;

		double stroke_weight = 1.0;
		Paint.Join stroke_join = Paint.Join.Miter;
		Paint.Cap stroke_cap = Paint.Cap.Round;

		Color stroke_color { get; set; }

		Color? fill_color { get; set; }

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
			Host.SetViewport (width, height);
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

		MotionEvent current_mouse;

		[ProcessingStandardField]
		public double mouseX {
			get { return current_mouse.GetX (); }
		}

		[ProcessingStandardField]
		public double mouseY {
			get { return current_mouse.GetY (); }
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
			return new PImage (BitmapFactory.DecodeStream (Assets.Open (uri)));
			// FIXME: extension is ignored so far
		}

		public void image (PImage img, double x, double y)
		{
			image (img, x, y, img.width, img.height);
		}

		public void image (PImage img, double x, double y, double width, double height)
		{
			Host.DrawBitmap (img.Image, new Rect (0, 0, img.Image.Width, img.Image.Height), ToRectF (x, y, width, height), HostPaint);
			// FIXME: add mask (as alpha channel)
			//foreach (var mask in img.Masks)
			//	image (mask, x, y, width, height);
		}

		public void set (double x, double y, Color c)
		{
			var bak = HostPaint.Color;
			HostPaint.Color = c.ToArgb ();
			Host.DrawPoint ((float) x, (float) y, HostPaint);
			HostPaint.Color = bak;
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
