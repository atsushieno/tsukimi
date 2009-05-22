using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using ProcessingCli.Ast;

namespace ProcessingCli
{
	public class CodeGenerator
	{
		List<AstRoot> roots;
		TextWriter w;
		string namespace_name;

		List<ClassDefinition> classes = new List<ClassDefinition> ();
		List<GlobalFunctionDefinition> funcs = new List<GlobalFunctionDefinition> ();
		List<Statement> stmts = new List<Statement> ();

		bool writeSemicolon = true;
		ClassDefinition current_class;
		bool in_global_context;

		class VariableScope : List<VariableDeclarationStatement>
		{
		}

		List<VariableScope> variable_stack = new List<VariableScope> ();
		VariableScope global_variables = new VariableScope ();

		VariableScope current_variable_scope {
			get { return variable_stack.Count > 0 ? variable_stack [variable_stack.Count - 1] : global_variables; }
		}

		public CodeGenerator (List<AstRoot> roots, TextWriter writer, string namespaceName)
		{
			this.roots = roots;
			this.w = writer;
			this.namespace_name = namespaceName ?? "ProcessingCliApplication";

			foreach (var root in roots) {
				foreach (ITopLevelContent c in root.Items) {
					if (c is ClassDefinition)
						classes.Add ((ClassDefinition) c);
					else if (c is GlobalFunctionDefinition)
						funcs.Add ((GlobalFunctionDefinition) c);
					else if (c is Statement)
						stmts.Add ((Statement) c);
				}
			}
		}

		public void Generate ()
		{
			w.WriteLine ("using System;");
			w.WriteLine ("using System.Linq;");
			w.WriteLine ("using System.Windows;");
			w.WriteLine ("using System.Windows.Controls;");
			w.WriteLine ("using System.Windows.Media;");
			w.WriteLine ("using ProcessingCli;");
			w.WriteLine ("namespace {0}", namespace_name);
			w.WriteLine ("{");

			w.WriteLine ("public partial class App : ProcessingApplication");
			w.WriteLine ("{");
			w.WriteLine ("public App () : base (() => Run ())");
			w.WriteLine ("{");
			w.WriteLine ("} // end of App.ctor()");
			w.WriteLine ();

			in_global_context = true;

			w.WriteLine ("// placeholder for global variables");
			// Global variables must be declared outside
			// Run() global method so that they can be
			// accessed everywhere.
			foreach (var st in stmts) {
				var b = st as StatementBlock;
				if (b == null)
					continue;
				foreach (var s in b.Statements) {
					var v = s as VariableDeclarationStatement;
					if (v == null)
						continue;
					w.Write ("internal "); // since they can be accessed by user classes.
					w.Write ("static ");
					GenerateType (v.Type);
					w.Write (' ');
					w.Write (v.Name);
					w.Write (';');
					w.WriteLine ();
					v.DeclaredGloballyInOutput = true;
				}
			}
			w.WriteLine ();

			w.WriteLine ("// placeholder for global functions");
			foreach (var f in funcs)
				GenerateGlobalFunction (f);

			w.WriteLine ("public static void Run ()");
			w.WriteLine ("{");
			foreach (var st in stmts)
				GenerateStatement (st);
			foreach (var f in funcs)
				if (f.Internal.Name == "setup")
					w.WriteLine ("setup ();");
			foreach (var f in funcs)
				if (f.Internal.Name == "draw")
					w.WriteLine ("RegisterDraw (() => draw ());");
			w.WriteLine ("OnApplicationSetup (this);");
			w.WriteLine ("}");
			w.WriteLine ("}");

			in_global_context = false;
			foreach (var c in classes)
				GenerateClass (c);
			in_global_context = true;

			w.WriteLine ("}");
		}

		void GenerateClass (ClassDefinition c)
		{
			current_class = c;
			w.Write ("public class ");
			w.Write (c.Name);
			if (c.Interfaces != null) {
				var l = new List<TypeInfo> (c.Interfaces);
				if (c.BaseType != null)
					l.Insert (0, c.BaseType);
				if (l.Count > 0) {
					w.Write (" : ");
					for (int i = 0; i < l.Count; i++) {
						if (i > 0)
							w.Write (", ");
						w.Write (l [i]);
					}
				}
			} else if (c.BaseType != null) {
				w.Write (" : ");
				w.WriteLine (c.BaseType);
			}
			w.WriteLine ();
			w.WriteLine ("{");
			w.WriteLine ("// Members");

			foreach (MemberDefinition m in c.Members) {
				if (m is FieldDefinition)
					GenerateField ((FieldDefinition) m);
				else if (m is FunctionDefinition)
					GenerateFunction ((FunctionDefinition) m);
				else if (m is ConstructorDefinition)
					GenerateConstructor ((ConstructorDefinition) m);
				else
					throw new Exception ("unexpected");
			}

			w.WriteLine ("}");
			current_class = null;
		}

		void GenerateField (FieldDefinition f)
		{
			w.Write ("public ");
			GenerateType (f.Type);
			w.Write (' ');
			for (int i = 0; i < f.Pairs.Count; i++) {
				if (i > 0)
					w.Write (", ");
				var p = f.Pairs [i];
				w.Write (p.Name);
				if (p.Initializer != null) {
					w.Write (" = ");
					GenerateExpression (p.Initializer);
				}
			}
			w.WriteLine (";");
		}

		void GenerateFunction (FunctionDefinition f)
		{
			w.Write ("public ");
			if (in_global_context)
				w.Write ("static ");
			GenerateType (f.Type);
			w.Write (' ');
			GenerateFunctionBase (f);
		}

		void GenerateConstructor (ConstructorDefinition c)
		{
			w.Write ("public ");
			GenerateFunctionBase (c);
		}

		void GenerateFunctionBase (FunctionDefinitionBase f)
		{
			w.Write (f.Name);
			w.Write (" (");
			for (int i = 0; i < f.Arguments.Count; i++) {
				if (i > 0)
					w.Write (", ");
				var a = f.Arguments [i];
				GenerateType (a.Type);
				w.Write (' ');
				w.Write (a.Name);
			}
			w.Write (")");
			w.WriteLine ();
			variable_stack.Add (new VariableScope ());
			GenerateStatement (f.Body);
			variable_stack.RemoveAt (variable_stack.Count - 1);
		}

		string ResolveTypeName (string name)
		{
			switch (name) {
			case "boolean":
				return "bool";
			case "byte":
				return "sbyte";
			case "String":
				return "string";
			case "float":
				return "double";
			case "color":
				return "System.Windows.Media.Color";
			default:
				return name;
			}
		}

		void GenerateType (TypeInfo type)
		{
			string csType = ResolveTypeName (type.Name);
			w.Write (csType);
			for (int i = 0; i < type.ArrayRank; i++)
				w.Write (" []");
		}

		void GenerateGlobalFunction (GlobalFunctionDefinition f)
		{
			GenerateFunction (f.Internal);
		}

		void GenerateStatement (Statement s)
		{
			if (s is CallableExpressionStatement) {
				GenerateExpression (((CallableExpressionStatement) s).Content);
				if (writeSemicolon)
					w.WriteLine (";");
			} else if (s is StatementBlock) {
				var b = (StatementBlock) s;
				if (b.Wrap)
					w.WriteLine ("{");
				foreach (Statement ss in b.Statements)
					GenerateStatement (ss);
				if (b.Wrap)
					w.WriteLine ("}");
			} else if (s is VariableDeclarationStatement) {
				var v = (VariableDeclarationStatement) s;
				current_variable_scope.Add (v);
				// declaration could done earlier in its
				// class (static) context.
				if (v.DeclaredGloballyInOutput) {
					if (v.Initializer == null)
						return; // the global variable has to initializer.
				} else {
					GenerateType (v.Type);
					w.Write (' ');
				}
				w.Write (v.Name);
				if (v.Initializer != null) {
					w.Write (" = ");
					GenerateExpression (v.Initializer);
				}
				if (writeSemicolon)
					w.WriteLine (";");
			} else if (s is IfStatement) {
				var i = (IfStatement) s;
				w.Write ("if (");
				GenerateExpression (i.Condition);
				w.Write (")");
				GenerateStatement (i.TrueBlock);
				if (i.FalseBlock != null) {
					w.Write ("else ");
					GenerateStatement (i.FalseBlock);
				}
			} else if (s is ForStatement) {
				var f = (ForStatement) s;
				// for local initializers
				variable_stack.Add (new VariableScope ());
				w.Write ("for (");
				writeSemicolon = false;
				if (f.Initializers != null) {
					for (int i = 0; i < f.Initializers.Count; i++) {
						if (i > 0)
							w.Write (", ");
						GenerateStatement (f.Initializers [i]);
					}
				}
				w.Write ("; ");
				GenerateExpression (f.Condition);
				w.Write ("; ");
				GenerateStatement (f.Continuer);
				w.WriteLine (")");
				writeSemicolon = true;
				GenerateStatement (f.Body);
				variable_stack.RemoveAt (variable_stack.Count - 1);
			} else if (s is BreakStatement) {
				w.WriteLine ("break;");
			} else if (s is ContinueStatement) {
				w.WriteLine ("continue;");
			} else if (s is ReturnStatement) {
				var r = (ReturnStatement) s;
				w.Write ("return");
				if (r.Value != null) {
					w.Write (' ');
					GenerateExpression (r.Value);
				}
				w.WriteLine (';');
			} else {
				Console.Error.WriteLine (s);
				throw new NotImplementedException ("Not implemented statement: " + s);
			}
		}

		static List<string> all_funcs;
		static List<string> all_fields;
		static List<string> all_consts;

		static CodeGenerator ()
		{
			all_funcs = new List<string> ();
			all_consts = new List<string> ();
			all_fields = new List<string> ();
			string [] lines = new StreamReader (typeof (CodeGenerator).Assembly.GetManifestResourceStream ("apilist.txt"))
				.ReadToEnd ().Split (new char [] {'\n'});
			foreach (string line in lines) {
				if (line.StartsWith ("Method "))
					all_funcs.Add (line.Substring (7));
				else if (line.StartsWith ("Const "))
					all_consts.Add (line.Substring (6));
				else if (line.StartsWith ("Field "))
					all_fields.Add (line.Substring (6));
			}
		}

		string ResolveFunction (string name, bool isGlobal)
		{
			if (isGlobal) {
				foreach (GlobalFunctionDefinition g in funcs)
					if (g.Internal.Name == name)
						return name;
				foreach (var n in all_funcs)
					if (n == name)
						return "ProcessingApplication.Current.@" + name;
			}
			return name;
		}

		string ResolveVariableIdentifier (string name)
		{
			// resolve local variable -> class field -> global variable.
			for (int i = variable_stack.Count - 1; i >= 0; i--)
				foreach (var s in variable_stack [i])
					if (s.Name == name)
						return name; // local variable.

			if (current_class != null)
				foreach (var f in current_class.Members.FindAll<FieldDefinition> ())
					foreach (var p in f.Pairs)
						if (p.Name == name)
							return name; // class member

			foreach (var s in global_variables)
				if (s.Name == name)
					// global variable. Note that we need the container
					// type name since the reference might be from another class.
					return "App." + name;

			// Alias. Since some fields seem to have the same names
			// as some functions, they have to be explicitly replaced
			// with different ones.
			switch (name) {
			case "frameRate":
				return "ProcessingApplication.Current.frameRateResult";
			}
			foreach (var m in all_consts)
				if (m == name)
					return "ProcessingApplication." + name;
			foreach (var m in all_fields)
				if (m == name)
						return "ProcessingApplication.Current." + name;

			return name;
		}

		string ResolveFieldMemberName (Expression type, string fieldName)
		{
			// FIXME: This AST currently has no way to infer
			// TypeInfo from an Expression, so it handles every
			// "length" property as for an array and replaces it
			// with "Length" in CLI.
			if (true) {
				switch (fieldName) {
				case "length":
					return "Length";
				}
			}
			return fieldName;
		}

		void GenerateExpression (Expression x)
		{
			if (x == null)
				throw new ArgumentNullException ("x");

			if (x is FunctionCallExpression) {
				var f = (FunctionCallExpression) x;
				if (f.Target != null) {
					GenerateExpression (f.Target);
					w.Write (".");
					w.Write (ResolveFunction (f.Name, false));
				}
				else
					w.Write (ResolveFunction (f.Name, true));
				w.Write (" (");
				for (int i = 0; i < f.Arguments.Count; i++) {
					if (i > 0)
						w.Write (", ");
					GenerateExpression (f.Arguments [i]);
				}
				w.Write (")");
			} else if (x is ConstantExpression) {
				var c = (ConstantExpression) x;
				if (c.Value == null)
					w.Write ("null");
				else if (c.Value is bool)
					w.Write (((bool) c.Value) ? "true" : "false");
				else if (c.Value is char) {
					w.Write ("'");
					w.Write (c.Value);
					w.Write ("'");
				} else if (c.Value is string) {
					w.Write ("@\"");
					w.Write (c.Value.ToString ().Replace ("\"", "\"\"").Replace ("\\", "\\\\"));
					w.Write ('"');
				}
				else
					w.Write (c.Value);
			} else if (x is ColorConstantExpression) {
				var c = (ColorConstantExpression) x;
				w.Write ("ProcessingApplication.Current.color (\"#");
				w.Write (c.Value);
				w.Write ("\")");
			} else if (x is NewObjectExpression) {
				var n = (NewObjectExpression) x;
				w.Write ("new ");
				GenerateType (n.Type);
				w.Write (" (");
				for (int i = 0; i < n.Arguments.Count; i++) {
					if (i > 0)
						w.Write (", ");
					GenerateExpression (n.Arguments [i]);
				}
				w.Write (")");
			} else if (x is NewArrayExpression) {
				var n = (NewArrayExpression) x;
				if (n.Sizes.Count == 1) {
					w.Write ("new ");
					GenerateType (n.Type);
					w.Write (" [");
					if (n.Sizes [0] != null)
						GenerateExpression (n.Sizes [0]);
					w.Write ("]");
				} else {
					w.Write ('(');
					GenerateType (n.Type);
					for (int i = 0; i < n.Sizes.Count; i++)
						w.Write (" []");
					w.Write (") ");
					w.Write ("ProcessingUtility.CreateMultiDimentionArray (typeof (");
					GenerateType (n.Type);
					w.Write ("), 0, ");
					if (n.Sizes [0] != null)
						GenerateExpression (n.Sizes [0]);
					for (int i = 1; i < n.Sizes.Count; i++) {
						w.Write (", ");
						if (n.Sizes [i] != null)
							GenerateExpression (n.Sizes [i]);
						else
							w.Write ("-1");
					}
					w.Write (")");
				}
			} else if (x is ArrayInitializerExpression) {
				var n = (ArrayInitializerExpression) x;
				w.Write ("{");
				if (n.Elements.Count > 0)
					GenerateExpression (n.Elements [0]);
				for (int i = 1; i < n.Elements.Count; i++) {
					w.Write (", ");
					GenerateExpression (n.Elements [i]);
				}
				w.Write ("}");
			} else if (x is IdentifierReferenceExpression) {
				var v = (IdentifierReferenceExpression) x;
				w.Write (ResolveVariableIdentifier (v.Name));
			} else if (x is ComparisonExpression) {
				var c = (ComparisonExpression) x;
				GenerateExpression (c.Left);
				switch (c.Kind) {
				case ComparisonKind.LessThan:
					w.Write (" < "); break;
				case ComparisonKind.LessThanEqual:
					w.Write (" <= "); break;
				case ComparisonKind.GreaterThan:
					w.Write (" > "); break;
				case ComparisonKind.GreaterThanEqual:
					w.Write (" >= "); break;
				case ComparisonKind.Equal:
					w.Write (" == "); break;
				case ComparisonKind.NotEqual:
					w.Write (" != "); break;
				}
				GenerateExpression (c.Right);
			} else if (x is ArithmeticExpression) {
				var a = (ArithmeticExpression) x;
				GenerateExpression (a.Left);
				switch (a.Kind) {
				case ArithmeticKind.Add:
					w.Write (" + "); break;
				case ArithmeticKind.Subtract:
					w.Write (" - "); break;
				case ArithmeticKind.Multiply:
					w.Write (" * "); break;
				case ArithmeticKind.Divide:
					w.Write (" / "); break;
				case ArithmeticKind.Modulo:
					w.Write (" % "); break;
				case ArithmeticKind.BitwiseAnd:
					w.Write (" & "); break;
				case ArithmeticKind.BitwiseOr:
					w.Write (" | "); break;
				case ArithmeticKind.ShiftLeft:
					w.Write (" << "); break;
				case ArithmeticKind.ShiftRight:
					w.Write (" >> "); break;
				}
				GenerateExpression (a.Right);
			} else if (x is LogicalOperationExpression) {
				var o = (LogicalOperationExpression) x;
				GenerateExpression (o.Left);
				switch (o.Kind) {
				case LogicalOperationKind.AndAlso:
					w.Write (" && "); break;
				case LogicalOperationKind.OrElse:
					w.Write (" || "); break;
				}
				GenerateExpression (o.Right);
			} else if (x is LogicalNotExpression) {
				var n = (LogicalNotExpression) x;
				w.Write ("!(");
				GenerateExpression (n.Value);
				w.Write (")");
			} else if (x is ArrayAccessExpression) {
				var a = (ArrayAccessExpression) x;
				GenerateExpression (a.Array);
				w.Write (" [");
				GenerateExpression (a.Index);
				w.Write ("]");
			} else if (x is CastExpression) {
				var c = (CastExpression) x;
				w.Write ("(");
				GenerateType (c.Type);
				w.Write (") ");
				GenerateExpression (c.Value);
			} else if (x is AssignmentExpression) {
				var a = (AssignmentExpression) x;
				GenerateExpression (a.Left);
				w.Write (" = ");
				GenerateExpression (a.Right);
			} else if (x is FieldAccessExpression) {
				var a = (FieldAccessExpression) x;
				GenerateExpression (a.Target);
				w.Write ('.');
				w.Write (ResolveFieldMemberName (a.Target, a.MemberName));
			} else if (x is ParenthesizedExpression) {
				var p = (ParenthesizedExpression) x;
				w.Write ('(');
				GenerateExpression (p.Body);
				w.Write (')');
			} else {
				Console.Error.WriteLine (x);
				throw new NotImplementedException ("Not implemented expression: " + x);
			}
		}
	}
}
