using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ProcessingCli;

namespace ProcessingCli
{
	public class CommandLineConverter
	{
		public static void Main (string [] args)
		{
			new CommandLineConverter ().Run (args);
		}

		public void Run (string [] args)
		{
			string exedir = Path.GetDirectoryName (new Uri (Assembly.GetEntryAssembly ().CodeBase).LocalPath);
			string curdir = Directory.GetCurrentDirectory ();
			bool do_package = false;
			foreach (string s in args) {
				if (s == "--xap") {
					do_package = true;
					continue;
				}
				ProcessingProjectSource p;
				if (Directory.Exists (s))
					p = ProcessingProjectSource.FromDirectory (s);
				else
					p = ProcessingProjectSource.FromFile (s);
				Console.WriteLine ("/* ===============");
				Console.WriteLine (p);
				Console.WriteLine ("================*/");
				StringWriter sw = new StringWriter ();
				ProcessingSourceImporter.Import (p, sw);

				if (!do_package) {
					Console.WriteLine (sw);
					continue;
				}

				using (var tw = File.CreateText (p.NamespaceName + ".cs"))
					tw.Write (sw.ToString ());
				using (var tw = File.CreateText ("AppManifest.xaml"))
					tw.Write (CreateAppManifest (p.NamespaceName));
				string dll = "Processing.Core.dll";
				string mdb = dll + ".mdb";
				Copy (exedir, curdir, dll);
				Copy (exedir, curdir, mdb);
				Process.Start ("mxap").WaitForExit ();
				if (p.DataFiles.Count > 0) {
					var l = new List<string> ();
					l.Add ("-j");
					l.Add (p.NamespaceName + ".xap");
					foreach (var uri in p.DataFiles)
						l.Add (uri.LocalPath);
					Process.Start ("zip", String.Join (" ", l.ToArray ())).WaitForExit ();
				}
			}
		}

		static void Copy (string exedir, string curdir, string file)
		{
			string srcfile = Path.Combine (exedir, file);
			string dstfile = Path.Combine (curdir, file);
			if (File.Exists (srcfile)) {
				if (File.Exists (dstfile))
					File.Delete (dstfile);
				File.Copy (srcfile, dstfile);
			}
		}

		const string manifest_template = @"
<Deployment xmlns='http://schemas.microsoft.com/client/2007/deployment' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' EntryPointAssembly='REPLACE_HERE' EntryPointType='REPLACE_HERE.App' RuntimeVersion='2.0.31005.0'>
  <Deployment.Parts>
    <AssemblyPart x:Name='Proce55ing.Core' Source='Proce55ing.Core.dll' />
    <AssemblyPart x:Name='REPLACE_HERE' Source='REPLACE_HERE.dll' />
  </Deployment.Parts>
</Deployment>";

		public static string CreateAppManifest (string namespaceName)
		{
			return manifest_template.Replace ("REPLACE_HERE", namespaceName);
		}

	}
}

