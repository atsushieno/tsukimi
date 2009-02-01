using System;
using System.IO;
using System.Net;
using System.Threading;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class ProcessingSourceImporter
	{
		public static void ImportFile (string sourceFileName, TextWriter writer)
		{
			Import (new Uri (Path.GetFullPath (sourceFileName)), writer);
		}

		public static void Import (Uri source, TextWriter writer)
		{
			Import (ProcessingProjectSource.FromUri (source), writer);
		}
		
		public static void Import (ProcessingProjectSource source, TextWriter writer)
		{
			using (var reader = source.OpenInput ()) {
				ProcessingParser p = new ProcessingParser (source.SourceUri, reader);
				p.Parse ();
				new CodeGenerator (p.Root, writer, source.NamespaceName).Generate ();
			}
		}
	}
}

