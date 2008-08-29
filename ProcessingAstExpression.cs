using System;
using System.Collections.Generic;

namespace ProcessingDlr.Ast
{
	public abstract class Statement : ITopLevelContent
	{
		public static StatementBlock Block (List<Statement> statements)
		{
			return Block (statements.ToArray ());
		}

		public static StatementBlock Block (params Statement [] statements)
		{
			return new StatementBlock (statements);
		}

		public static Statement IfThen (Expression cond, StatementBlock trueBlock)
		{
			throw new NotImplementedException ();
		}

		public static Statement IfThenElse (Expression cond, StatementBlock trueBlock, StatementBlock falseBlock)
		{
			throw new NotImplementedException ();
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
			throw new NotImplementedException ();
		}

		public static Statement Loop (Expression cond, StatementBlock block)
		{
			throw new NotImplementedException ();
		}

		public static Statement Return (Expression value)
		{
			throw new NotImplementedException ();
		}

		public static Statement DeclareVariable (string typeName, string name, Expression initializer)
		{
			return new VariableDeclarationStatement (typeName, name, initializer);
		}

		public static Statement CallExpression (Expression exp)
		{
			return new CallableExpressionStatement (exp);
		}
	}

	public class VariableDeclarationStatement : Statement
	{
		public VariableDeclarationStatement (string typeName, string name, Expression initializer)
		{
		}
	}

	public class CallableExpressionStatement : Statement
	{
		public CallableExpressionStatement (Expression exp)
		{
		}
	}

	public class StatementBlock : Statement
	{
		public StatementBlock (List<Statement> statements)
			: this (statements.ToArray ())
		{
		}

		public StatementBlock (params Statement [] statements)
		{
		}
	}

	public class BreakStatement : Statement
	{
	}

	public class ForStatement : Statement
	{
		public ForStatement (List<Statement> init,
			Expression cond,
			Statement cont,
			StatementBlock body)
		{
		}
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
			throw new NotImplementedException ();
		}

		public static Expression VariableRef (string name)
		{
			return new VariableReferenceExpression (name);
		}

		public static Expression New (string typeName, List<Expression> args)
		{
			return new NewObjectExpression (typeName, args);
		}

		public static Expression NewArrayBounds (string typeName, Expression size)
		{
			return new NewArrayExpression (typeName, size);
		}

		public static Expression Cast (string typeName, Expression value)
		{
			return new CastExpression (typeName, value);
		}

		public static Expression Condition (Expression cond, Expression trueExpr, Expression falseExpr)
		{
			throw new NotImplementedException ();
		}

		public static Expression PropertyOrField (Expression obj, string memberName)
		{
			throw new NotImplementedException ();
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
		}
	}

	public class CastExpression : Expression
	{
		public CastExpression (string typeName, Expression value)
		{
		}
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
		}
	}

	public enum LogicalOperationKind
	{
		AndAlso,
		OrElse,
	}

	public class LogicalOperationExpression : OperationExpression
	{
		public LogicalOperationExpression (Expression left, Expression right, LogicalOperationKind kind)
		{
		}
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
		}
	}

	public class VariableReferenceExpression : Expression
	{
		public VariableReferenceExpression (string name)
		{
		}
	}

	public class LiteralExpression : Expression
	{
	}

	public class FieldAccessExpression : Expression
	{
	}

	public class ConstantExpression : Expression
	{
		public ConstantExpression (object value)
		{
		}
	}

	public class FunctionCallExpression : Expression
	{
		public FunctionCallExpression (Expression obj, string name, List<Expression> args)
		{
		}
	}

	public class NewObjectExpression : Expression
	{
		public NewObjectExpression (string typeName, List<Expression> args)
		{
		}
	}

	public class NewArrayExpression : Expression
	{
		public NewArrayExpression (string typeName, Expression size)
		{
		}
	}

	public class ArrayAccessExpression : Expression
	{
		public ArrayAccessExpression (Expression array, Expression index)
		{
		}
	}
}
