using System;
using System.Collections.Generic;

namespace ProcessingCli.Ast
{
	public abstract class Statement : ITopLevelContent
	{
		public static StatementBlock Block (bool wrap, List<Statement> statements)
		{
			return Block (wrap, statements.ToArray ());
		}

		public static StatementBlock Block (bool wrap, params Statement [] statements)
		{
			return new StatementBlock (wrap, statements);
		}

		public static Statement IfThen (Expression cond, StatementBlock trueBlock)
		{
			return new IfStatement (cond, trueBlock, null);
		}

		// It does not use StatementBlock to handle else-if statements as IfStatement (i.e. IfStatement(trueBlock, IfStatement(elseIfTrue, finalElseBlock)) etc.)
		public static Statement IfThenElse (Expression cond, Statement trueBlock, Statement falseBlock)
		{
			return new IfStatement (cond, trueBlock, falseBlock);
		}

		public static Statement For (List<Statement> init,
			Expression cond,
			Statement cont,
			StatementBlock body)
		{
			return new ForStatement (init, cond, cont, body);
		}

		public static Statement Switch (Expression cond, SwitchCase [] cases)
		{
			throw new NotImplementedException ();
		}

		public static Statement Break ()
		{
			return new BreakStatement ();
		}

		public static Statement Continue ()
		{
			return new ContinueStatement ();
		}

		public static Statement Loop (Expression cond, StatementBlock block)
		{
			throw new NotImplementedException ();
		}

		public static Statement Return (Expression value)
		{
			return new ReturnStatement (value);
		}

		public static VariableDeclarationStatement DeclareVariable (TypeInfo type, string name, Expression initializer)
		{
			return new VariableDeclarationStatement (type, name, initializer);
		}

		public static Statement CallExpression (Expression exp)
		{
			return new CallableExpressionStatement (exp);
		}
	}

	public class VariableDeclarationStatement : Statement
	{
		public VariableDeclarationStatement (TypeInfo type, string name, Expression initializer)
		{
			Type = type;
			Name = name;
			Initializer = initializer;
		}

		public TypeInfo Type { get; set; }
		public string Name { get; set; }
		public Expression Initializer { get; set; }
		// it is used by code generator to indicate whether
		// the variable is already declared globally.
		public bool DeclaredGloballyInOutput { get; set; }
	}

	public class CallableExpressionStatement : Statement
	{
		public CallableExpressionStatement (Expression exp)
		{
			Content = exp;
		}

		public Expression Content { get; set; }
	}

	public class StatementBlock : Statement
	{
		public StatementBlock (bool wrap, List<Statement> statements)
			: this (wrap, statements.ToArray ())
		{
		}

		public StatementBlock (bool wrap, params Statement [] statements)
		{
			Wrap = wrap;
			Statements = statements;
		}

		public bool Wrap { get; set; }
		public Statement [] Statements { get; private set; }
	}

	public class ContinueStatement : Statement
	{
	}

	public class BreakStatement : Statement
	{
	}

	public class IfStatement : Statement
	{
		public IfStatement (Expression cond, Statement trueBlock, Statement falseBlock)
		{
			Condition = cond;
			TrueBlock = trueBlock;
			FalseBlock = falseBlock;
		}

		public Expression Condition { get; set; }
		public Statement TrueBlock { get; set; }
		public Statement FalseBlock { get; set; }
	}

	public class ForStatement : Statement
	{
		public ForStatement (List<Statement> init,
			Expression cond,
			Statement cont,
			StatementBlock body)
		{
			Initializers = init;
			Condition = cond;
			Continuer = cont;
			Body = body;
		}

		public List<Statement> Initializers { get; private set; }
		public Expression Condition { get; set; }
		public Statement Continuer { get; set; }
		public StatementBlock Body { get; set; }
	}

	public class ReturnStatement : Statement
	{
		public ReturnStatement (Expression value)
		{
			Value = value;
		}

		public Expression Value { get; set; }
	}

	public abstract class Expression
	{

		public static Expression Null ()
		{
			return new ConstantExpression (null);
		}

		public static Expression True ()
		{
			return new ConstantExpression (true);
		}

		public static Expression False ()
		{
			return new ConstantExpression (false);
		}

		public static Expression Constant (object value)
		{
			return new ConstantExpression (value);
		}

		public static Expression Not (Expression operand)
		{
			if (operand == null)
				throw new ArgumentNullException ("operand");
			return new LogicalNotExpression (operand);
		}

		public static Expression IdentifierReference (string name)
		{
			return new IdentifierReferenceExpression (name);
		}

		public static Expression New (TypeInfo type, List<Expression> args)
		{
			return new NewObjectExpression (type, args);
		}

		public static Expression NewArrayBounds (TypeInfo type, List<Expression> sizes)
		{
			return new NewArrayExpression (type, sizes);
		}

		public static Expression Cast (TypeInfo type, Expression value)
		{
			return new CastExpression (type, value);
		}

		public static Expression Condition (Expression cond, Expression trueExpr, Expression falseExpr)
		{
			throw new NotImplementedException ();
		}

		public static Expression PropertyOrField (Expression obj, string memberName)
		{
			return new FieldAccessExpression (obj, memberName);
		}

		public static Expression Call (Expression obj, string name, List<Expression> args)
		{
			// obj could be null (global function)
			return new FunctionCallExpression (obj, name, args);
		}

		public static Expression Assign (Expression left, Expression right)
		{
			return new AssignmentExpression (left, right);
		}

		public static Expression ArrayAccess (Expression array, Expression index)
		{
			return new ArrayAccessExpression (array, index);
		}

		public static Expression LeftShift (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression RightShift (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression AndAlso (Expression left, Expression right)
		{
			return new LogicalOperationExpression (left, right, LogicalOperationKind.AndAlso);
		}

		public static Expression OrElse (Expression left, Expression right)
		{
			return new LogicalOperationExpression (left, right, LogicalOperationKind.OrElse);
		}

		public static Expression Equal (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.Equal);
		}

		public static Expression NotEqual (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.NotEqual);
		}

		public static Expression GreaterThan (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.GreaterThan);
		}

		public static Expression GreaterThanOrEqual (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.GreaterThanEqual);
		}

		public static Expression LessThan (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.LessThan);
		}

		public static Expression LessThanOrEqual (Expression left, Expression right)
		{
			return new ComparisonExpression (left, right, ComparisonKind.LessThanEqual);
		}

		public static Expression AddChecked (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.Add);
		}

		public static Expression SubtractChecked (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.Subtract);
		}

		public static Expression MultiplyChecked (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.Multiply);
		}

		public static Expression DivideChecked (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.Divide);
		}

		public static Expression ModuloChecked (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.Modulo);
		}

		public static Expression And (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.BitwiseAnd);
		}

		public static Expression Or (Expression left, Expression right)
		{
			return new ArithmeticExpression (left, right, ArithmeticKind.BitwiseOr);
		}
	}

	public class AssignmentExpression : Expression
	{
		public AssignmentExpression (Expression left, Expression right)
		{
			Left = left;
			Right = right;
		}

		public Expression Left { get; set; }
		public Expression Right { get; set; }
	}

	public class CastExpression : Expression
	{
		public CastExpression (TypeInfo type, Expression value)
		{
			Type = type;
			Value = value;
		}

		public TypeInfo Type { get; set; }
		public Expression Value { get; set; }
	}

	public abstract class CallableExpression : Expression
	{
	}

	public abstract class OperationExpression : Expression
	{
	}

	public enum ComparisonKind
	{
		LessThan,
		LessThanEqual,
		GreaterThan,
		GreaterThanEqual,
		Equal,
		NotEqual
	}

	public class ComparisonExpression : OperationExpression
	{
		public ComparisonExpression (Expression left, Expression right, ComparisonKind kind)
		{
			Left = left;
			Right = right;
			Kind = kind;
		}

		public Expression Left { get; set; }
		public Expression Right { get; set; }
		public ComparisonKind Kind { get; set; }
	}

	public enum LogicalOperationKind
	{
		AndAlso,
		OrElse,
	}

	public class LogicalNotExpression : OperationExpression
	{
		public LogicalNotExpression (Expression value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			Value = value;
		}

		public Expression Value { get; set; }
	}

	public class LogicalOperationExpression : OperationExpression
	{
		public LogicalOperationExpression (Expression left, Expression right, LogicalOperationKind kind)
		{
			Left = left;
			Right = right;
			Kind = kind;
		}

		public Expression Left { get; set; }
		public Expression Right { get; set; }
		public LogicalOperationKind Kind { get; set; }
	}

	public enum ArithmeticKind
	{
		Add,
		Subtract,
		Multiply,
		Divide,
		Modulo,
		BitwiseAnd,
		BitwiseOr,
		ShiftLeft,
		ShiftRight,
	}

	public class ArithmeticExpression : OperationExpression
	{
		public ArithmeticExpression (Expression left, Expression right, ArithmeticKind kind)
		{
			Left = left;
			Right = right;
			Kind = kind;
		}

		public Expression Left { get; set; }
		public Expression Right { get; set; }
		public ArithmeticKind Kind { get; set; }
	}

	public class IdentifierReferenceExpression : Expression
	{
		public IdentifierReferenceExpression (string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}

	public class LiteralExpression : Expression
	{
	}

	public class FieldAccessExpression : Expression
	{
		public FieldAccessExpression (Expression target, string memberName)
		{
			Target = target;
			MemberName = memberName;
		}

		public Expression Target { get; set; }
		public string MemberName { get; set; }
	}

	public class ConstantExpression : Expression
	{
		public ConstantExpression (object value)
		{
			this.Value = value;
		}

		public object Value { get; set; }
	}

	public class FunctionCallExpression : Expression
	{
		public FunctionCallExpression (Expression obj, string name, List<Expression> args)
		{
			Target = obj;
			Name = name;
			Arguments = args;
		}

		public Expression Target { get; set; }
		public string Name { get; set; }
		public List<Expression> Arguments { get; private set; }
	}

	public class NewObjectExpression : Expression
	{
		public NewObjectExpression (TypeInfo type, List<Expression> args)
		{
			Type = type;
			Arguments = args;
		}

		public TypeInfo Type { get; set; }
		public List<Expression> Arguments { get; set; }
	}

	public class NewArrayExpression : Expression
	{
		public NewArrayExpression (TypeInfo type, List<Expression> sizes)
		{
			Type = type;
			Sizes = sizes;
		}

		public TypeInfo Type { get; set; }
		public List<Expression> Sizes { get; set; }
	}

	public class ArrayInitializerExpression : Expression
	{
		public ArrayInitializerExpression (List<Expression> elements)
		{
			Elements = elements;
		}

		public IList<Expression> Elements { get; private set; }
	}

	public class ArrayAccessExpression : Expression
	{
		public ArrayAccessExpression (Expression array, Expression index)
		{
			Array = array;
			Index = index;
		}

		public Expression Array { get; set; }
		public Expression Index { get; set; }
	}
}
