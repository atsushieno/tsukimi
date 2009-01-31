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
			string s = source.LocalPath;
			int last = s.LastIndexOf (".pde", StringComparison.OrdinalIgnoreCase);
			if (last > 0)
				s = s.Substring (0, last);
			last = s.LastIndexOf ('/');
			if (last > 0)
				s = s.Substring (last + 1);
			if (s.Length == 0)
				s = "MyApp";
			else if (s [0] == '/')
				s = s.Substring (1);
			// e.g. Convert (Uri("file:///path/to/Test.pde"), "Test", writer);
			Import (source, s, writer);
		}
		
		public static void Import (Uri source, string namespaceName, TextWriter writer)
		{
			WebClient client = new WebClient ();
			ManualResetEvent wait = new ManualResetEvent (false);
			client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
				Import (source, new StringReader (e.Result), namespaceName, writer);
				wait.Set ();
			};
			client.DownloadStringAsync (source);
			wait.WaitOne (180000);
		}

		public static void Import (Uri sourceUri, TextReader source, string namespaceName, TextWriter writer)
		{
			ProcessingParser p = new ProcessingParser (sourceUri, source);
			p.Parse ();
			new CodeGenerator (p.Root, writer, namespaceName).Generate ();
		}
	}
}

