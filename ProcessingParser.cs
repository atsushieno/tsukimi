// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 3 "ProcessingParser.jay"
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ProcessingDlr
{
	public class PdlrClass
	{
		string name, base_type;
		List<string> interfaces;
		List<PdlrMember> members;

		public PdlrClass (string name, string baseType, List<string> interfaces, List<PdlrMember> members)
		{
			this.name = name;
			this.base_type = baseType;
			this.interfaces = interfaces;
			this.members = members;
		}
	}

	public class PdlrFunction
	{
		public PdlrFunction (string typeName, PdlrFunctionBase funcBase)
		{
		}
	}

	public class PdlrConstructor
	{
		public PdlrConstructor (PdlrFunctionBase funcBase)
		{
		}
	}

	public class PdlrFunctionBase
	{
		public PdlrFunctionBase (string name, List<FunctionArgument> args, List<Expression> body)
		{
			Name = name;
			Arguments = args;
			Body = body;
		}

		public string Name;
		public List<FunctionArgument> Arguments;
		public List<Expression> Body;
	}

	public class VariableDeclarationPair
	{
		public string Name;
		public Expression Initializer;
	}

	public abstract class PdlrMember
	{
	}

	public class PdlrParser
	{

#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = System.Console.Out;

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
  internal yydebug.yyDebug debug;

  protected static  int yyFinal = 1;
 // Put this array into a separate class so it is only initialized if debugging is actually used
 // Use MarshalByRefObject to disable inlining
 class YYRules : MarshalByRefObject {
  public static  string [] yyRule = {
    "$accept : top_level_contents",
    "identifier : IDENTIFIER",
    "top_level_contents :",
    "top_level_contents : top_level_contents top_level_content",
    "top_level_content : statement",
    "top_level_content : class_declaration",
    "top_level_content : global_function_definition",
    "global_function_definition : function_definition",
    "class_declaration : CLASS identifier opt_derivation_indication opt_implementation_indications OPEN_CURLY class_member_definitions CLOSE_CURLY",
    "opt_derivation_indication :",
    "opt_derivation_indication : derivation_indication",
    "opt_implementation_indications :",
    "opt_implementation_indications : implementation_indications",
    "derivation_indication : EXTENDS identifier",
    "implementation_indications : IMPLEMENTS identifier_list_comma",
    "identifier_list_comma : identifier",
    "identifier_list_comma : identifier COMMA identifier_list_comma",
    "class_member_definitions :",
    "class_member_definitions : class_member_definition class_member_definitions",
    "class_member_definition : field_definition",
    "class_member_definition : constructor_definition",
    "class_member_definition : function_definition",
    "field_definition : variable_declaration_statement SEMICOLON",
    "constructor_definition : function_definition_base",
    "function_definition : type_name function_definition_base",
    "function_definition_base : identifier OPEN_PAREN argument_definitions CLOSE_PAREN statement_block",
    "argument_definitions :",
    "argument_definitions : argument_definition COMMA argument_definitions",
    "argument_definition : type_name identifier",
    "statement_block : OPEN_CURLY statements CLOSE_CURLY",
    "statements :",
    "statements : statement statements",
    "statement : statement_with_semicolon SEMICOLON",
    "statement : statement_without_semicolon",
    "statement_with_semicolon : return_statement",
    "statement_with_semicolon : abstract_assignment_statement",
    "statement_with_semicolon : break_statement",
    "statement_with_semicolon : increment_statement",
    "statement_with_semicolon : decrement_statement",
    "statement_with_semicolon : variable_declaration_statement",
    "statement_with_semicolon : call_super_statement",
    "statement_with_semicolon : callable_expression",
    "statement_without_semicolon : if_statement",
    "statement_without_semicolon : for_statement",
    "statement_without_semicolon : while_statement",
    "statement_without_semicolon : switch_statement",
    "abstract_assignment_statement : assignment_statement",
    "abstract_assignment_statement : add_assignment_statement",
    "abstract_assignment_statement : subtract_assignment_statement",
    "add_assignment_statement : variable_or_member_reference PLUS_EQUAL expression",
    "subtract_assignment_statement : variable_or_member_reference MINUS_EQUAL expression",
    "increment_statement : variable_or_member_reference PLUS_PLUS",
    "decrement_statement : variable_or_member_reference MINUS_MINUS",
    "call_super_statement : SUPER OPEN_PAREN function_args CLOSE_PAREN",
    "return_statement : RETURN expression",
    "assignment_statement : variable_or_member_reference EQUAL expression",
    "while_statement : WHILE OPEN_PAREN expression CLOSE_PAREN statement_block",
    "break_statement : BREAK",
    "variable_declaration_statement : type_name variable_declaration_pairs SEMICOLON",
    "type_name : identifier",
    "type_name : identifier array_indicator",
    "array_indicator : OPEN_BRACE CLOSE_BRACE",
    "variable_declaration_pairs : variable_declaration_pair",
    "variable_declaration_pairs : variable_declaration_pair COMMA variable_declaration_pairs",
    "variable_declaration_pair : identifier opt_variable_initializer",
    "opt_variable_initializer :",
    "opt_variable_initializer : variable_initializer",
    "variable_initializer : EQUAL expression",
    "if_statement : IF OPEN_PAREN expression CLOSE_PAREN statement_block opt_else_block",
    "opt_else_block :",
    "opt_else_block : else_block",
    "else_block : ELSE statement_block",
    "for_statement : FOR OPEN_PAREN for_initializer SEMICOLON expression SEMICOLON for_continuations CLOSE_PAREN statement_block",
    "for_initializer :",
    "for_initializer : variable_declaration_statement",
    "for_initializer : abstract_assignment_list",
    "abstract_assignment_list : abstract_assignment_statement",
    "abstract_assignment_list : abstract_assignment_statement COMMA abstract_assignment_list",
    "for_continuations : statements",
    "switch_statement : SWITCH OPEN_PAREN expression CLOSE_PAREN OPEN_CURLY switch_case_list CLOSE_CURLY",
    "switch_case_list :",
    "switch_case_list : switch_case_default switch_case_list",
    "switch_case_default : switch_case",
    "switch_case_default : switch_default",
    "switch_case : CASE literal COLON statements",
    "switch_default : DEFAULT COLON statements",
    "expression : callable_expression",
    "expression : operation_expression",
    "expression : variable_or_member_reference",
    "expression : literal",
    "expression : field_access_expression",
    "expression : super_expression",
    "expression : this_expression",
    "expression : new_expression",
    "expression : array_access_expression",
    "expression : null_expression",
    "expression : parenthesized_expression",
    "expression : numeric_negation_expression",
    "variable_or_member_reference : identifier",
    "new_expression : new_object_expression",
    "new_expression : new_array_expression",
    "new_object_expression : NEW function_call_base",
    "new_array_expression : NEW identifier OPEN_BRACE expression CLOSE_BRACE",
    "callable_expression : function_call_expression",
    "callable_expression : function_access_expression",
    "function_call_expression : function_call_base",
    "function_call_base : identifier OPEN_PAREN function_args CLOSE_PAREN",
    "function_args :",
    "function_args : function_arg_list",
    "function_arg_list : expression",
    "function_arg_list : expression COMMA function_arg_list",
    "literal : string_literal",
    "literal : numeric_literal",
    "literal : true_expression",
    "literal : false_expression",
    "true_expression : TRUE",
    "false_expression : FALSE",
    "numeric_literal : NUMERIC_LITERAL",
    "string_literal : STRING_LITERAL",
    "operation_expression : comparison_expression",
    "operation_expression : logical_operation_expression",
    "operation_expression : arithmetic_expression",
    "comparison_expression : less_than_expression",
    "comparison_expression : less_eq_expression",
    "comparison_expression : greater_than_expression",
    "comparison_expression : greater_eq_expression",
    "comparison_expression : eq_expression",
    "comparison_expression : not_eq_expression",
    "comparison_expression : conditional_expression",
    "logical_operation_expression : logical_or_expression",
    "logical_operation_expression : logical_and_expression",
    "logical_operation_expression : logical_not_expression",
    "arithmetic_expression : additive_expression",
    "arithmetic_expression : multiplicative_expression",
    "additive_expression : add_expression",
    "additive_expression : subtract_expression",
    "additive_expression : bitwise_and_expression",
    "additive_expression : bitwise_or_expression",
    "additive_expression : left_shift_expression",
    "additive_expression : right_shift_expression",
    "multiplicative_expression : multiply_expression",
    "multiplicative_expression : divide_expression",
    "multiplicative_expression : modulo_expression",
    "field_access_expression : expression DOT identifier",
    "function_access_expression : expression DOT function_call_base",
    "super_expression : SUPER",
    "this_expression : THIS",
    "array_access_expression : expression OPEN_BRACE expression CLOSE_BRACE",
    "null_expression : NULL",
    "parenthesized_expression : OPEN_PAREN expression CLOSE_PAREN",
    "less_than_expression : expression OPEN_ANGLE expression",
    "less_eq_expression : expression OPEN_ANGLE_EQUAL expression",
    "greater_than_expression : expression CLOSE_ANGLE expression",
    "greater_eq_expression : expression CLOSE_ANGLE_EQUAL expression",
    "eq_expression : expression EQUAL2 expression",
    "not_eq_expression : expression EXCLAIM_EQUAL expression",
    "conditional_expression : expression QUESTION expression COLON expression",
    "logical_or_expression : expression BAR2 expression",
    "logical_and_expression : expression AND2 expression",
    "logical_not_expression : EXCLAIM expression",
    "add_expression : expression PLUS expression",
    "subtract_expression : expression MINUS expression",
    "numeric_negation_expression : MINUS expression",
    "multiply_expression : expression ASTERISK expression",
    "divide_expression : expression SLASH expression",
    "modulo_expression : expression PERCENT expression",
    "bitwise_and_expression : expression AND expression",
    "bitwise_or_expression : expression BAR expression",
    "left_shift_expression : expression OPEN_ANGLE2 expression",
    "right_shift_expression : expression CLOSE_ANGLE2 expression",
  };
 public static string getRule (int index) {
    return yyRule [index];
 }
}
  protected static  string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"IDENTIFIER","NUMERIC_LITERAL",
    "STRING_LITERAL","CLASS","EXTENDS","IMPLEMENTS","WHILE","BREAK",
    "RETURN","IF","ELSE","FOR","SWITCH","CASE","DEFAULT","NEW","TRUE",
    "FALSE","THIS","SUPER","NULL","PLUS","MINUS","ASTERISK","SLASH",
    "PERCENT","PLUS_EQUAL","MINUS_EQUAL","PLUS_PLUS","MINUS_MINUS",
    "EQUAL","EQUAL2","COMMA","DOT","COLON","SEMICOLON","QUESTION",
    "EXCLAIM","EXCLAIM_EQUAL","AND","AND2","BAR","BAR2","OPEN_CURLY",
    "CLOSE_CURLY","OPEN_PAREN","CLOSE_PAREN","OPEN_BRACE","CLOSE_BRACE",
    "OPEN_ANGLE","OPEN_ANGLE2","OPEN_ANGLE_EQUAL","CLOSE_ANGLE",
    "CLOSE_ANGLE2","CLOSE_ANGLE_EQUAL","PLUS2","MINUS2",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected string[] yyExpecting (int state) {
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];

    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }

    string [] result = new string[len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = yyNames[token];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;			// initial size
    int yyState = 0;                                   // state stack ptr
    int [] yyStates = new int[yyMax];	                // state stack 
    Object yyVal = null;                               // value stack ptr
    Object [] yyVals = new Object[yyMax];	        // value stack
    int yyToken = -1;					// current input
    int yyErrorFlag = 0;				// #tks to shift

    /*yyLoop:*/ for (int yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        int[] i = new int[yyStates.Length+yyMax];
        yyStates.CopyTo (i, 0);
        yyStates = i;
        Object[] o = new Object[yyVals.Length+yyMax];
        yyVals.CopyTo (o, 0);
        yyVals = o;
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ for (;;) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
            if (debug != null)
              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
            if (debug != null)
              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              // yyerror(String.Format ("syntax error, got token `{0}'", yyname (yyToken)), yyExpecting(yyState));
              if (debug != null) debug.error("syntax error");
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
                  if (debug != null)
                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
              if (debug != null) debug.reject();
              throw new yyParser.yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
                if (debug != null) debug.reject();
                throw new yyParser.yyException("irrecoverable syntax error at end-of-file");
              }
              if (debug != null)
                debug.discard(yyState, yyToken, yyname(yyToken),
  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
        if (debug != null)
          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 2:
#line 147 "ProcessingParser.jay"
  {
		return new List<Expression> ();
	}
  break;
case 3:
#line 151 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		l.Insert (0, (Expression) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 8:
#line 173 "ProcessingParser.jay"
  {
		var name = (string) yyVals[-5+yyTop];
		var baseType = (string) yyVals[-4+yyTop];
		var interfaces = (List<string>) yyVals[-3+yyTop];
		var members = (List<PdlrMember>) yyVals[-2+yyTop];
		yyVal = new PdlrClass (name, baseType, interfaces, members);
	}
  break;
case 9:
#line 182 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 11:
#line 187 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 13:
#line 193 "ProcessingParser.jay"
  {
		yyVal = yyVals[0+yyTop];
	}
  break;
case 14:
#line 200 "ProcessingParser.jay"
  {
	 	yyVal = yyVals[0+yyTop];
	 }
  break;
case 15:
#line 206 "ProcessingParser.jay"
  {
		var l = new List<string> ();
		l.Add ((string) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 16:
#line 212 "ProcessingParser.jay"
  {
		var l = (List<string>) yyVals[0+yyTop];
		l.Insert (0, (string) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 17:
#line 220 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 18:
#line 222 "ProcessingParser.jay"
  {
		var l = (List<PdlrMember>) yyVals[0+yyTop] ?? new List<PdlrMember> ();
		l.Insert (0, (PdlrMember) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 22:
#line 234 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 23:
#line 238 "ProcessingParser.jay"
  {
		yyVal = new PdlrConstructor ((PdlrFunctionBase) yyVals[0+yyTop]);
	}
  break;
case 24:
#line 244 "ProcessingParser.jay"
  {
		yyVal = new PdlrFunction ((string) yyVals[-1+yyTop], (PdlrFunctionBase) yyVals[0+yyTop]);
	}
  break;
case 25:
#line 250 "ProcessingParser.jay"
  {
		yyVal = new PdlrFunctionBase ((string) yyVals[-4+yyTop], (List<FunctionArgument>) yyVals[-2+yyTop], (List<Expression>) yyVals[0+yyTop]);
	}
  break;
case 26:
#line 255 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 27:
#line 257 "ProcessingParser.jay"
  {
		var l = (List<FunctionArgument>) yyVals[0+yyTop] ?? new List<FunctionArgument> ();
		l.Insert (0, (FunctionArgument) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 28:
#line 265 "ProcessingParser.jay"
  {
		yyVal = new FunctionArgument ((string) yyVals[-1+yyTop], (string) yyVals[0+yyTop]);
	}
  break;
case 29:
#line 273 "ProcessingParser.jay"
  {
		yyVal = yyVals[-1+yyTop];
	}
  break;
case 30:
#line 278 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 31:
#line 280 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop] ?? new List<Expression> ();
		l.Insert (0, (Expression) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 32:
#line 287 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 49:
#line 313 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-2+yyTop];
		var e2 = (Expression) yyVals[0+yyTop];
		yyVal = Expression.Assign (e1, Expression.AddChecked (e1, e2));
	}
  break;
case 50:
#line 321 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-2+yyTop];
		var e2 = (Expression) yyVals[0+yyTop];
		yyVal = Expression.Assign (e1, Expression.SubtractChecked (e1, e2));
	}
  break;
case 51:
#line 329 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-1+yyTop];
		yyVal = Expression.Assign (e1, Expression.AddChecked (e1, Expression.Constant (1)));
	}
  break;
case 52:
#line 336 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-1+yyTop];
		yyVal = Expression.Assign (e1, Expression.SubtractChecked (e1, Expression.Constant (1)));
	}
  break;
case 53:
#line 343 "ProcessingParser.jay"
  {
		/* FIXME: get current type, find base constructor, and call it.*/
	}
  break;
case 54:
#line 349 "ProcessingParser.jay"
  {
		yyVal = Expression.Return ((Expression) yyVals[0+yyTop]);
	}
  break;
case 55:
#line 355 "ProcessingParser.jay"
  {
		yyVal = Expression.Assign ((Expression) yyVals[-2+yyTop], (Expression) yyVals[-1+yyTop]);
	}
  break;
case 56:
#line 361 "ProcessingParser.jay"
  {
		yyVal = Expression.Loop ((Expression) yyVals[-2+yyTop], null, Expression.Block (null, (List<Expression>) yyVals[0+yyTop]), Expression.Break (null), null);
	}
  break;
case 57:
#line 365 "ProcessingParser.jay"
  { yyVal = Expression.Break (null); }
  break;
case 58:
#line 371 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is not likely possible here.*/
		var l = new List<VariableExpression> ();
		Type t = CreateType ((string) yyVals[-2+yyTop], (bool) yyVals[-1+yyTop]);
		foreach (VariableDeclarationPair p in yyVals[0+yyTop])
			l.Add (Expression.Variable (t, p)); /* FIXME from here*/
	}
  break;
case 59:
#line 381 "ProcessingParser.jay"
  {
		/* FIXME: implement*/
	}
  break;
case 60:
#line 385 "ProcessingParser.jay"
  {
		/* FIXME: implement*/
	}
  break;
case 62:
#line 394 "ProcessingParser.jay"
  {
		var l = new List<VariableDeclarationPair> ();
		l.Add ((VariableDeclarationPair) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 63:
#line 400 "ProcessingParser.jay"
  {
		var l = (List<VariableDeclarationPair>) yyVals[0+yyTop];
		l.Insert (0, (VariableDeclarationPair) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 64:
#line 408 "ProcessingParser.jay"
  {
		yyVal = new VariableDeclarationPair ((string) yyVals[-1+yyTop], (Expression) yyVals[0+yyTop]);
	}
  break;
case 65:
#line 413 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 67:
#line 418 "ProcessingParser.jay"
  {
		yyVal = yyVals[0+yyTop];
	}
  break;
case 68:
#line 426 "ProcessingParser.jay"
  {
		var cond = (Expression) yyVals[-3+yyTop];
		var tb = (List<Expression>) yyVals[-1+yyTop];
		var fb = (List<Expression>) yyVals[0+yyTop];
		yyVal = eb != null ?
			Expression.IfThenElse (cond, Expression.Block (tb), Expression.Block (fb)) :
			Expression.IfThen (cond, Expression.Block (tb));
	}
  break;
case 69:
#line 436 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 71:
#line 440 "ProcessingParser.jay"
  { yyVal = yyVals[0+yyTop]; }
  break;
case 72:
#line 447 "ProcessingParser.jay"
  {
		var init = (List<Expression>) yyVals[-6+yyTop];
		var cond = (Expression) yyVals[-4+yyTop];
		var cont = (List<Expression>) yyVals[-2+yyTop];
		var loop = Expression.Loop (cond,
			cont != null ? Expression.Block (cont) : null,
			Expression.Block ((List<Expression>) yyVals[0+yyTop]),
			Expression.Break (null),
			new LabelTarget (null));
		yyVal = init != null ? Expression.Block (init, loop) : loop;
	}
  break;
case 73:
#line 460 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 76:
#line 466 "ProcessingParser.jay"
  {
		var l = new List<Expression> ();
		l.Add ((Expression) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 77:
#line 472 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		l.Insert (0, (Expression) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 79:
#line 485 "ProcessingParser.jay"
  {
		yyVal = Expression.Switch (null, null, (Expression) yyVals[-4+yyTop], ((List<SwitchCase>) yyVals[-1+yyTop]).ToArray ());
	}
  break;
case 80:
#line 490 "ProcessingParser.jay"
  { yyVal = new List<SwitchCase> (); }
  break;
case 81:
#line 493 "ProcessingParser.jay"
  {
		var l = (List<SwitchCase>) yyVals[0+yyTop];
		l.Insert (0, (SwitchCase) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 84:
#line 505 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		/* must end with break_statement*/
		if (!(l [l.Length - 1] is BreakExpression))
			l.Add (Expression.Break (null));
		yyVal = Expression.SwitchCase (GetIntValue ((ConstantExpression) yyVals[-2+yyTop]), Expression.Block (l));
	}
  break;
case 85:
#line 515 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		/* must end with break_statement*/
		if (!(l [l.Length - 1] is BreakExpression))
			l.Add (Expression.Break (null));
		yyVal = Expression.DefaultCase (Expression.Block (l));
	}
  break;
case 98:
#line 541 "ProcessingParser.jay"
  {
		/* FIXME: how can I distinguish variable and field-or-property?*/
		/* FIXME: how can I instantiate an Expression for varref?*/
	}
  break;
case 101:
#line 554 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is unlikely possible*/
		var fcb = (KeyValuePair<string, List<Expression>>) yyVals[0+yyTop];
		/* FIXME: no CallSiteBinder?*/
		yyVal = Expression.New (CreateType (fcb.Key), null, fcb.Value);
	}
  break;
case 102:
#line 563 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is unlikely possible*/
		yyVal = Expression.NewArrayBounds (CreateType ((string) yyVals[-3+yyTop]), (Expression) yyVals[-1+yyTop]);
	}
  break;
case 106:
#line 578 "ProcessingParser.jay"
  {
		yyVal = new KeyValuePair<string, List<Expression>> ((string) yyVals[-3+yyTop], (List<Expression>) yyVals[-1+yyTop]);
	}
  break;
case 107:
#line 583 "ProcessingParser.jay"
  { yyVal = new List<Expression> (); }
  break;
case 109:
#line 588 "ProcessingParser.jay"
  {
		var l = new List<Expression> ();
		l.Add ((Expression) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 110:
#line 594 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		l.Insert (0, (Expression) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 115:
#line 606 "ProcessingParser.jay"
  { yyVal = Expression.True (); }
  break;
case 116:
#line 608 "ProcessingParser.jay"
  { yyVal = Expression.False (); }
  break;
case 143:
#line 658 "ProcessingParser.jay"
  {
		yyVal = Expression.PropertyOrField ((Expression) yyVals[-2+yyTop], (string) yyVals[0+yyTop]);
	}
  break;
case 144:
#line 664 "ProcessingParser.jay"
  {
		var inst = (Expression) yyVals[-2+yyTop];
		var fcb = (KeyValuePair<string, List<Expression>>) yyVals[0+yyTop];
		/* FIXME: GetMethod() is unlikely possible*/
		yyVal = Expression.Call (inst, GetMethod (inst, fcb.Key), fcb.Value.ToArray ());
	}
  break;
case 145:
#line 673 "ProcessingParser.jay"
  {
		/* FIXME: needs current type context, Type, and then base type*/
	}
  break;
case 146:
#line 677 "ProcessingParser.jay"
  { }
  break;
case 147:
#line 681 "ProcessingParser.jay"
  {
		yyVal = Expression.ArrayIndex ((Expression) yyVals[-3+yyTop], (Expression) yyVals[-1+yyTop]);
	}
  break;
case 148:
#line 685 "ProcessingParser.jay"
  { yyVal = Expression.Null (); }
  break;
case 149:
#line 687 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 150:
#line 691 "ProcessingParser.jay"
  { yyVal = Expression.LessThan ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 151:
#line 695 "ProcessingParser.jay"
  { yyVal = Expression.LessThanOrEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 152:
#line 699 "ProcessingParser.jay"
  { yyVal = Expression.GreaterThan ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 153:
#line 703 "ProcessingParser.jay"
  { yyVal = Expression.GreaterThanOrEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 154:
#line 707 "ProcessingParser.jay"
  { yyVal = Expression.Equal ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 155:
#line 711 "ProcessingParser.jay"
  { yyVal = Expression.NotEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 156:
#line 715 "ProcessingParser.jay"
  { yyVal = Expression.Condition ((Expression) yyVals[-4+yyTop], (Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 157:
#line 719 "ProcessingParser.jay"
  { yyVal = Expression.OrElse ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 158:
#line 723 "ProcessingParser.jay"
  { yyVal = Expression.AndAlso ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 159:
#line 727 "ProcessingParser.jay"
  { yyVal = Expression.Not ((Expression) yyVals[-1+yyTop]); }
  break;
case 160:
#line 731 "ProcessingParser.jay"
  { yyVal = Expression.AddChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 161:
#line 735 "ProcessingParser.jay"
  { yyVal = Expression.SubtractChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 162:
#line 739 "ProcessingParser.jay"
  { yyVal = Expression.MultiplyChecked ((Expression) yyVals[-1+yyTop], Expression.Constant (-1)); }
  break;
case 163:
#line 743 "ProcessingParser.jay"
  { yyVal = Expression.MultiplyChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 164:
#line 747 "ProcessingParser.jay"
  { yyVal = Expression.DivideChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 165:
#line 751 "ProcessingParser.jay"
  { yyVal = Expression.ModuloChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 166:
#line 755 "ProcessingParser.jay"
  { yyVal = Expression.And ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 167:
#line 759 "ProcessingParser.jay"
  { yyVal = Expression.Or ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 168:
#line 763 "ProcessingParser.jay"
  { yyVal = Expression.LeftShift ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 169:
#line 767 "ProcessingParser.jay"
  { yyVal = Expression.RightShift ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
#line default
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
            if (debug != null)
               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: continue;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: continue;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

   static  short [] yyLhs  = {              -1,
    1,    0,    0,    2,    2,    2,    5,    4,    7,    7,
    8,    8,   10,   11,   12,   12,    9,    9,   13,   13,
   13,   14,   15,    6,   17,   19,   19,   21,   20,   22,
   22,    3,    3,   23,   23,   23,   23,   23,   23,   23,
   23,   24,   24,   24,   24,   26,   26,   26,   37,   38,
   28,   29,   30,   25,   36,   34,   27,   16,   18,   18,
   43,   42,   42,   44,   45,   45,   46,   32,   47,   47,
   48,   33,   49,   49,   49,   51,   51,   50,   35,   52,
   52,   53,   53,   54,   55,   40,   40,   40,   40,   40,
   40,   40,   40,   40,   40,   40,   40,   39,   61,   61,
   66,   67,   31,   31,   69,   68,   41,   41,   71,   71,
   56,   56,   56,   56,   74,   75,   73,   72,   57,   57,
   57,   76,   76,   76,   76,   76,   76,   76,   77,   77,
   77,   78,   78,   89,   89,   89,   89,   89,   89,   90,
   90,   90,   58,   70,   59,   60,   62,   63,   64,   79,
   80,   81,   82,   83,   84,   85,   86,   87,   88,   91,
   92,   65,   97,   98,   99,   93,   94,   95,   96,
  };
   static  short [] yyLen = {           2,
    1,    0,    2,    1,    1,    1,    1,    7,    0,    1,
    0,    1,    2,    2,    1,    3,    0,    2,    1,    1,
    1,    2,    1,    2,    5,    0,    3,    2,    3,    0,
    2,    2,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    3,    3,
    2,    2,    4,    2,    3,    5,    1,    3,    1,    2,
    2,    1,    3,    2,    0,    1,    2,    6,    0,    1,
    2,    9,    0,    1,    1,    1,    3,    1,    7,    0,
    2,    1,    1,    4,    3,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    2,    5,    1,    1,    1,    4,    0,    1,    1,    3,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    3,    3,    1,    1,    4,    1,    3,    3,
    3,    3,    3,    3,    3,    5,    3,    3,    2,    3,
    3,    2,    3,    3,    3,    3,    3,    3,    3,
  };
   static  short [] yyDefRed = {            2,
    0,    1,  117,  118,    0,    0,   57,    0,    0,    0,
    0,    0,  115,  116,  146,    0,  148,    0,    0,    0,
    0,    3,    4,    5,    6,    7,   39,    0,    0,   33,
   34,   35,   36,   37,   38,   40,    0,   42,   43,   44,
   45,   46,   47,   48,    0,    0,   89,   87,   90,   91,
   92,   93,   94,   95,   96,   97,   99,  100,  105,  103,
  104,  111,  112,  113,  114,  119,  120,  121,  122,  123,
  124,  125,  126,  127,  128,  129,  130,  131,  132,  133,
  134,  135,  136,  137,  138,  139,  140,  141,  142,    0,
    0,  145,    0,   86,   88,    0,    0,    0,    0,    0,
  101,    0,    0,  159,    0,    0,    0,   60,    0,   24,
    0,    0,   32,    0,    0,   51,   52,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
   10,    0,    0,    0,   74,    0,    0,    0,    0,   75,
    0,    0,    0,    0,  108,  149,    0,   61,    0,    0,
   64,   66,   58,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  144,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   13,    0,
    0,   12,    0,    0,    0,    0,    0,    0,    0,    0,
   53,  106,    0,    0,    0,    0,    0,   63,    0,  147,
    0,   14,    0,    0,   56,    0,   98,   77,    0,    0,
  102,  110,   28,    0,    0,    0,    0,    0,   21,    0,
    0,   19,   20,    0,   23,    0,    0,    0,   68,   70,
    0,    0,    0,    0,    0,   82,   83,   25,   27,   16,
    8,   18,   22,   31,   29,   71,   78,    0,    0,    0,
   79,   81,    0,    0,   85,   72,   84,
  };
  protected static  short [] yyDgoto  = {             1,
   93,   22,  236,   24,   25,  229,  140,  191,  230,  141,
  192,  212,  231,  232,  233,   27,  235,  146,  206,  215,
  207,  237,   29,   30,   31,   32,   33,   34,   35,   36,
   94,   38,   39,   40,   41,   42,   43,   44,   95,   46,
  154,  111,  108,  112,  161,  162,  239,  240,  149,  258,
  150,  244,  245,  246,  247,   47,   48,   49,   50,   51,
   52,   53,   54,   55,   56,   57,   58,   59,   60,   61,
  155,   62,   63,   64,   65,   66,   67,   68,   69,   70,
   71,   72,   73,   74,   75,   76,   77,   78,   79,   80,
   81,   82,   83,   84,   85,   86,   87,   88,   89,
  };
  protected static  short [] yySindex = {            0,
 -168,    0,    0,    0, -245, -270,    0, -104, -267, -256,
 -251, -245,    0,    0,    0, -209,    0, -104, -104, -104,
 -281,    0,    0,    0,    0,    0,    0, -245, -229,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, -122,  687,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -214,
 -104,    0, -208,    0,    0,  687, -104, -245, -104, -277,
    0, -104,  787,    0, -102, -104, -218,    0, -278,    0,
 -193, -213,    0, -104, -104,    0,    0, -104, -104, -104,
 -104, -104, -104, -104, -245, -104, -104, -104, -104, -104,
 -104, -104, -104, -104, -104, -104, -104, -104, -245, -152,
    0,  381,  415, -201,    0, -245, -177, -169, -179,    0,
  449, -104,  483, -183,    0,    0, -161,    0, -104, -245,
    0,    0,    0, -245,  687,  687,  687,  787,  787,  992,
  992,  992, 1014, -208,    0,  517, 1014, 1036, 1044, 1036,
 1044,  551, -291, -180, -291, -291, -180, -291,    0, -245,
 -156,    0, -144, -144, -130, -245, -104, -126,  585, -104,
    0,    0,  687, -201, -245, -121, -106,    0, -104,    0,
 -105,    0, -245, -136,    0,  -80,    0,    0,  619, -192,
    0,    0,    0, -144, -245, -159, -245, -216,    0, -112,
 -245,    0,    0, -100,    0, -136, -101, -144,    0,    0,
 -136, -244,  -92,  -98, -192,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  -93,  -78, -136,
    0,    0, -144, -136,    0,    0,    0,
  };
  protected static  short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  721,    0,    0,    0,    0,
 -226,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  653,    0,    0,    0,
    0,    0,    0,    0,  755,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -257,
    0,    0,   85,    0,    0,  -77,    0,  -76,    0,    0,
    0,  -89,  899,    0,    0,  -89,    0,    0, -272,    0,
    0,  -67,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  -73,
    0,    0,    0, -239,    0,    0,  -59,    0,    0,    0,
    0,    0,  -66,    0,    0,    0,    0,    0,    0,  -61,
    0,    0,    0,    0, -255, -250, -224,  927,  981,  -60,
   18,  966,  123,  143,    0,    0,  961,  905,  849,  933,
  877,    0,  245,  177,  279,  313,  211,  347,    0,    0,
    0,    0,    0,    0, -272,    0,    0,    0,    0,    0,
    0,    0, -215,  -21,    0,    0,    0,    0,    0,    0,
  -56,    0,  -54,  -52,    0,    1,    0,    0,    0,  -51,
    0,    0,    0,    0,  -61,  821,    0,  -21,    0,    0,
  -54,    0,    0,    0,    0, -263,    0,    0,    0,    0,
  -50,    0,    0,    0,  -51,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -260,
    0,    0,    0, -260,    0,    0,    0,
  };
  protected static  short [] yyGindex = {            0,
   21,    0,  250,    0,    0,  253,    0,    0,   24,    0,
    0,   29,    0,    0,    0,  -96,  254,   -1,   58, -188,
    0,   48,    0,    0,    0,  -94,    0,    0,    0,    0,
   65,    0,    0,    0,    0,    0,    0,    0,   27,  220,
  180,  124,    0,    0,    0,    0,    0,    0,    0,    0,
   94,   47,    0,    0,    0,   51,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   -9,    0,    0,
  105,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,
  };
  protected static  short [] yyTable = {            28,
   69,  145,  101,  147,    9,  216,   30,   30,  159,   30,
   30,    2,  132,    3,    4,  134,   65,   59,  137,   65,
  106,   21,  107,  160,  106,   90,  152,   45,   13,   14,
   59,   91,  100,   49,   97,  248,   49,   30,   50,   30,
   30,   50,    9,   98,   98,   98,  139,   98,  109,  256,
   99,   98,   98,   98,   98,   98,   98,   98,   98,   98,
   98,   98,  113,   98,   55,   37,   98,   55,   98,   98,
   98,   98,   98,   67,  266,  164,   67,  242,  243,   98,
   98,   98,   98,   98,   98,  160,  158,  107,    2,    3,
    4,    5,  102,  106,    6,    7,    8,    9,  163,   10,
   11,  147,  107,   12,   13,   14,   15,   16,   17,  190,
   18,  196,  197,  114,  115,  175,  234,  118,  144,  201,
    2,    3,    4,  132,  148,   19,    6,    7,    8,    9,
  125,   10,   11,   20,  234,   12,   13,   14,   15,   16,
   17,  202,   18,  213,  132,  174,  133,  134,  135,  136,
  137,  138,    2,    3,    4,  214,  159,   19,  205,  189,
  114,  115,  116,  117,  118,   20,  195,   12,   13,   14,
   15,   92,   17,  220,   18,  119,  120,  121,  122,  123,
  204,  224,  225,  227,  195,  124,  238,  125,  251,   19,
  126,  253,  127,  128,  129,  130,  131,   20,  260,  255,
  156,  132,  261,  133,  134,  135,  136,  137,  138,  263,
  211,   28,  264,  107,   54,   73,  217,  163,  163,  163,
  163,  163,  148,  205,   62,  223,   11,   96,  163,   28,
  163,  163,   76,  228,   21,   59,  109,  103,  104,  105,
   45,   26,  163,   15,  163,  204,   17,  211,   30,   80,
   23,  228,   30,   26,  252,  250,   21,   69,   69,   69,
   69,   21,   45,   69,   69,   69,   69,   45,   69,   69,
   69,   69,   69,   69,   69,   69,   69,   69,   37,   69,
   21,  110,  249,  254,   21,  157,   45,  208,  257,  218,
   45,  262,  259,    0,   69,  164,  164,  164,  164,  164,
   37,   69,   69,   69,  222,   37,  164,  265,  164,  164,
  142,  267,    0,    0,    0,    0,  143,    0,  151,    0,
  164,  153,  164,    0,   37,  153,    0,    0,   37,    0,
    0,    0,    0,  165,  166,    0,    0,  167,  168,  169,
  170,  171,  172,  173,    0,  176,  177,  178,  179,  180,
  181,  182,  183,  184,  185,  186,  187,  188,    0,    0,
    0,    0,   98,   98,   98,   98,   98,    0,    0,    0,
    0,  199,   98,   98,   98,   98,   98,   98,  203,   98,
   98,   98,   98,   98,    0,    0,    0,   98,   98,   98,
   98,   98,   98,   98,   98,   98,    0,    0,    0,    0,
  154,  154,  154,  154,  154,    0,    0,    0,    0,    0,
  154,  154,    0,  154,  154,    0,  219,  154,    0,  153,
  143,  143,  143,  143,  143,  154,    0,  154,  226,    0,
  143,  143,  143,  143,  143,  143,    0,  143,  143,  143,
  143,  143,    0,    0,    0,  143,  143,  143,  143,  143,
  143,  143,  143,  143,  168,  168,  168,  168,  168,    0,
    0,    0,    0,    0,  168,  168,  168,  168,  168,  168,
    0,  168,  168,  168,  168,  168,    0,    0,    0,  168,
    0,  168,  168,  168,  168,  168,  168,  168,  169,  169,
  169,  169,  169,    0,    0,    0,    0,    0,  169,  169,
  169,  169,  169,  169,    0,  169,  169,  169,  169,  169,
    0,    0,    0,  169,    0,  169,  169,  169,  169,  169,
  169,  169,  150,  150,  150,  150,  150,    0,    0,    0,
    0,    0,  150,  150,  150,  150,  150,  150,    0,  150,
  150,  150,  150,  150,    0,    0,    0,  150,    0,  150,
  150,    0,  150,  150,    0,  150,  151,  151,  151,  151,
  151,    0,    0,    0,    0,    0,  151,  151,  151,  151,
  151,  151,    0,  151,  151,  151,  151,  151,    0,    0,
    0,  151,    0,  151,  151,    0,  151,  151,    0,  151,
  152,  152,  152,  152,  152,    0,    0,    0,    0,    0,
  152,  152,  152,  152,  152,  152,    0,  152,  152,  152,
  152,  152,    0,    0,    0,  152,    0,  152,  152,    0,
  152,  152,    0,  152,  153,  153,  153,  153,  153,    0,
    0,    0,    0,    0,  153,  153,  153,  153,  153,  153,
    0,  153,  153,  153,  153,  153,    0,    0,    0,  153,
    0,  153,  153,    0,  153,  153,    0,  153,  119,  120,
  121,  122,  123,    0,    0,    0,    0,    0,  124,    0,
  125,    0,    0,  126,    0,  127,  128,  129,  130,  131,
    0,    0,    0,  193,  132,    0,  133,  134,  135,  136,
  137,  138,  119,  120,  121,  122,  123,    0,    0,    0,
    0,    0,  124,    0,  125,    0,    0,  126,    0,  127,
  128,  129,  130,  131,    0,    0,    0,  194,  132,    0,
  133,  134,  135,  136,  137,  138,  119,  120,  121,  122,
  123,    0,    0,    0,    0,    0,  124,    0,  125,    0,
    0,  126,    0,  127,  128,  129,  130,  131,    0,    0,
    0,  198,  132,    0,  133,  134,  135,  136,  137,  138,
  119,  120,  121,  122,  123,    0,    0,    0,    0,    0,
  124,  200,  125,    0,    0,  126,    0,  127,  128,  129,
  130,  131,    0,    0,    0,    0,  132,    0,  133,  134,
  135,  136,  137,  138,  119,  120,  121,  122,  123,    0,
    0,    0,    0,    0,  124,    0,  125,  209,    0,  126,
    0,  127,  128,  129,  130,  131,    0,    0,    0,    0,
  132,    0,  133,  134,  135,  136,  137,  138,  119,  120,
  121,  122,  123,    0,    0,    0,    0,    0,  124,    0,
  125,    0,    0,  126,    0,  127,  128,  129,  130,  131,
    0,    0,    0,    0,  132,  210,  133,  134,  135,  136,
  137,  138,  119,  120,  121,  122,  123,    0,    0,    0,
    0,    0,  124,    0,  125,    0,    0,  126,    0,  127,
  128,  129,  130,  131,    0,    0,    0,    0,  132,  221,
  133,  134,  135,  136,  137,  138,  119,  120,  121,  122,
  123,    0,    0,    0,    0,    0,  124,    0,  125,    0,
  241,  126,    0,  127,  128,  129,  130,  131,    0,    0,
    0,    0,  132,    0,  133,  134,  135,  136,  137,  138,
   86,   86,   86,   86,   86,    0,    0,    0,    0,    0,
   86,    0,   86,    0,   41,   86,    0,   86,   86,   86,
   86,   86,    0,    0,    0,    0,   86,    0,   86,   86,
   86,   86,   86,   86,  119,  120,  121,  122,  123,    0,
    0,    0,    0,    0,  124,    0,  125,    0,    0,  126,
    0,  127,  128,  129,  130,  131,    0,    0,    0,    0,
  132,    0,  133,  134,  135,  136,  137,  138,  145,  145,
  145,  145,  145,    0,    0,    0,    0,    0,  145,    0,
  145,    0,    0,  145,    0,  145,  145,  145,  145,  145,
    0,    0,    0,    0,  145,    0,  145,  145,  145,  145,
  145,  145,   88,   88,   88,   88,   88,    0,    0,    0,
    0,    0,   88,    0,   88,    0,    0,   88,    0,   88,
   88,   88,   88,   88,    0,    0,    0,    0,   88,    0,
   88,   88,   88,   88,   88,   88,  121,  122,  123,    0,
    0,    0,    0,    0,  124,    0,  125,    0,    0,  126,
    0,  127,  128,  129,  130,  131,    0,    0,    0,    0,
  132,    0,  133,  134,  135,  136,  137,  138,  156,  156,
  156,  156,  156,    0,    0,    0,    0,    0,  156,  156,
    0,  156,  156,  156,    0,  156,  156,  156,  156,  156,
    0,    0,    0,  156,    0,  156,  158,  158,  158,  158,
  158,    0,    0,    0,    0,    0,  158,  158,    0,  158,
  158,    0,    0,  158,  158,  158,  158,  158,    0,    0,
    0,  158,    0,  158,  157,  157,  157,  157,  157,    0,
    0,    0,    0,    0,  157,  157,    0,  157,  157,    0,
    0,  157,  157,  157,  157,  157,  162,  162,    0,  157,
    0,  157,  166,  166,  166,  166,  166,  162,    0,  162,
  162,    0,  166,  166,    0,  166,  166,    0,    0,  166,
  166,  162,  166,  162,  160,  160,    0,  166,    0,  166,
  167,  167,  167,  167,  167,  160,    0,  160,  160,    0,
  167,  167,    0,  167,  167,    0,    0,  167,  167,  160,
  167,  160,    0,    0,    0,  167,    0,  167,  155,  155,
  155,  155,  155,  165,  165,  165,  165,  165,  155,  155,
    0,  155,  155,    0,  165,  155,  165,  165,  161,  161,
    0,    0,    0,  155,    0,  155,    0,    0,  165,  161,
  165,  161,  161,    0,    0,    0,    0,    0,    0,  124,
    0,  125,    0,  161,  126,  161,  127,  128,  129,  130,
  131,    0,    0,    0,    0,  132,    0,  133,  134,  135,
  136,  137,  138,  125,    0,    0,  126,    0,    0,  128,
  129,  130,  131,    0,    0,    0,    0,  132,    0,  133,
  134,  135,  136,  137,  138,  125,    0,    0,  126,    0,
    0,    0,  129,  125,  131,    0,  126,    0,    0,  132,
    0,  133,  134,  135,  136,  137,  138,  132,    0,  133,
  134,  135,  136,  137,  138,
  };
  protected static  short [] yyCheck = {             1,
    0,   98,   12,   98,  262,  194,  270,  271,  287,  270,
  271,  257,  304,  258,  259,  307,  289,  257,  310,  292,
  302,    1,  304,  302,  302,    5,  304,    1,  273,  274,
  257,  302,   12,  289,  302,  224,  292,  301,  289,  303,
  301,  292,  300,  283,  284,  302,  261,  287,   28,  238,
  302,  278,  279,  280,  281,  282,  283,  284,  285,  286,
  287,  288,  292,  290,  289,    1,  293,  292,  295,  296,
  297,  298,  299,  289,  263,  289,  292,  270,  271,  306,
  307,  308,  309,  310,  311,  302,  305,  304,  257,  258,
  259,  260,  302,  302,  263,  264,  265,  266,  292,  268,
  269,  196,  304,  272,  273,  274,  275,  276,  277,  262,
  279,  289,  292,  283,  284,  125,  213,  287,   98,  303,
  257,  258,  259,  304,   98,  294,  263,  264,  265,  266,
  290,  268,  269,  302,  231,  272,  273,  274,  275,  276,
  277,  303,  279,  300,  304,  125,  306,  307,  308,  309,
  310,  311,  257,  258,  259,  300,  287,  294,  160,  139,
  283,  284,  285,  286,  287,  302,  146,  272,  273,  274,
  275,  276,  277,  300,  279,  278,  279,  280,  281,  282,
  160,  303,  289,  289,  164,  288,  267,  290,  301,  294,
  293,  292,  295,  296,  297,  298,  299,  302,  291,  301,
  303,  304,  301,  306,  307,  308,  309,  310,  311,  303,
  190,  213,  291,  303,  292,  292,  196,  278,  279,  280,
  281,  282,  196,  225,  292,  205,  300,    8,  289,  231,
  291,  292,  292,  213,  214,  257,  303,   18,   19,   20,
  214,  303,  303,  300,  305,  225,  301,  227,  301,  301,
    1,  231,  303,    1,  231,  227,  236,  257,  258,  259,
  260,  241,  236,  263,  264,  265,  266,  241,  268,  269,
  270,  271,  272,  273,  274,  275,  276,  277,  214,  279,
  260,   28,  225,  236,  264,  106,  260,  164,  241,  196,
  264,  245,  242,   -1,  294,  278,  279,  280,  281,  282,
  236,  301,  302,  303,  200,  241,  289,  260,  291,  292,
   91,  264,   -1,   -1,   -1,   -1,   97,   -1,   99,   -1,
  303,  102,  305,   -1,  260,  106,   -1,   -1,  264,   -1,
   -1,   -1,   -1,  114,  115,   -1,   -1,  118,  119,  120,
  121,  122,  123,  124,   -1,  126,  127,  128,  129,  130,
  131,  132,  133,  134,  135,  136,  137,  138,   -1,   -1,
   -1,   -1,  278,  279,  280,  281,  282,   -1,   -1,   -1,
   -1,  152,  288,  289,  290,  291,  292,  293,  159,  295,
  296,  297,  298,  299,   -1,   -1,   -1,  303,  304,  305,
  306,  307,  308,  309,  310,  311,   -1,   -1,   -1,   -1,
  278,  279,  280,  281,  282,   -1,   -1,   -1,   -1,   -1,
  288,  289,   -1,  291,  292,   -1,  197,  295,   -1,  200,
  278,  279,  280,  281,  282,  303,   -1,  305,  209,   -1,
  288,  289,  290,  291,  292,  293,   -1,  295,  296,  297,
  298,  299,   -1,   -1,   -1,  303,  304,  305,  306,  307,
  308,  309,  310,  311,  278,  279,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,  289,  290,  291,  292,  293,
   -1,  295,  296,  297,  298,  299,   -1,   -1,   -1,  303,
   -1,  305,  306,  307,  308,  309,  310,  311,  278,  279,
  280,  281,  282,   -1,   -1,   -1,   -1,   -1,  288,  289,
  290,  291,  292,  293,   -1,  295,  296,  297,  298,  299,
   -1,   -1,   -1,  303,   -1,  305,  306,  307,  308,  309,
  310,  311,  278,  279,  280,  281,  282,   -1,   -1,   -1,
   -1,   -1,  288,  289,  290,  291,  292,  293,   -1,  295,
  296,  297,  298,  299,   -1,   -1,   -1,  303,   -1,  305,
  306,   -1,  308,  309,   -1,  311,  278,  279,  280,  281,
  282,   -1,   -1,   -1,   -1,   -1,  288,  289,  290,  291,
  292,  293,   -1,  295,  296,  297,  298,  299,   -1,   -1,
   -1,  303,   -1,  305,  306,   -1,  308,  309,   -1,  311,
  278,  279,  280,  281,  282,   -1,   -1,   -1,   -1,   -1,
  288,  289,  290,  291,  292,  293,   -1,  295,  296,  297,
  298,  299,   -1,   -1,   -1,  303,   -1,  305,  306,   -1,
  308,  309,   -1,  311,  278,  279,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,  289,  290,  291,  292,  293,
   -1,  295,  296,  297,  298,  299,   -1,   -1,   -1,  303,
   -1,  305,  306,   -1,  308,  309,   -1,  311,  278,  279,
  280,  281,  282,   -1,   -1,   -1,   -1,   -1,  288,   -1,
  290,   -1,   -1,  293,   -1,  295,  296,  297,  298,  299,
   -1,   -1,   -1,  303,  304,   -1,  306,  307,  308,  309,
  310,  311,  278,  279,  280,  281,  282,   -1,   -1,   -1,
   -1,   -1,  288,   -1,  290,   -1,   -1,  293,   -1,  295,
  296,  297,  298,  299,   -1,   -1,   -1,  303,  304,   -1,
  306,  307,  308,  309,  310,  311,  278,  279,  280,  281,
  282,   -1,   -1,   -1,   -1,   -1,  288,   -1,  290,   -1,
   -1,  293,   -1,  295,  296,  297,  298,  299,   -1,   -1,
   -1,  303,  304,   -1,  306,  307,  308,  309,  310,  311,
  278,  279,  280,  281,  282,   -1,   -1,   -1,   -1,   -1,
  288,  289,  290,   -1,   -1,  293,   -1,  295,  296,  297,
  298,  299,   -1,   -1,   -1,   -1,  304,   -1,  306,  307,
  308,  309,  310,  311,  278,  279,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,   -1,  290,  291,   -1,  293,
   -1,  295,  296,  297,  298,  299,   -1,   -1,   -1,   -1,
  304,   -1,  306,  307,  308,  309,  310,  311,  278,  279,
  280,  281,  282,   -1,   -1,   -1,   -1,   -1,  288,   -1,
  290,   -1,   -1,  293,   -1,  295,  296,  297,  298,  299,
   -1,   -1,   -1,   -1,  304,  305,  306,  307,  308,  309,
  310,  311,  278,  279,  280,  281,  282,   -1,   -1,   -1,
   -1,   -1,  288,   -1,  290,   -1,   -1,  293,   -1,  295,
  296,  297,  298,  299,   -1,   -1,   -1,   -1,  304,  305,
  306,  307,  308,  309,  310,  311,  278,  279,  280,  281,
  282,   -1,   -1,   -1,   -1,   -1,  288,   -1,  290,   -1,
  292,  293,   -1,  295,  296,  297,  298,  299,   -1,   -1,
   -1,   -1,  304,   -1,  306,  307,  308,  309,  310,  311,
  278,  279,  280,  281,  282,   -1,   -1,   -1,   -1,   -1,
  288,   -1,  290,   -1,  292,  293,   -1,  295,  296,  297,
  298,  299,   -1,   -1,   -1,   -1,  304,   -1,  306,  307,
  308,  309,  310,  311,  278,  279,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,   -1,  290,   -1,   -1,  293,
   -1,  295,  296,  297,  298,  299,   -1,   -1,   -1,   -1,
  304,   -1,  306,  307,  308,  309,  310,  311,  278,  279,
  280,  281,  282,   -1,   -1,   -1,   -1,   -1,  288,   -1,
  290,   -1,   -1,  293,   -1,  295,  296,  297,  298,  299,
   -1,   -1,   -1,   -1,  304,   -1,  306,  307,  308,  309,
  310,  311,  278,  279,  280,  281,  282,   -1,   -1,   -1,
   -1,   -1,  288,   -1,  290,   -1,   -1,  293,   -1,  295,
  296,  297,  298,  299,   -1,   -1,   -1,   -1,  304,   -1,
  306,  307,  308,  309,  310,  311,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,   -1,  290,   -1,   -1,  293,
   -1,  295,  296,  297,  298,  299,   -1,   -1,   -1,   -1,
  304,   -1,  306,  307,  308,  309,  310,  311,  278,  279,
  280,  281,  282,   -1,   -1,   -1,   -1,   -1,  288,  289,
   -1,  291,  292,  293,   -1,  295,  296,  297,  298,  299,
   -1,   -1,   -1,  303,   -1,  305,  278,  279,  280,  281,
  282,   -1,   -1,   -1,   -1,   -1,  288,  289,   -1,  291,
  292,   -1,   -1,  295,  296,  297,  298,  299,   -1,   -1,
   -1,  303,   -1,  305,  278,  279,  280,  281,  282,   -1,
   -1,   -1,   -1,   -1,  288,  289,   -1,  291,  292,   -1,
   -1,  295,  296,  297,  298,  299,  278,  279,   -1,  303,
   -1,  305,  278,  279,  280,  281,  282,  289,   -1,  291,
  292,   -1,  288,  289,   -1,  291,  292,   -1,   -1,  295,
  296,  303,  298,  305,  278,  279,   -1,  303,   -1,  305,
  278,  279,  280,  281,  282,  289,   -1,  291,  292,   -1,
  288,  289,   -1,  291,  292,   -1,   -1,  295,  296,  303,
  298,  305,   -1,   -1,   -1,  303,   -1,  305,  278,  279,
  280,  281,  282,  278,  279,  280,  281,  282,  288,  289,
   -1,  291,  292,   -1,  289,  295,  291,  292,  278,  279,
   -1,   -1,   -1,  303,   -1,  305,   -1,   -1,  303,  289,
  305,  291,  292,   -1,   -1,   -1,   -1,   -1,   -1,  288,
   -1,  290,   -1,  303,  293,  305,  295,  296,  297,  298,
  299,   -1,   -1,   -1,   -1,  304,   -1,  306,  307,  308,
  309,  310,  311,  290,   -1,   -1,  293,   -1,   -1,  296,
  297,  298,  299,   -1,   -1,   -1,   -1,  304,   -1,  306,
  307,  308,  309,  310,  311,  290,   -1,   -1,  293,   -1,
   -1,   -1,  297,  290,  299,   -1,  293,   -1,   -1,  304,
   -1,  306,  307,  308,  309,  310,  311,  304,   -1,  306,
  307,  308,  309,  310,  311,
  };

#line 771 "ProcessingParser.jay"
} // end of class
#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 Console.Error.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int IDENTIFIER = 257;
  public const int NUMERIC_LITERAL = 258;
  public const int STRING_LITERAL = 259;
  public const int CLASS = 260;
  public const int EXTENDS = 261;
  public const int IMPLEMENTS = 262;
  public const int WHILE = 263;
  public const int BREAK = 264;
  public const int RETURN = 265;
  public const int IF = 266;
  public const int ELSE = 267;
  public const int FOR = 268;
  public const int SWITCH = 269;
  public const int CASE = 270;
  public const int DEFAULT = 271;
  public const int NEW = 272;
  public const int TRUE = 273;
  public const int FALSE = 274;
  public const int THIS = 275;
  public const int SUPER = 276;
  public const int NULL = 277;
  public const int PLUS = 278;
  public const int MINUS = 279;
  public const int ASTERISK = 280;
  public const int SLASH = 281;
  public const int PERCENT = 282;
  public const int PLUS_EQUAL = 283;
  public const int MINUS_EQUAL = 284;
  public const int PLUS_PLUS = 285;
  public const int MINUS_MINUS = 286;
  public const int EQUAL = 287;
  public const int EQUAL2 = 288;
  public const int COMMA = 289;
  public const int DOT = 290;
  public const int COLON = 291;
  public const int SEMICOLON = 292;
  public const int QUESTION = 293;
  public const int EXCLAIM = 294;
  public const int EXCLAIM_EQUAL = 295;
  public const int AND = 296;
  public const int AND2 = 297;
  public const int BAR = 298;
  public const int BAR2 = 299;
  public const int OPEN_CURLY = 300;
  public const int CLOSE_CURLY = 301;
  public const int OPEN_PAREN = 302;
  public const int CLOSE_PAREN = 303;
  public const int OPEN_BRACE = 304;
  public const int CLOSE_BRACE = 305;
  public const int OPEN_ANGLE = 306;
  public const int OPEN_ANGLE2 = 307;
  public const int OPEN_ANGLE_EQUAL = 308;
  public const int CLOSE_ANGLE = 309;
  public const int CLOSE_ANGLE2 = 310;
  public const int CLOSE_ANGLE_EQUAL = 311;
  public const int PLUS2 = 312;
  public const int MINUS2 = 313;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
