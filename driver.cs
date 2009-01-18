using System;
using System.IO;
using ProcessingCli.Parser;

namespace ProcessingCli
{
	public class CommandLineParser
	{
		public static void Main (string [] args)
		{
			foreach (string s in args) {
				ProcessingParser p = new ProcessingParser (s);
				p.Parse ();
				// was this task so messy? :p
				string fileBodyName = new FileInfo (s).Name;
				int last = fileBodyName.LastIndexOf ('.');
				if (last > 0)
					fileBodyName = fileBodyName.Substring (0, last);
				new CodeGenerator (p.Root, Console.Out, fileBodyName).Generate ();
			}
		}
	}
}

