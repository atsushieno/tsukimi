using System;
using System.Collections.Generic;
using System.IO;

using ProcessingDlr.Ast;

namespace ProcessingDlr
{
	public class CodeGenerator
	{
		AstRoot root;
		TextWriter w;

		public CodeGenerator (AstRoot root, TextWriter writer)
		{
			this.root = root;
			this.w = writer;
		}

		public void Generate ()
		{
			w.WriteLine ("using System;");
			w.WriteLine ("using System.Windows;");
			w.WriteLine ("using System.Windows.Controls;");
			w.WriteLine ("using ProcessingDlr;");

			var classes = new List<ClassDefinition> ();
			var funcs = new List<GlobalFunctionDefinition> ();
			var stmts = new List<Statement> ();
			foreach (ITopLevelContent c in root.Items) {
				if (c is ClassDefinition)
					classes.Add ((ClassDefinition) c);
				else if (c is GlobalFunctionDefinition)
					funcs.Add ((GlobalFunctionDefinition) c);
				else if (c is Statement)
					stmts.Add ((Statement) c);
			}

			foreach (var c in classes)
				GenerateClass (c);
			w.WriteLine ("public class Global");
			w.WriteLine ("{");
			w.WriteLine ("// placeholder for global functions");
			foreach (var f in funcs)
				GenerateGlobalFunction (f);
			w.WriteLine ("}");
			foreach (var st in stmts)
				GenerateStatement (st);
		}

		void GenerateClass (ClassDefinition c)
		{
			w.Write ("public class ");
			w.Write (c.Name);
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
				if (p.Initializer != null)
					w.Write (" = ");
					GenerateExpression (p.Initializer);
			}
			w.WriteLine (";");
		}

		void GenerateFunction (FunctionDefinition f)
		{
			w.Write ("public ");
			GenerateType (f.Type);
			w.Write (' ');
			GenerateFunctionBase (f);
		}

		void GenerateConstructor (ConstructorDefinition c)
		{
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
			GenerateStatement (f.Body);
		}

		void GenerateType (TypeInfo type)
		{
			w.Write (type.Name);
			for (int i = 0; i < type.ArrayRank; i++)
				w.Write (" []");
		}

		void GenerateGlobalFunction (GlobalFunctionDefinition f)
		{
			GenerateFunction (f.Internal);
		}

		bool writeSemicolon = true;

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
				GenerateType (v.Type);
				w.Write (' ');
				w.Write (v.Name);
				if (v.Initializer != null) {
					w.Write (" = ");
					GenerateExpression (v.Initializer);
				}
				if (writeSemicolon)
					w.WriteLine (";");
			} else if (s is ForStatement) {
				var f = (ForStatement) s;
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
			} else {
				Console.Error.WriteLine (s);
				throw new NotImplementedException ();
			}
		}

		void GenerateExpression (Expression x)
		{
			if (x is FunctionCallExpression) {
				var f = (FunctionCallExpression) x;
				if (f.Target != null) {
					GenerateExpression (f.Target);
					w.Write (".");
				}
				w.Write (f.Name);
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
				else
					w.Write (c.Value);
			} else if (x is NewArrayExpression) {
				var n = (NewArrayExpression) x;
				w.Write ("new ");
				GenerateType (n.Type);
				w.Write (" [");
				GenerateExpression (n.Size);
				w.Write ("]");
			} else if (x is VariableReferenceExpression) {
				var v = (VariableReferenceExpression) x;
				w.Write (v.Name);
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
			} else {
				Console.Error.WriteLine (x);
				throw new NotImplementedException ();
			}
		}
	}
}
