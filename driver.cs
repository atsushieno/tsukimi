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
				ProcessingSourceImporter.ImportFile (s, Console.Out);
			}
		}
	}
}

