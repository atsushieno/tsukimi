using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Threading;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class ProcessingXapImporterClr : ProcessingXapImporter
	{
		bool created;
		DirectoryInfo dir;

		public ProcessingXapImporterClr (ProcessingProjectSource source, bool useCurrent)
			: base (source)
		{
			string path = useCurrent ?
				Directory.GetCurrentDirectory () :
				Path.Combine (Path.GetTempPath (), "tsukimi-" + Guid.NewGuid ());
			if (!Directory.Exists (path)) {
				dir = Directory.CreateDirectory (path);
				created = true;
			}
			else
				dir = new DirectoryInfo (path);
		}

		public override Stream CreateOutputStream (string name)
		{
			return File.Create (Path.Combine (dir.FullName, name));
		}

		public override void CopyDependentAssemblies ()
		{
				string exedir = Path.GetDirectoryName (new Uri (Assembly.GetEntryAssembly ().CodeBase).LocalPath);
				string dll = "Processing.Core.dll";
				string mdb = dll + ".mdb";
				Copy (exedir, dir.FullName, dll);
				Copy (exedir, dir.FullName, mdb);
		}

		public override void CreateApplicationPackage ()
		{
			ProcessingProjectSource p = Source;

			ProcessStartInfo psi = new ProcessStartInfo ();
			psi.FileName = "mxap";
			psi.Arguments = "--application-name " + p.AssemblyName + " --entry-point-type " + p.NamespaceName + ".App";
			psi.WorkingDirectory = dir.FullName;
			Process proc = Process.Start (psi);
			proc.WaitForExit ();
			if (proc.ExitCode != 0) {
				Console.Error.WriteLine ("mxap failed with status code {0}", proc.ExitCode);
				ErrorCode = -1;
				return;
			}
			if (p.DataFiles.Count > 0) {
				var l = new List<string> ();
				l.Add ("-j");
				l.Add (p.NamespaceName + ".xap");
				foreach (var uri in p.DataFiles)
					l.Add (uri.LocalPath);
				psi = new ProcessStartInfo ();
				psi.FileName = "zip";
				psi.WorkingDirectory = dir.FullName;
				psi.Arguments = String.Join (" ", l.ToArray ());
				proc = Process.Start (psi);
				proc.WaitForExit ();
				if (proc.ExitCode != 0) {
					Console.Error.WriteLine ("zip failed with status code {0}", proc.ExitCode);
					ErrorCode = -2;
					return;
				}
			}
		}

		public override void Dispose ()
		{
			if (created)
				dir.Delete ();
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
			else
				Console.Error.WriteLine ("warning!! copy source {0} not found.", srcfile);
		}
	}

	public class ProcessingXapImporterCoreClr : ProcessingXapImporter
	{
		public ProcessingXapImporterCoreClr (ProcessingProjectSource source)
			: base (source)
		{
			isf = IsolatedStorageFile.GetUserStoreForApplication ();
		}

		IsolatedStorageFile isf;

		public override Stream CreateOutputStream (string name)
		{
			return new IsolatedStorageFileStream (name, FileMode.Create, isf);
		}

		public override void CopyDependentAssemblies ()
		{
#if NET_2_1
			var ass = "Processing.Core.dll";
			using (var a = Deployment.Current.AssemblyParts [ass]) {
				using (var f = CreateOutputStream (ass)) {
					byte [] arr = new byte[a.Length];
					a.Read (arr, 0, arr.Length);
					f.Write (arr, 0, arr.Length);
				}
			}
#endif
		}

		public override void CreateApplicationPackage ()
		{
			// It is not possible to run "mxap" on moonlight (or siverlight either).
			throw new NotImplementedException ("We need to sort out how to compile the output .cs files");
		}

		public override void Dispose ()
		{
			isf.Close ();
		}
	}

	public abstract class ProcessingXapImporter : IDisposable
	{
		protected ProcessingXapImporter (ProcessingProjectSource source)
		{
			Source = source;
		}

		public ProcessingProjectSource Source { get; private set; }

		public int ErrorCode { get; set; }

		public abstract void Dispose ();
		public abstract Stream CreateOutputStream (string name);
		public abstract void CopyDependentAssemblies ();
		public abstract void CreateApplicationPackage ();

		public void GenerateFile ()
		{
			ProcessingProjectSource source = Source;
			var sw = new StringWriter ();
			ProcessingSourceImporter.Import (source, sw);
			using (var writer = new StreamWriter (CreateOutputStream (source.AssemblyName + ".cs"))) {
				writer.Write (sw.ToString ());
			}
			using (var writer = new StreamWriter (CreateOutputStream ("AppManifest.xaml"))) {
				writer.Write (CreateAppManifest (source.NamespaceName, source.AssemblyName));
			}
		}

		const string manifest_template = @"
<Deployment xmlns='http://schemas.microsoft.com/client/2007/deployment'
  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
  EntryPointAssembly='ASSEMBLY_NAME'
  EntryPointType='NAMESPACE_NAME.App' RuntimeVersion='2.0.31005.0'>
  <Deployment.Parts>
    <AssemblyPart x:Name='Proce55ing.Core' Source='Proce55ing.Core.dll' />
    <AssemblyPart x:Name='ASSEMBLY_NAME' Source='ASSEMBLY_NAME.dll' />
  </Deployment.Parts>
</Deployment>";

		public static string CreateAppManifest (string namespaceName, string assemblyName)
		{
			return manifest_template.Replace ("ASSEMBLY_NAME", assemblyName).Replace ("NAMESPACE_NAME", namespaceName);
		}
	}
}

