using System;
using System.IO;
using System.Net;
using System.Threading;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class ProcessingProjectSource
	{
		public string NamespaceName { get; set; }
		public Uri SourceUri { get; set; }

		public static ProcessingProjectSource FromFile (string source)
		{
			return FromUri (new Uri (Path.GetFullPath (source)));
		}

		public static ProcessingProjectSource FromUri (Uri source)
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
			// e.g. .ctor(Uri("file:///path/to/Test.pde"), "Test");
			return new ProcessingProjectSource () {SourceUri = source, NamespaceName = s};
		}

		public TextReader OpenInput ()
		{
			WebClient client = new WebClient ();
			ManualResetEvent wait = new ManualResetEvent (false);
			string result = null;
			client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
				result = e.Result;
				wait.Set ();
			};
			client.DownloadStringAsync (SourceUri);
			wait.WaitOne (180000);
			return new StringReader (result);
		}
	}
}

