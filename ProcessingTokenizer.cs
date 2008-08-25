using System;
using System.Collections.Generic;
using System.IO;

namespace ProcessingDlr
{
	public class ProcessingSource : IDisposable
	{
		TextReader reader;
		bool should_dispose;

		public ProcessingSource (string filename)
		{
			should_dispose = true;
			reader = new StreamReader (filename);
		}

		public ProcessingSource (TextReader reader)
		{
			this.reader = reader;
		}

		public void Dispose ()
		{
			if (should_dispose)
				reader.Close ();
			should_dispose = false;
		}
	}

	public class CompilationUnit : IDisposable
	{
		public List<ProcessingSource> Sources = new List<ProcessingSource> ();

		public void Dispose ()
		{
			foreach (var s in Sources)
				s.Dispose ();
		}
	}

	class Tokenizer : ProcessingDlr.yyParser.yyInput
	{
		CompilationUnit unit;

		public Tokenizer (TextReader reader)
		{
			unit = new CompilationUnit ();
			unit.Sources.Add (new ProcessingSource (reader));
		}

		public void Dispose ()
		{
			unit.Dispose ();
		}
	}
}
