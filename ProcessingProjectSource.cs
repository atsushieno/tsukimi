using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ProcessingCli.Parser;

//
// The processing project structure is defined here as:
//
//	- A directory is regarded as a package for the entire project
//	- *.pde files are the sources. If there is a *.pde file that
//	  has the same name as the directory, then it is the primary source.
//	- since processing is based on Java, it also supports raw java
//	  sources. Since we don't support Java, we just skip them.
//	- all content resources are put in "data" subdirectory.

namespace ProcessingCli
{
	public class ProcessingProjectSource
	{
		public string NamespaceName { get; set; }
		public List<Uri> DataFiles { get; private set; }
		public List<Uri> Sources { get; private set; }

		public static ProcessingProjectSource FromDirectory (string path)
		{
			path = Path.GetFullPath (path);
			var uri = new Uri (path);
			var ns = DetermineNamespace (uri);
			var p = new ProcessingProjectSource () {NamespaceName = ns};
			foreach (var s in Directory.GetFiles (path, "*.pde"))
				p.Sources.Add (new Uri (Path.GetFullPath (s)));
			string datapath = Path.Combine (path, "data");
			if (Directory.Exists (datapath))
				foreach (var s in Directory.GetFiles (datapath))
					p.DataFiles.Add (new Uri (Path.GetFullPath (s)));
			foreach (var s in Directory.GetFiles (path, "*.java"))
				Console.WriteLine ("WARNING: we do not support Java sources");
			return p;
		}

		public static ProcessingProjectSource FromFile (string source)
		{
			return FromUri (new Uri (Path.GetFullPath (source)));
		}

		public static ProcessingProjectSource FromUri (Uri source)
		{
			var ns = DetermineNamespace (source);
			var p = new ProcessingProjectSource () {NamespaceName = ns};
			p.Sources.Add (source);
			return p;
		}

		public static string DetermineNamespace (Uri source)
		{
			string s = source.LocalPath;
			int last = s.LastIndexOf (".pde", StringComparison.OrdinalIgnoreCase);
			if (last > 0)
				s = s.Substring (0, last);
			last = s.LastIndexOf (Path.DirectorySeparatorChar);
			if (last > 0)
				s = s.Substring (last + 1);
			if (s.Length == 0)
				s = "MyApp";
			else if (s [0] == Path.DirectorySeparatorChar)
				s = s.Substring (1);
			return s;
		}

		public ProcessingProjectSource ()
		{
			Sources = new List<Uri> ();
			DataFiles = new List<Uri> ();
		}
		
		public TextReader OpenInput (Uri uri)
		{
			WebClient client = new WebClient ();
			ManualResetEvent wait = new ManualResetEvent (false);
			string result = null;
			client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
				result = e.Result;
				wait.Set ();
			};
			client.DownloadStringAsync (uri);
			wait.WaitOne (180000);
			return new StringReader (result);
		}

		public override string ToString ()
		{
			return String.Format (@"ProcessingProjectSource {{
Namespace: {0}
Sources:
{1}
Data:
{2}
}}", NamespaceName,
				String.Join (Environment.NewLine, (from uri in Sources select uri.LocalPath).ToArray ()),
				String.Join (Environment.NewLine, (from uri in DataFiles select uri.LocalPath).ToArray ()));
		}
	}
}

