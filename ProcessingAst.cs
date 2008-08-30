using System;
using System.Collections.Generic;

namespace ProcessingDlr.Ast
{
	public class AstRoot
	{
		List<ITopLevelContent> items = new List<ITopLevelContent> ();

		public AstRoot ()
		{
		}

		public IList<ITopLevelContent> Items {
			get { return items; }
		}
	}

	public interface ITopLevelContent
	{
	}

	public class TypeInfo
	{
		public TypeInfo (string name, int arrayRank)
		{
			Name = name;
			ArrayRank = arrayRank;
		}

		public string Name { get; set; }
		public int ArrayRank { get; set; }
	}

	public class ClassDefinition : ITopLevelContent
	{
		public ClassDefinition (string name, TypeInfo baseType, List<TypeInfo> interfaces, MemberList members)
		{
			Name = name;
			BaseType = baseType;
			Interfaces = interfaces;
			Members = members;
		}

		public string Name { get; set; }
		public TypeInfo BaseType { get; set; }
		public List<TypeInfo> Interfaces { get; private set; }
		public MemberList Members { get; private set; }
	}

	public class MemberList : List<MemberDefinition>
	{
		public IEnumerable<T> FindAll<T> () where T : MemberDefinition
		{
			foreach (var m in this)
				if (m is T)
					yield return (T) m;
		}
	}

	public class GlobalFunctionDefinition : ITopLevelContent
	{
		public GlobalFunctionDefinition (FunctionDefinition function)
		{
			Internal = function;
		}

		internal FunctionDefinition Internal { get; set; }
	}

	public abstract class MemberDefinition
	{
	}

	public class FieldDefinition : MemberDefinition
	{
		public FieldDefinition (VariableDeclarations v)
		{
			Decls = v;
		}

		internal VariableDeclarations Decls { get; set; }

		public TypeInfo Type {
			get { return Decls.Type; }
			set { Decls.Type = value; }
		}

		public List<VariableDeclarationPair> Pairs {
			get { return Decls.Pairs; }
		}
	}

	public abstract class FunctionDefinitionBase : MemberDefinition
	{
		protected FunctionDefinitionBase (FunctionBase funcBase)
		{
			Name = funcBase.Name;
			Arguments = funcBase.Arguments;
			Body = funcBase.Body;
		}

		public string Name { get; set; }
		public List<FunctionArgument> Arguments { get; private set; }
		public StatementBlock Body { get; set; }
	}

	public class FunctionDefinition : FunctionDefinitionBase
	{
		public FunctionDefinition (TypeInfo type, FunctionBase funcBase)
			: base (funcBase)
		{
			Type = type;
		}

		public TypeInfo Type { get; set; }
	}

	public class ConstructorDefinition : FunctionDefinitionBase
	{
		public ConstructorDefinition (FunctionBase funcBase)
			: base (funcBase)
		{
		}
	}

	public class FunctionBase
	{
		public FunctionBase (string name, List<FunctionArgument> args, StatementBlock body)
		{
			Name = name;
			Arguments = args;
			Body = body;
		}

		public string Name;
		public List<FunctionArgument> Arguments;
		public StatementBlock Body;
	}

	public class FunctionArgument
	{
		public TypeInfo Type;
		public string Name;
	}

	public class FunctionCall
	{
		public FunctionCall (string name, List<Expression> args)
		{
			Name = name;
			Arguments = args;
		}

		public string Name;
		public List<Expression> Arguments;
	}

	public class VariableDeclarations
	{
		public TypeInfo Type;
		public List<VariableDeclarationPair> Pairs;
	}

	public class VariableDeclarationPair
	{
		public string Name;
		public Expression Initializer;
	}

	public class SwitchCase
	{
		public SwitchCase (ConstantExpression label, List<Statement> block)
		{
		}
	}

	public class DefaultCase
	{
		public DefaultCase (List<Statement> block)
		{
		}
	}
}
