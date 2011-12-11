using System;
using System.Linq;
using Android.App;
using Android.Graphics;
using ProcessingCli;
namespace LinearGradient
{
	[Activity (Name = "name.atsushieno.tsukimi.android.sample", Label = "tsukimi sampele", MainLauncher = true)]
public partial class App : ProcessingActivity
{
public App () : base (() => Run ())
{
} // end of App.ctor()

// placeholder for global variables
internal static int Y_AXIS;
internal static int X_AXIS;

// placeholder for global functions
public static void setup ()
{
ProcessingApplication.Current.@size (200, 200);
Color b1 = ProcessingApplication.Current.@color (190, 190, 190);
Color b2 = ProcessingApplication.Current.@color (20, 20, 20);
setGradient (0, 0, ProcessingApplication.Current.width, ProcessingApplication.Current.height, b1, b2, Y_AXIS);
Color c1 = ProcessingApplication.Current.@color (255, 120, 0);
Color c2 = ProcessingApplication.Current.@color (10, 45, 255);
Color c3 = ProcessingApplication.Current.@color (10, 255, 15);
Color c4 = ProcessingApplication.Current.@color (125, 2, 140);
Color c5 = ProcessingApplication.Current.@color (255, 255, 0);
Color c6 = ProcessingApplication.Current.@color (25, 255, 200);
setGradient (25, 25, 75, 75, c1, c2, Y_AXIS);
setGradient (100, 25, 75, 75, c3, c4, X_AXIS);
setGradient (25, 100, 75, 75, c2, c5, X_AXIS);
setGradient (100, 100, 75, 75, c4, c6, Y_AXIS);
}
public static void setGradient (int x, int y, double w, double h, Color c1, Color c2, int axis)
{
double deltaR = ProcessingApplication.Current.@red (c2) - ProcessingApplication.Current.@red (c1);
double deltaG = ProcessingApplication.Current.@green (c2) - ProcessingApplication.Current.@green (c1);
double deltaB = ProcessingApplication.Current.@blue (c2) - ProcessingApplication.Current.@blue (c1);
if (axis == Y_AXIS){
for (int i = x; i <= (x + w); i = i + 1)
{
for (int j = y; j <= (y + h); j = j + 1)
{
Color c = ProcessingApplication.Current.@color ((ProcessingApplication.Current.@red (c1) + (j - y) * (deltaR / h)), (ProcessingApplication.Current.@green (c1) + (j - y) * (deltaG / h)), (ProcessingApplication.Current.@blue (c1) + (j - y) * (deltaB / h)));
ProcessingApplication.Current.@set (i, j, c);
}
}
}
else if (axis == X_AXIS){
for (int i = y; i <= (y + h); i = i + 1)
{
for (int j = x; j <= (x + w); j = j + 1)
{
Color c = ProcessingApplication.Current.@color ((ProcessingApplication.Current.@red (c1) + (j - x) * (deltaR / h)), (ProcessingApplication.Current.@green (c1) + (j - x) * (deltaG / h)), (ProcessingApplication.Current.@blue (c1) + (j - x) * (deltaB / h)));
ProcessingApplication.Current.@set (j, i, c);
}
}
}
}
public static void Run ()
{
Y_AXIS = 1;
X_AXIS = 2;
setup ();
OnApplicationSetup (ProcessingApplication.Current);
}
}
}
