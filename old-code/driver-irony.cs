using System;
using System.IO;

namespace ProcessingDLR
{
	public class CommandLineParser
	{
		public static void Main (string [] args)
		{
			ProcessingAstVisitor v = new ProcessingAstVisitor ();
			foreach (string s in args) {
				var l = new LanguageCompiler (new ProcessingGrammar ());
				var n = l.Parse (s);
				foreach (SyntaxError e in l.Context.Errors)
					Console.WriteLine (e);
				if (l.Context.Errors.Count > 0)
					break;
				if (n == null)
					Console.WriteLine ("null parser result");
				else
					n.AcceptVisitor (v);
			}
		}
	}
}

