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
		public static int Main (string [] args)
		{
			bool do_package = false;
			List<string> files = new List<string> ();
			foreach (string s in args) {
				if (s == "--xap") {
					do_package = true;
					continue;
				}
				files.Add (s);
			}
			foreach (string s in files) {
				ProcessingProjectSource p;
				if (Directory.Exists (s))
					p = ProcessingProjectSource.FromDirectory (s);
				else
					p = ProcessingProjectSource.FromFile (s);
				Console.WriteLine ("/* ===============");
				Console.WriteLine (p);
				Console.WriteLine ("================*/");

				if (!do_package) {
					StringWriter sw = new StringWriter ();
					ProcessingSourceImporter.Import (p, sw);
					Console.WriteLine (sw);
					continue;
				}

				ProcessingXapImporterClr importer = new ProcessingXapImporterClr (p, true);
				importer.GenerateFile ();
				importer.CopyDependentAssemblies ();
				importer.CreateApplicationPackage ();
				if (importer.ErrorCode != 0)
					return importer.ErrorCode;
			}
			return 0;
		}
	}
}

