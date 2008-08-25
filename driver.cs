using System;
using System.IO;

namespace ProcessingDlr
{
	public class CommandLineParser
	{
		public static void Main (string [] args)
		{
			foreach (string s in args) {
				ProcessingParser p = new ProcessingParser (s);
				p.Parse ();
			}
		}
	}
}

