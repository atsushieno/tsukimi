using System;
using System.Collections.Generic;
using System.IO;

namespace ProcessingDlr
{
	public class ProcessingSource : IDisposable
	{
		TextReader reader;

		public ProcessingSource (TextReader reader)
		{
			this.reader = reader;
		}
	}

	public class CompilationUnit
	{
		public List<ProcessingSource> Sources = new List<ProcessingSource> ();
	}

	class Tokenizer : ProcessingDlr.yyParser.yyInput
	{
		CompilationUnit unit;

		public Tokenizer (TextReader reader)
		{
			unit = new CompilationUnit ();
			unit.Sources.Add (new ProcessingSource (reader));
		}
	}
}
