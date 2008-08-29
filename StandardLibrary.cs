using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;

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

	public static partial class StandardLibrary
	{
		public const double PI = System.Math.PI;
		public const double HALF_PI = PI / 2.0;
		public const double TWO_PI = PI * 2.0;

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

			Geometry g = Application.Current.RootVisual.Clip;
			g.Bounds.Width = width;
			g.Bounds.Height = height;
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
*** Color

	function:
		background()
		colorMode()
		stroke()
		noFill()
		noStroke()
		fill()

		blendColor()
		red()
		brightness()
		blue()
		saturation()
		lerpColor()
		green()
		hue()
		alpha()
		color()

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

*** Math

	function:
		min()
		max()
		round()
		dist()
		exp()
		pow()
		floor()
		sqrt()
		abs()
		constrain()
		norm()
		mag()
		log()
		lerp()
		sq()
		ceil()
		map()

		acos()
		tan()
		sin()
		cos()
		degrees()
		atan2()
		atan()
		radians()
		asin()

		noise()
		noiseSeed()
		randomSeed()
		noiseDetail()
		random()
*/
	}
}
