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
				ProcessingProjectSource p;
				if (Directory.Exists (s))
					p = ProcessingProjectSource.FromDirectory (s);
				else
					p = ProcessingProjectSource.FromFile (s);
				Console.WriteLine ("/*{0}*/", p);
				ProcessingSourceImporter.Import (p, Console.Out);
			}
		}
	}
}

