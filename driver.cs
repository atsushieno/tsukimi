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
				new CodeGenerator (p.Root, Console.Out).Generate ();
			}
		}
	}
}

