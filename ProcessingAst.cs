using System;
using System.Collections.Generic;

namespace ProcessingDlr.Ast
{
	public interface ITopLevelContent
	{
	}

	public class ClassDefinition : ITopLevelContent
	{
		string name, base_type;
		List<string> interfaces;
		List<MemberDefinition> members;

		public ClassDefinition (string name, string baseType, List<string> interfaces, List<MemberDefinition> members)
		{
			this.name = name;
			this.base_type = baseType;
			this.interfaces = interfaces;
			this.members = members;
		}
	}

	public abstract class MemberDefinition
	{
	}

	public class FunctionDefinition : MemberDefinition
	{
		public FunctionDefinition (string typeName, FunctionBase funcBase)
		{
		}
	}

	public class ConstructorDefinition
	{
		public ConstructorDefinition (FunctionBase funcBase)
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
		public string Type;
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
