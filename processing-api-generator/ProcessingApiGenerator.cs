// It is used to generate apilist.txt which is embedded to Processing.Importer.dll.
// If it is not updated, then target processing apps won't be converted accordingly.
using System;
using System.Linq;
using Mono.Cecil;

namespace ProcessingApiGenerator
{
	class Driver
	{
		public static void Main (string[] args)
		{
			var ass = AssemblyFactory.GetAssembly (args [0]);
			foreach (ModuleDefinition mod in ass.Modules)
				foreach (TypeDefinition type in mod.Types)
					if (type.Name == "ProcessingApplication")
						Run (type);
		}
		
		static void ProcessField (IMemberDefinition m)
		{
			foreach (CustomAttribute att in m.CustomAttributes)
				if (att.Constructor.DeclaringType.Name == "ProcessingStandardFieldAttribute") {
					var f = m as FieldDefinition;
					if (f != null && f.IsLiteral)
						Console.WriteLine ("Const " + m.Name);
					else
						Console.WriteLine ("Field " + m.Name);
				}
		}
		static void Run (TypeDefinition type)
		{
			foreach (IMemberDefinition field in type.Fields)
				ProcessField (field);
			foreach (IMemberDefinition field in type.Properties)
				ProcessField (field);

			foreach (MethodDefinition method in type.Methods)
				if ((method.Attributes & MethodAttributes.Public) != 0)
				Console.WriteLine ("Method " + method.Name);
		}
	}
}
