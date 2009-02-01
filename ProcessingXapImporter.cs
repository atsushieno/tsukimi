using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class ProcessingXapImporter
	{
		public static void ImportFile (ProcessingProjectSource source)
		{
			var sw = new StringWriter ();
			ProcessingSourceImporter.Import (source, sw);
			using (var isf = IsolatedStorageFile.GetUserStoreForApplication ()) {
				using (var writer = new StreamWriter (new IsolatedStorageFileStream (source.NamespaceName + ".cs", FileMode.Create, isf))) {
					writer.Write (sw.ToString ());
				}
				using (var writer = new StreamWriter (new IsolatedStorageFileStream ("AppManifest.xaml", FileMode.Create, isf))) {
					writer.Write (CreateAppManifest (source.NamespaceName));
				}
				// FIXME: run "mxap", but it requires Process support.
				// It is not possible on moonlight (or siverlight either).
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

