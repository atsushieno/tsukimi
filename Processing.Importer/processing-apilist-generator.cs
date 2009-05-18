using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace ProcessingCli.ApiGeneration
{
	public class ApiGenerator
	{
		public static void Main (string [] args)
		{
			var ass = AssemblyFactory.GetAssembly (
				File.OpenRead (args [0]));
			var mod = ass.Modules [0];
			TypeDefinition app_type = null, fld_type = null;
			foreach (TypeDefinition t in mod.Types) {
				switch (t.Name) {
				case "ProcessingApplication":
					app_type = t; break;
				case "ProcessingStandardFieldAttribute":
					fld_type = t; break;
				}
				if (app_type != null && fld_type != null)
					break;
			}

			// methods
			foreach (MethodDefinition m in app_type.Methods)
				if (m.IsPublic)
					Console.WriteLine ("Method " + m.Name);
			// fields (and properties)
			foreach (FieldDefinition f in app_type.Fields)
				if (f.CustomAttributes.Find<CustomAttribute> (x => x.Constructor.DeclaringType.Name == "ProcessingStandardFieldAttribute") != null)
					Console.WriteLine ("{0} {1}", f.IsStatic || f.HasConstant ? "Const" : "Field", f.Name);
			foreach (PropertyDefinition f in app_type.Properties)
				if (f.CustomAttributes.Find<CustomAttribute> (x => x.Constructor.DeclaringType.Name == "ProcessingStandardFieldAttribute") != null)
					Console.WriteLine ("{0} {1}", f.GetMethod.IsStatic || f.HasConstant ? "Const" : "Field", f.Name);
		}
	}

	static class Extensions
	{
		public delegate bool Cond<T> (T obj);

		public static T Find<T> (this CollectionBase list, Cond<T> f)
		{
			foreach (T obj in list)
				if (f (obj))
					return obj;
			return default (T);
		}
	}
}
