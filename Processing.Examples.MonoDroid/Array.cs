using System;
using System.Linq;
using ProcessingCli;
using Android.App;

namespace Array
{
public partial class App : ProcessingActivity
{
public App () : base (() => Run ())
{
} // end of App.ctor()

// placeholder for global variables
internal static double [] coswave;

// placeholder for global functions
public static void Run ()
{
ProcessingApplication.Current.@size (200, 200);
coswave = new double [ProcessingApplication.Current.width];
for (int i = 0; i < ProcessingApplication.Current.width; i = i + 1)
{
double amount = ProcessingApplication.Current.@map (i, 0, ProcessingApplication.Current.width, 0, ProcessingApplication.PI);
App.coswave [i] = ProcessingApplication.Current.@abs (ProcessingApplication.Current.@cos (amount));
}
for (int i = 0; i < ProcessingApplication.Current.width; i = i + 1)
{
ProcessingApplication.Current.@stroke (App.coswave [i] * 255);
ProcessingApplication.Current.@line (i, 0, i, ProcessingApplication.Current.height / 3);
}
for (int i = 0; i < ProcessingApplication.Current.width; i = i + 1)
{
ProcessingApplication.Current.@stroke (App.coswave [i] * 255 / 4);
ProcessingApplication.Current.@line (i, ProcessingApplication.Current.height / 3, i, ProcessingApplication.Current.height / 3 * 2);
}
for (int i = 0; i < ProcessingApplication.Current.width; i = i + 1)
{
ProcessingApplication.Current.@stroke (255 - App.coswave [i] * 255);
ProcessingApplication.Current.@line (i, ProcessingApplication.Current.height / 3 * 2, i, ProcessingApplication.Current.height);
}
OnApplicationSetup (ProcessingApplication.Current);
}
}
}
