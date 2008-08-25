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
			throw new NotImplementedException ();
		}

		public static Statement IfThen (Expression cond, StatementBlock trueBlock)
		{
			throw new NotImplementedException ();
		}

		public static Statement IfThenElse (Expression cond, StatementBlock trueBlock, StatementBlock falseBlock)
		{
			throw new NotImplementedException ();
		}

		public static Statement For (List<Expression> init,
			Expression cond,
			List<Expression> cont,
			StatementBlock body)
		{
			throw new NotImplementedException ();
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
			throw new NotImplementedException ();
		}

		public static Statement Assign (Expression left, Expression right)
		{
			throw new NotImplementedException ();
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

	public abstract class Expression
	{

		public static Expression Null ()
		{
			throw new NotImplementedException ();
		}

		public static Expression True ()
		{
			throw new NotImplementedException ();
		}

		public static Expression False ()
		{
			throw new NotImplementedException ();
		}

		public static Expression Not (Expression operand)
		{
			throw new NotImplementedException ();
		}

		public static Expression Constant (object value)
		{
			throw new NotImplementedException ();
		}

		public static Expression New (string typeName, List<Expression> args)
		{
			throw new NotImplementedException ();
		}

		public static Expression NewArrayBounds (string typeName, Expression size)
		{
			throw new NotImplementedException ();
		}

		public static Expression Condition (Expression cond, Expression trueExpr, Expression falseExpr)
		{
			throw new NotImplementedException ();
		}

		public static Expression PropertyOrField (Expression obj, string memberName)
		{
			throw new NotImplementedException ();
		}

		public static Expression Call (Expression obj, string funcName, List<Expression> args)
		{
			throw new NotImplementedException ();
		}

		public static Expression ArrayIndex (Expression array, Expression index)
		{
			throw new NotImplementedException ();
		}

		public static Expression Equal (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression NotEqual (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression LeftShift (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression RightShift (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression And (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression Or (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression AndAlso (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression OrElse (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression GreaterThan (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression GreaterThanOrEqual (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression LessThan (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression LessThanOrEqual (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression AddChecked (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression SubtractChecked (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression MultiplyChecked (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression DivideChecked (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}

		public static Expression ModuloChecked (Expression left, Expression right)
		{
			throw new NotImplementedException ();
		}
	}

	public abstract class CallableExpression : Expression
	{
	}

	public abstract class OperationExpression : Expression
	{
	}

	public class VariableReferenceExpression : Expression
	{
	}

	public class LiteralExpression : Expression
	{
	}

	public class FieldAccessExpression : Expression
	{
	}

	public class ConstantExpression : Expression
	{
	}
}
