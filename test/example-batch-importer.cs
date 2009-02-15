using System;
using System.Diagnostics;
using System.IO;

public class Test
{
	public static void Main (string [] args)
	{
		string dir = Path.GetFullPath (args [0]);
		string cur = Path.GetFullPath (args [1]);

		foreach (var topcat in new DirectoryInfo (dir).GetDirectories ()) {
			if (topcat.Name [0] == '.') continue;
			string topfull = topcat.FullName;
			string dsttopfull = Path.Combine (cur, topcat.Name);
			DirectoryInfo dsttop;
			if (!Directory.Exists (dsttopfull))
				dsttop = Directory.CreateDirectory (dsttopfull);
			else
				dsttop = new DirectoryInfo (dsttopfull);
		
			foreach (var category in new DirectoryInfo (topfull).GetDirectories ()) {
				if (category.Name [0] == '.') continue;
				string catfull = category.FullName;
				string dstcatfull = Path.Combine (dsttopfull, category.Name);
				DirectoryInfo dstcat;
				if (!Directory.Exists (dstcatfull))
					dstcat = Directory.CreateDirectory (dstcatfull);
				else
					dstcat = new DirectoryInfo (dstcatfull);
	
				foreach (var app in new DirectoryInfo (catfull).GetDirectories ()) {
					if (app.Name [0] == '.') continue;
					string appfull = app.FullName;
					string dstappfull = Path.Combine (dstcat.FullName, app.Name);
					if (!Directory.Exists (dstappfull))
						Directory.CreateDirectory (dstappfull);
					var psi = new ProcessStartInfo ();
					psi.FileName = "mono";
					psi.WorkingDirectory = dstappfull;
					psi.UseShellExecute = true;
					string exepath = Path.GetFullPath ("../../tsukimi-tool.exe");
					psi.Arguments = "--debug " + exepath + " --xap " + appfull;
					var p = Process.Start (psi);
					p.WaitForExit ();
				}
			}
		}
	}
}
