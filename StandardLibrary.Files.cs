using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using PString = System.String;

namespace ProcessingCli
{
	public partial class ProcessingApplication
	{
/*
 		// Input - Files.

		createInput()
		loadBytes()
		loadStrings()
		open()
		selectFolder()
		selectInput()
*/

		public byte [] loadBytes (string file)
		{
			var s = ProcessingUtility.OpenRead (file);
			byte [] bytes = new byte[s.Length];
			s.Read (bytes, 0, bytes.Length);
			s.Close ();
			return bytes;
		}

		// since this function is used in *.pde, the returned
		// value must be still in PString[] (unlike PString,
		// PString[] is not implicitly convertible to string[]).
		public PString [] loadStrings (string file)
		{
			try {
				var stream = ProcessingUtility.OpenRead (file);
				if (stream == null)
					throw new ArgumentException (String.Format ( "File '{0}' does not exist", file));
				var sr = new StreamReader (stream);
				var l = new List<PString> ();
				do {
					var s = sr.ReadLine ();
					if (s == null)
						break;
					l.Add (s);
				} while (true);
				sr.Close ();
				return l.ToArray ();
			} catch (Exception ex) {
				// This is the documented behavior.
				// Though I'm not sure if this catch is okay.
				Console.WriteLine (ex);
				return null;
			}
		}

		public string selectInput ()
		{
			return selectInput (null);
		}

		public string selectInput (string message)
		{
			// message is ignored.

			var d = new OpenFileDialog ();
			d.Multiselect = false;
			d.ShowDialog ();
			return d.File.FullName;
		}
	}
}
