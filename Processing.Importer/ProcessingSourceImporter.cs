using System;
using System.IO;
using System.Net;
using System.Threading;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class ProcessingSourceImporter
	{
		public static void Import (ProcessingProjectSource source, TextWriter writer)
		{
			ProcessingParser p = new ProcessingParser (source);
			p.Parse ();
			new CodeGenerator (p.Roots, writer, source.NamespaceName).Generate ();
		}
	}
}

