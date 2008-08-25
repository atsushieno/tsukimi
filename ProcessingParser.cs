// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 3 "ProcessingParser.jay"
using System;
using System.IO;
using System.Collections.Generic;
using ProcessingDlr.Ast;

namespace ProcessingDlr
{

	public class ProcessingParser
	{
		string filename;
		int yacc_verbose_flag;

		public ProcessingParser (string filename)
		{
			this.filename = filename;
		}

		public void Parse ()
		{
			yyparse (new Tokenizer (filename));
		}
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
    null,null,null,null,null,null,null,"ERROR","EOF","IDENTIFIER",
    "NUMERIC_LITERAL","STRING_LITERAL","CLASS","EXTENDS","IMPLEMENTS",
    "WHILE","BREAK","RETURN","IF","ELSE","FOR","SWITCH","CASE","DEFAULT",
    "NEW","TRUE","FALSE","THIS","SUPER","NULL","PLUS","MINUS","ASTERISK",
    "SLASH","PERCENT","PLUS_EQUAL","MINUS_EQUAL","PLUS_PLUS",
    "MINUS_MINUS","EQUAL","EQUAL2","COMMA","DOT","COLON","SEMICOLON",
    "QUESTION","EXCLAIM","EXCLAIM_EQUAL","AND","AND2","BAR","BAR2",
    "OPEN_CURLY","CLOSE_CURLY","OPEN_PAREN","CLOSE_PAREN","OPEN_BRACE",
    "CLOSE_BRACE","OPEN_ANGLE","OPEN_ANGLE2","OPEN_ANGLE_EQUAL",
    "CLOSE_ANGLE","CLOSE_ANGLE2","CLOSE_ANGLE_EQUAL","PLUS2","MINUS2",
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
#line 109 "ProcessingParser.jay"
  {
		yyVal = new List<ITopLevelContent> ();
	}
  break;
case 3:
#line 113 "ProcessingParser.jay"
  {
		var l = (List<ITopLevelContent>) yyVals[0+yyTop];
		l.Insert (0, (ITopLevelContent) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 8:
#line 135 "ProcessingParser.jay"
  {
		var name = (string) yyVals[-5+yyTop];
		var baseType = (string) yyVals[-4+yyTop];
		var interfaces = (List<string>) yyVals[-3+yyTop];
		var members = (List<MemberDefinition>) yyVals[-2+yyTop];
		yyVal = new ClassDefinition (name, baseType, interfaces, members);
	}
  break;
case 9:
#line 144 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 11:
#line 149 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 13:
#line 155 "ProcessingParser.jay"
  {
		yyVal = yyVals[0+yyTop];
	}
  break;
case 14:
#line 162 "ProcessingParser.jay"
  {
	 	yyVal = yyVals[0+yyTop];
	 }
  break;
case 15:
#line 168 "ProcessingParser.jay"
  {
		var l = new List<string> ();
		l.Add ((string) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 16:
#line 174 "ProcessingParser.jay"
  {
		var l = (List<string>) yyVals[0+yyTop];
		l.Insert (0, (string) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 17:
#line 182 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 18:
#line 184 "ProcessingParser.jay"
  {
		var l = (List<MemberDefinition>) yyVals[0+yyTop] ?? new List<MemberDefinition> ();
		l.Insert (0, (MemberDefinition) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 22:
#line 196 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 23:
#line 200 "ProcessingParser.jay"
  {
		yyVal = new ConstructorDefinition ((FunctionBase) yyVals[0+yyTop]);
	}
  break;
case 24:
#line 206 "ProcessingParser.jay"
  {
		yyVal = new FunctionDefinition ((string) yyVals[-1+yyTop], (FunctionBase) yyVals[0+yyTop]);
	}
  break;
case 25:
#line 212 "ProcessingParser.jay"
  {
		yyVal = new FunctionBase ((string) yyVals[-4+yyTop], (List<FunctionArgument>) yyVals[-2+yyTop], Statement.Block ((List<Statement>) yyVals[0+yyTop]));
	}
  break;
case 26:
#line 217 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 27:
#line 219 "ProcessingParser.jay"
  {
		var l = (List<FunctionArgument>) yyVals[0+yyTop] ?? new List<FunctionArgument> ();
		l.Insert (0, (FunctionArgument) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 28:
#line 227 "ProcessingParser.jay"
  {
		yyVal = new FunctionArgument () { Type = (string) yyVals[-1+yyTop], Name = (string) yyVals[0+yyTop]};
	}
  break;
case 29:
#line 235 "ProcessingParser.jay"
  {
		yyVal = yyVals[-1+yyTop];
	}
  break;
case 30:
#line 240 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 31:
#line 242 "ProcessingParser.jay"
  {
		var l = (List<Statement>) yyVals[0+yyTop] ?? new List<Statement> ();
		l.Insert (0, (Statement) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 32:
#line 249 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 49:
#line 275 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-2+yyTop];
		var e2 = (Expression) yyVals[0+yyTop];
		yyVal = Statement.Assign (e1, Expression.AddChecked (e1, e2));
	}
  break;
case 50:
#line 283 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-2+yyTop];
		var e2 = (Expression) yyVals[0+yyTop];
		yyVal = Statement.Assign (e1, Expression.SubtractChecked (e1, e2));
	}
  break;
case 51:
#line 291 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-1+yyTop];
		yyVal = Statement.Assign (e1, Expression.AddChecked (e1, Expression.Constant (1)));
	}
  break;
case 52:
#line 298 "ProcessingParser.jay"
  {
		var e1 = (Expression) yyVals[-1+yyTop];
		yyVal = Statement.Assign (e1, Expression.SubtractChecked (e1, Expression.Constant (1)));
	}
  break;
case 53:
#line 305 "ProcessingParser.jay"
  {
		/* FIXME: get current type, find base constructor, and call it.*/
	}
  break;
case 54:
#line 311 "ProcessingParser.jay"
  {
		yyVal = Statement.Return ((Expression) yyVals[0+yyTop]);
	}
  break;
case 55:
#line 317 "ProcessingParser.jay"
  {
		yyVal = Statement.Assign ((Expression) yyVals[-2+yyTop], (Expression) yyVals[-1+yyTop]);
	}
  break;
case 56:
#line 323 "ProcessingParser.jay"
  {
		yyVal = Statement.Loop ((Expression) yyVals[-2+yyTop], Statement.Block ((List<Statement>) yyVals[0+yyTop]));
	}
  break;
case 57:
#line 327 "ProcessingParser.jay"
  { yyVal = Statement.Break (); }
  break;
case 58:
#line 333 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is not likely possible here.*/
		var l = new List<Statement> ();
		string type = (string) yyVals[-2+yyTop];
		foreach (VariableDeclarationPair p in (List<VariableDeclarationPair>) yyVals[-1+yyTop])
			l.Add (Statement.DeclareVariable (type, p.Name, p.Initializer));
		yyVal = l;
	}
  break;
case 59:
#line 344 "ProcessingParser.jay"
  {
		/* FIXME: implement*/
	}
  break;
case 60:
#line 348 "ProcessingParser.jay"
  {
		/* FIXME: implement*/
	}
  break;
case 62:
#line 357 "ProcessingParser.jay"
  {
		var l = new List<VariableDeclarationPair> ();
		l.Add ((VariableDeclarationPair) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 63:
#line 363 "ProcessingParser.jay"
  {
		var l = (List<VariableDeclarationPair>) yyVals[0+yyTop];
		l.Insert (0, (VariableDeclarationPair) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 64:
#line 371 "ProcessingParser.jay"
  {
		yyVal = new VariableDeclarationPair () {Name = (string) yyVals[-1+yyTop], Initializer = (Expression) yyVals[0+yyTop]};
	}
  break;
case 65:
#line 376 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 67:
#line 381 "ProcessingParser.jay"
  {
		yyVal = yyVals[0+yyTop];
	}
  break;
case 68:
#line 389 "ProcessingParser.jay"
  {
		var cond = (Expression) yyVals[-3+yyTop];
		var tb = (List<Statement>) yyVals[-1+yyTop];
		var fb = (List<Statement>) yyVals[0+yyTop];
		yyVal = fb != null ?
			Statement.IfThenElse (cond, Statement.Block (tb), Statement.Block (fb)) :
			Statement.IfThen (cond, Statement.Block (tb));
	}
  break;
case 69:
#line 399 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 71:
#line 403 "ProcessingParser.jay"
  { yyVal = yyVals[0+yyTop]; }
  break;
case 72:
#line 410 "ProcessingParser.jay"
  {
		var init = (List<Expression>) yyVals[-6+yyTop];
		var cond = (Expression) yyVals[-4+yyTop];
		var cont = (List<Expression>) yyVals[-2+yyTop];
		var body = Statement.Block ((List<Statement>) yyVals[0+yyTop]);
		yyVal = Statement.For (init, cond, cont, body);
	}
  break;
case 73:
#line 419 "ProcessingParser.jay"
  { yyVal = null; }
  break;
case 76:
#line 425 "ProcessingParser.jay"
  {
		var l = new List<Expression> ();
		l.Add ((Expression) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 77:
#line 431 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		l.Insert (0, (Expression) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 79:
#line 444 "ProcessingParser.jay"
  {
		yyVal = Statement.Switch ((Expression) yyVals[-4+yyTop], ((List<SwitchCase>) yyVals[-1+yyTop]).ToArray ());
	}
  break;
case 80:
#line 449 "ProcessingParser.jay"
  { yyVal = new List<SwitchCase> (); }
  break;
case 81:
#line 452 "ProcessingParser.jay"
  {
		var l = (List<SwitchCase>) yyVals[0+yyTop];
		l.Insert (0, (SwitchCase) yyVals[-1+yyTop]);
		yyVal = l;
	}
  break;
case 84:
#line 464 "ProcessingParser.jay"
  {
		var l = (List<Statement>) yyVals[0+yyTop];
		/* must end with break_statement*/
		if (!(l [l.Count - 1] is BreakStatement))
			l.Add (Statement.Break ());
		yyVal = new SwitchCase ((ConstantExpression) yyVals[-2+yyTop], l);
	}
  break;
case 85:
#line 474 "ProcessingParser.jay"
  {
		var l = (List<Statement>) yyVals[0+yyTop];
		/* must end with break_statement*/
		if (!(l [l.Count - 1] is BreakStatement))
			l.Add (Statement.Break ());
		yyVal = new DefaultCase (l);
	}
  break;
case 98:
#line 500 "ProcessingParser.jay"
  {
		/* FIXME: how can I distinguish variable and field-or-property?*/
		/* FIXME: how can I instantiate an Expression for varref?*/
	}
  break;
case 101:
#line 513 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is unlikely possible*/
		var fcb = (KeyValuePair<string, List<Expression>>) yyVals[0+yyTop];
		/* FIXME: no CallSiteBinder?*/
		yyVal = Expression.New (fcb.Key, fcb.Value);
	}
  break;
case 102:
#line 522 "ProcessingParser.jay"
  {
		/* FIXME: CreateType() is unlikely possible*/
		yyVal = Expression.NewArrayBounds ((string) yyVals[-3+yyTop], (Expression) yyVals[-1+yyTop]);
	}
  break;
case 106:
#line 537 "ProcessingParser.jay"
  {
		yyVal = new KeyValuePair<string, List<Expression>> ((string) yyVals[-3+yyTop], (List<Expression>) yyVals[-1+yyTop]);
	}
  break;
case 107:
#line 542 "ProcessingParser.jay"
  { yyVal = new List<Expression> (); }
  break;
case 109:
#line 547 "ProcessingParser.jay"
  {
		var l = new List<Expression> ();
		l.Add ((Expression) yyVals[0+yyTop]);
		yyVal = l;
	}
  break;
case 110:
#line 553 "ProcessingParser.jay"
  {
		var l = (List<Expression>) yyVals[0+yyTop];
		l.Insert (0, (Expression) yyVals[-2+yyTop]);
		yyVal = l;
	}
  break;
case 115:
#line 565 "ProcessingParser.jay"
  { yyVal = Expression.True (); }
  break;
case 116:
#line 567 "ProcessingParser.jay"
  { yyVal = Expression.False (); }
  break;
case 143:
#line 617 "ProcessingParser.jay"
  {
		yyVal = Expression.PropertyOrField ((Expression) yyVals[-2+yyTop], (string) yyVals[0+yyTop]);
	}
  break;
case 144:
#line 623 "ProcessingParser.jay"
  {
		var inst = (Expression) yyVals[-2+yyTop];
		var fcb = (KeyValuePair<string, List<Expression>>) yyVals[0+yyTop];
		/* FIXME: GetMethod() is unlikely possible*/
		yyVal = Expression.Call (inst, fcb.Key, fcb.Value);
	}
  break;
case 145:
#line 632 "ProcessingParser.jay"
  {
		/* FIXME: needs current type context, Type, and then base type*/
	}
  break;
case 146:
#line 636 "ProcessingParser.jay"
  { }
  break;
case 147:
#line 640 "ProcessingParser.jay"
  {
		yyVal = Expression.ArrayIndex ((Expression) yyVals[-3+yyTop], (Expression) yyVals[-1+yyTop]);
	}
  break;
case 148:
#line 644 "ProcessingParser.jay"
  { yyVal = Expression.Null (); }
  break;
case 149:
#line 646 "ProcessingParser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 150:
#line 650 "ProcessingParser.jay"
  { yyVal = Expression.LessThan ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 151:
#line 654 "ProcessingParser.jay"
  { yyVal = Expression.LessThanOrEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 152:
#line 658 "ProcessingParser.jay"
  { yyVal = Expression.GreaterThan ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 153:
#line 662 "ProcessingParser.jay"
  { yyVal = Expression.GreaterThanOrEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 154:
#line 666 "ProcessingParser.jay"
  { yyVal = Expression.Equal ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 155:
#line 670 "ProcessingParser.jay"
  { yyVal = Expression.NotEqual ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 156:
#line 674 "ProcessingParser.jay"
  { yyVal = Expression.Condition ((Expression) yyVals[-4+yyTop], (Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 157:
#line 678 "ProcessingParser.jay"
  { yyVal = Expression.OrElse ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 158:
#line 682 "ProcessingParser.jay"
  { yyVal = Expression.AndAlso ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 159:
#line 686 "ProcessingParser.jay"
  { yyVal = Expression.Not ((Expression) yyVals[-1+yyTop]); }
  break;
case 160:
#line 690 "ProcessingParser.jay"
  { yyVal = Expression.AddChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 161:
#line 694 "ProcessingParser.jay"
  { yyVal = Expression.SubtractChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 162:
#line 698 "ProcessingParser.jay"
  { yyVal = Expression.MultiplyChecked ((Expression) yyVals[-1+yyTop], Expression.Constant (-1)); }
  break;
case 163:
#line 702 "ProcessingParser.jay"
  { yyVal = Expression.MultiplyChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 164:
#line 706 "ProcessingParser.jay"
  { yyVal = Expression.DivideChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 165:
#line 710 "ProcessingParser.jay"
  { yyVal = Expression.ModuloChecked ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 166:
#line 714 "ProcessingParser.jay"
  { yyVal = Expression.And ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 167:
#line 718 "ProcessingParser.jay"
  { yyVal = Expression.Or ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 168:
#line 722 "ProcessingParser.jay"
  { yyVal = Expression.LeftShift ((Expression) yyVals[-2+yyTop], (Expression) yyVals[0+yyTop]); }
  break;
case 169:
#line 726 "ProcessingParser.jay"
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
 -171,    0,    0,    0, -244, -275,    0,  112, -257, -230,
 -218, -244,    0,    0,    0, -217,    0,  112,  112,  112,
 -272,    0,    0,    0,    0,    0,    0, -244, -231,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, -174,  759,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -165,
  112,    0, -203,    0,    0,  759,  112, -244,  112, -186,
    0,  112, -145,    0,  419,  112, -198,    0, -277,    0,
 -175, -168,    0,  112,  112,    0,    0,  112,  112,  112,
  112,  112,  112,  112, -244,  112,  112,  112,  112,  112,
  112,  112,  112,  112,  112,  112,  112,  112, -244, -138,
    0,  453,  487, -178,    0, -244, -162, -264, -164,    0,
  521,  112,  555, -173,    0,    0, -169,    0,  112, -244,
    0,    0,    0, -244,  759,  759,  759, -145, -145, 1065,
 1065, 1065, 1087, -203,    0,  589, 1087, 1109,  109, 1109,
  109,  623, -293, -172, -293, -293, -172, -293,    0, -244,
 -161,    0, -160, -160, -158, -244,  112, -159,  657,  112,
    0,    0,  759, -178, -244, -156, -151,    0,  112,    0,
 -147,    0, -244,  -86,    0, -123,    0,    0,  691, -207,
    0,    0,    0, -160, -244, -106, -244, -182,    0, -152,
 -244,    0,    0, -137,    0,  -86, -143, -160,    0,    0,
  -86, -183, -135, -133, -207,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, -134, -121,  -86,
    0,    0, -160,  -86,    0,    0,    0,
  };
  protected static  short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  793,    0,    0,    0,    0,
 -228,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  725,    0,    0,    0,
    0,    0,    0,    0,  827,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -259,
    0,    0,  147,    0,    0, -118,    0, -117,    0,    0,
    0, -127,    5,    0,    0, -127,    0,    0, -274,    0,
    0, -100,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -105,
    0,    0,    0, -241,    0,    0,  -98,    0,    0,    0,
    0,    0, -107,    0,    0,    0,    0,    0,    0, -104,
    0,    0,    0,    0, -268, -252, -245,   89,  939, 1034,
 1039, 1054, 1001,  181,    0,    0, 1006,  945,  889,  973,
  917,    0,  283,  215,  317,  351,  249,  385,    0,    0,
    0,    0,    0,    0, -274,    0,    0,    0,    0,    0,
    0,    0, -215,  -60,    0,    0,    0,    0,    0,    0,
  -93,    0,  -95,  -92,    0,    1,    0,    0,    0,  -89,
    0,    0,    0,    0, -104,  861,    0,  -60,    0,    0,
  -95,    0,    0,    0,    0, -265,    0,    0,    0,    0,
  -90,    0,    0,    0,  -89,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -262,
    0,    0,    0, -262,    0,    0,    0,
  };
  protected static  short [] yyGindex = {            0,
   23,    0,  216,    0,    0,  219,    0,    0,  -15,    0,
    0,   -6,    0,    0,    0,  -96,  194,   -1,   -2, -188,
    0, -227,    0,    0,    0,  -94,    0,    0,    0,    0,
   67,    0,    0,    0,    0,    0,    0,    0,   29,  226,
  120,   63,    0,    0,    0,    0,    0,    0,    0,    0,
   33,  -14,    0,    0,    0,  -10,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   -9,    0,    0,
   35,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,
  };
  protected static  short [] yyTable = {            28,
   69,  145,  101,  147,    9,  216,   30,   30,  254,   30,
   30,  159,  132,  257,    2,  134,   65,   59,  137,   65,
  114,  115,   49,   21,  118,   49,  160,   90,   91,   45,
   59,  106,  265,  107,  100,  248,  267,   30,   50,   30,
   30,   50,    9,   98,   98,   55,   97,   98,   55,  256,
  109,   98,   98,   98,   98,   98,   98,   98,   98,   98,
   98,   98,  113,   98,  242,  243,   98,   37,   98,   98,
   98,   98,   98,   98,  266,   67,    3,    4,   67,   98,
   98,   98,   98,   98,   98,   99,  102,    2,    3,    4,
    5,   13,   14,    6,    7,    8,    9,  139,   10,   11,
  106,  147,   12,   13,   14,   15,   16,   17,  158,   18,
  114,  115,  116,  117,  118,  175,  234,  106,  163,  152,
  144,  160,  164,  107,   19,  190,  148,  107,  196,  197,
  159,  201,   20,  132,  234,  202,  121,  122,  123,  225,
  213,  214,  220,  227,  124,  238,  125,  174,  224,  126,
  251,  127,  128,  129,  130,  131,  253,  260,  205,  255,
  132,  189,  133,  134,  135,  136,  137,  138,  195,  261,
  263,  264,    2,    3,    4,   54,   73,  107,    6,    7,
    8,    9,  204,   10,   11,  125,  195,   12,   13,   14,
   15,   16,   17,   62,   18,   76,   11,  109,   59,  132,
   26,  133,  134,  135,  136,  137,  138,   17,   15,   19,
   30,   28,  211,   80,   30,  252,   23,   20,  217,   26,
  250,  110,  249,  205,  148,  157,  208,  223,  218,   28,
  262,  259,    0,   96,  222,  228,   21,    0,    0,    0,
    0,    0,   45,  103,  104,  105,    0,  204,    0,  211,
    0,    0,    0,  228,    0,    0,    0,    0,   21,   69,
   69,   69,   69,   21,   45,   69,   69,   69,   69,   45,
   69,   69,   69,   69,   69,   69,   69,   69,   69,   69,
   37,   69,   21,    0,  162,  162,   21,    0,   45,    0,
    0,    0,   45,    0,    0,  162,   69,  162,  162,    0,
    0,    0,   37,   69,   69,   69,    0,   37,    0,  162,
    0,  162,    0,    0,    0,    0,  142,    0,    0,    0,
    0,    0,  143,    0,  151,    0,   37,  153,    0,    0,
   37,  153,    0,    0,    0,    0,    0,    0,    0,  165,
  166,    0,    0,  167,  168,  169,  170,  171,  172,  173,
    0,  176,  177,  178,  179,  180,  181,  182,  183,  184,
  185,  186,  187,  188,    0,    0,    0,    0,  160,  160,
    2,    3,    4,    0,    0,    0,    0,  199,    0,  160,
    0,  160,  160,    0,  203,   12,   13,   14,   15,   92,
   17,    0,   18,  160,    0,  160,    0,    0,    0,    0,
  125,    0,    0,  126,    0,    0,    0,   19,    0,    0,
    0,    0,    0,    0,  132,   20,  133,  134,  135,  136,
  137,  138,  219,    0,    0,  153,   98,   98,   98,   98,
   98,    0,    0,    0,  226,    0,   98,   98,   98,   98,
   98,   98,    0,   98,   98,   98,   98,   98,    0,    0,
    0,   98,   98,   98,   98,   98,   98,   98,   98,   98,
  143,  143,  143,  143,  143,    0,    0,    0,    0,    0,
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
    0,    0,    0,  156,  132,    0,  133,  134,  135,  136,
  137,  138,  119,  120,  121,  122,  123,    0,    0,    0,
    0,    0,  124,    0,  125,    0,    0,  126,    0,  127,
  128,  129,  130,  131,    0,    0,    0,  193,  132,    0,
  133,  134,  135,  136,  137,  138,  119,  120,  121,  122,
  123,    0,    0,    0,    0,    0,  124,    0,  125,    0,
    0,  126,    0,  127,  128,  129,  130,  131,    0,    0,
    0,  194,  132,    0,  133,  134,  135,  136,  137,  138,
  119,  120,  121,  122,  123,    0,    0,    0,    0,    0,
  124,    0,  125,    0,    0,  126,    0,  127,  128,  129,
  130,  131,    0,    0,    0,  198,  132,    0,  133,  134,
  135,  136,  137,  138,  119,  120,  121,  122,  123,    0,
    0,    0,    0,    0,  124,  200,  125,    0,    0,  126,
    0,  127,  128,  129,  130,  131,    0,    0,    0,    0,
  132,    0,  133,  134,  135,  136,  137,  138,  119,  120,
  121,  122,  123,    0,    0,    0,    0,    0,  124,    0,
  125,  209,    0,  126,    0,  127,  128,  129,  130,  131,
    0,    0,    0,    0,  132,    0,  133,  134,  135,  136,
  137,  138,  119,  120,  121,  122,  123,    0,    0,    0,
    0,    0,  124,    0,  125,    0,    0,  126,    0,  127,
  128,  129,  130,  131,    0,    0,    0,    0,  132,  210,
  133,  134,  135,  136,  137,  138,  119,  120,  121,  122,
  123,    0,    0,    0,    0,    0,  124,    0,  125,    0,
    0,  126,    0,  127,  128,  129,  130,  131,    0,    0,
    0,    0,  132,  221,  133,  134,  135,  136,  137,  138,
  119,  120,  121,  122,  123,    0,    0,    0,    0,    0,
  124,    0,  125,    0,  241,  126,    0,  127,  128,  129,
  130,  131,    0,    0,    0,    0,  132,    0,  133,  134,
  135,  136,  137,  138,   86,   86,   86,   86,   86,    0,
    0,    0,    0,    0,   86,    0,   86,    0,   41,   86,
    0,   86,   86,   86,   86,   86,    0,    0,    0,    0,
   86,    0,   86,   86,   86,   86,   86,   86,  119,  120,
  121,  122,  123,    0,    0,    0,    0,    0,  124,    0,
  125,    0,    0,  126,    0,  127,  128,  129,  130,  131,
    0,    0,    0,    0,  132,    0,  133,  134,  135,  136,
  137,  138,  145,  145,  145,  145,  145,    0,    0,    0,
    0,    0,  145,    0,  145,    0,    0,  145,    0,  145,
  145,  145,  145,  145,    0,    0,    0,    0,  145,    0,
  145,  145,  145,  145,  145,  145,   88,   88,   88,   88,
   88,    0,    0,    0,    0,    0,   88,    0,   88,    0,
    0,   88,    0,   88,   88,   88,   88,   88,    0,    0,
    0,    0,   88,    0,   88,   88,   88,   88,   88,   88,
  156,  156,  156,  156,  156,    0,    0,    0,    0,    0,
  156,  156,    0,  156,  156,  156,    0,  156,  156,  156,
  156,  156,    0,    0,    0,  156,    0,  156,  158,  158,
  158,  158,  158,    0,    0,    0,    0,    0,  158,  158,
    0,  158,  158,    0,    0,  158,  158,  158,  158,  158,
    0,    0,    0,  158,    0,  158,  157,  157,  157,  157,
  157,    0,    0,    0,    0,    0,  157,  157,    0,  157,
  157,    0,    0,  157,  157,  157,  157,  157,  161,  161,
    0,  157,    0,  157,  166,  166,  166,  166,  166,  161,
    0,  161,  161,    0,  166,  166,    0,  166,  166,    0,
    0,  166,  166,  161,  166,  161,    0,    0,    0,  166,
    0,  166,  167,  167,  167,  167,  167,    0,    0,    0,
    0,    0,  167,  167,    0,  167,  167,    0,    0,  167,
  167,    0,  167,    0,    0,    0,    0,  167,    0,  167,
  154,  154,  154,  154,  154,  155,  155,  155,  155,  155,
  154,  154,    0,  154,  154,  155,  155,  154,  155,  155,
    0,    0,  155,    0,    0,  154,    0,  154,    0,    0,
  155,    0,  155,  163,  163,  163,  163,  163,  164,  164,
  164,  164,  164,    0,  163,    0,  163,  163,    0,  164,
    0,  164,  164,  165,  165,  165,  165,  165,  163,    0,
  163,    0,    0,  164,  165,  164,  165,  165,    0,    0,
    0,    0,    0,    0,  124,    0,  125,    0,  165,  126,
  165,  127,  128,  129,  130,  131,    0,    0,    0,    0,
  132,    0,  133,  134,  135,  136,  137,  138,  125,    0,
    0,  126,    0,    0,  128,  129,  130,  131,    0,    0,
    0,    0,  132,    0,  133,  134,  135,  136,  137,  138,
  125,    0,    0,  126,    0,    0,    0,  129,    0,  131,
    0,    0,    0,    0,  132,    0,  133,  134,  135,  136,
  137,  138,
  };
  protected static  short [] yyCheck = {             1,
    0,   98,   12,   98,  264,  194,  272,  273,  236,  272,
  273,  289,  306,  241,  259,  309,  291,  259,  312,  294,
  285,  286,  291,    1,  289,  294,  304,    5,  304,    1,
  259,  304,  260,  306,   12,  224,  264,  303,  291,  305,
  303,  294,  302,  285,  286,  291,  304,  289,  294,  238,
   28,  280,  281,  282,  283,  284,  285,  286,  287,  288,
  289,  290,  294,  292,  272,  273,  295,    1,  297,  298,
  299,  300,  301,  304,  263,  291,  260,  261,  294,  308,
  309,  310,  311,  312,  313,  304,  304,  259,  260,  261,
  262,  275,  276,  265,  266,  267,  268,  263,  270,  271,
  304,  196,  274,  275,  276,  277,  278,  279,  307,  281,
  285,  286,  287,  288,  289,  125,  213,  304,  294,  306,
   98,  304,  291,  306,  296,  264,   98,  306,  291,  294,
  289,  305,  304,  306,  231,  305,  282,  283,  284,  291,
  302,  302,  302,  291,  290,  269,  292,  125,  305,  295,
  303,  297,  298,  299,  300,  301,  294,  293,  160,  303,
  306,  139,  308,  309,  310,  311,  312,  313,  146,  303,
  305,  293,  259,  260,  261,  294,  294,  305,  265,  266,
  267,  268,  160,  270,  271,  292,  164,  274,  275,  276,
  277,  278,  279,  294,  281,  294,  302,  305,  259,  306,
  305,  308,  309,  310,  311,  312,  313,  303,  302,  296,
  303,  213,  190,  303,  305,  231,    1,  304,  196,    1,
  227,   28,  225,  225,  196,  106,  164,  205,  196,  231,
  245,  242,   -1,    8,  200,  213,  214,   -1,   -1,   -1,
   -1,   -1,  214,   18,   19,   20,   -1,  225,   -1,  227,
   -1,   -1,   -1,  231,   -1,   -1,   -1,   -1,  236,  259,
  260,  261,  262,  241,  236,  265,  266,  267,  268,  241,
  270,  271,  272,  273,  274,  275,  276,  277,  278,  279,
  214,  281,  260,   -1,  280,  281,  264,   -1,  260,   -1,
   -1,   -1,  264,   -1,   -1,  291,  296,  293,  294,   -1,
   -1,   -1,  236,  303,  304,  305,   -1,  241,   -1,  305,
   -1,  307,   -1,   -1,   -1,   -1,   91,   -1,   -1,   -1,
   -1,   -1,   97,   -1,   99,   -1,  260,  102,   -1,   -1,
  264,  106,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  114,
  115,   -1,   -1,  118,  119,  120,  121,  122,  123,  124,
   -1,  126,  127,  128,  129,  130,  131,  132,  133,  134,
  135,  136,  137,  138,   -1,   -1,   -1,   -1,  280,  281,
  259,  260,  261,   -1,   -1,   -1,   -1,  152,   -1,  291,
   -1,  293,  294,   -1,  159,  274,  275,  276,  277,  278,
  279,   -1,  281,  305,   -1,  307,   -1,   -1,   -1,   -1,
  292,   -1,   -1,  295,   -1,   -1,   -1,  296,   -1,   -1,
   -1,   -1,   -1,   -1,  306,  304,  308,  309,  310,  311,
  312,  313,  197,   -1,   -1,  200,  280,  281,  282,  283,
  284,   -1,   -1,   -1,  209,   -1,  290,  291,  292,  293,
  294,  295,   -1,  297,  298,  299,  300,  301,   -1,   -1,
   -1,  305,  306,  307,  308,  309,  310,  311,  312,  313,
  280,  281,  282,  283,  284,   -1,   -1,   -1,   -1,   -1,
  290,  291,  292,  293,  294,  295,   -1,  297,  298,  299,
  300,  301,   -1,   -1,   -1,  305,  306,  307,  308,  309,
  310,  311,  312,  313,  280,  281,  282,  283,  284,   -1,
   -1,   -1,   -1,   -1,  290,  291,  292,  293,  294,  295,
   -1,  297,  298,  299,  300,  301,   -1,   -1,   -1,  305,
   -1,  307,  308,  309,  310,  311,  312,  313,  280,  281,
  282,  283,  284,   -1,   -1,   -1,   -1,   -1,  290,  291,
  292,  293,  294,  295,   -1,  297,  298,  299,  300,  301,
   -1,   -1,   -1,  305,   -1,  307,  308,  309,  310,  311,
  312,  313,  280,  281,  282,  283,  284,   -1,   -1,   -1,
   -1,   -1,  290,  291,  292,  293,  294,  295,   -1,  297,
  298,  299,  300,  301,   -1,   -1,   -1,  305,   -1,  307,
  308,   -1,  310,  311,   -1,  313,  280,  281,  282,  283,
  284,   -1,   -1,   -1,   -1,   -1,  290,  291,  292,  293,
  294,  295,   -1,  297,  298,  299,  300,  301,   -1,   -1,
   -1,  305,   -1,  307,  308,   -1,  310,  311,   -1,  313,
  280,  281,  282,  283,  284,   -1,   -1,   -1,   -1,   -1,
  290,  291,  292,  293,  294,  295,   -1,  297,  298,  299,
  300,  301,   -1,   -1,   -1,  305,   -1,  307,  308,   -1,
  310,  311,   -1,  313,  280,  281,  282,  283,  284,   -1,
   -1,   -1,   -1,   -1,  290,  291,  292,  293,  294,  295,
   -1,  297,  298,  299,  300,  301,   -1,   -1,   -1,  305,
   -1,  307,  308,   -1,  310,  311,   -1,  313,  280,  281,
  282,  283,  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,
  292,   -1,   -1,  295,   -1,  297,  298,  299,  300,  301,
   -1,   -1,   -1,  305,  306,   -1,  308,  309,  310,  311,
  312,  313,  280,  281,  282,  283,  284,   -1,   -1,   -1,
   -1,   -1,  290,   -1,  292,   -1,   -1,  295,   -1,  297,
  298,  299,  300,  301,   -1,   -1,   -1,  305,  306,   -1,
  308,  309,  310,  311,  312,  313,  280,  281,  282,  283,
  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,  292,   -1,
   -1,  295,   -1,  297,  298,  299,  300,  301,   -1,   -1,
   -1,  305,  306,   -1,  308,  309,  310,  311,  312,  313,
  280,  281,  282,  283,  284,   -1,   -1,   -1,   -1,   -1,
  290,   -1,  292,   -1,   -1,  295,   -1,  297,  298,  299,
  300,  301,   -1,   -1,   -1,  305,  306,   -1,  308,  309,
  310,  311,  312,  313,  280,  281,  282,  283,  284,   -1,
   -1,   -1,   -1,   -1,  290,  291,  292,   -1,   -1,  295,
   -1,  297,  298,  299,  300,  301,   -1,   -1,   -1,   -1,
  306,   -1,  308,  309,  310,  311,  312,  313,  280,  281,
  282,  283,  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,
  292,  293,   -1,  295,   -1,  297,  298,  299,  300,  301,
   -1,   -1,   -1,   -1,  306,   -1,  308,  309,  310,  311,
  312,  313,  280,  281,  282,  283,  284,   -1,   -1,   -1,
   -1,   -1,  290,   -1,  292,   -1,   -1,  295,   -1,  297,
  298,  299,  300,  301,   -1,   -1,   -1,   -1,  306,  307,
  308,  309,  310,  311,  312,  313,  280,  281,  282,  283,
  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,  292,   -1,
   -1,  295,   -1,  297,  298,  299,  300,  301,   -1,   -1,
   -1,   -1,  306,  307,  308,  309,  310,  311,  312,  313,
  280,  281,  282,  283,  284,   -1,   -1,   -1,   -1,   -1,
  290,   -1,  292,   -1,  294,  295,   -1,  297,  298,  299,
  300,  301,   -1,   -1,   -1,   -1,  306,   -1,  308,  309,
  310,  311,  312,  313,  280,  281,  282,  283,  284,   -1,
   -1,   -1,   -1,   -1,  290,   -1,  292,   -1,  294,  295,
   -1,  297,  298,  299,  300,  301,   -1,   -1,   -1,   -1,
  306,   -1,  308,  309,  310,  311,  312,  313,  280,  281,
  282,  283,  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,
  292,   -1,   -1,  295,   -1,  297,  298,  299,  300,  301,
   -1,   -1,   -1,   -1,  306,   -1,  308,  309,  310,  311,
  312,  313,  280,  281,  282,  283,  284,   -1,   -1,   -1,
   -1,   -1,  290,   -1,  292,   -1,   -1,  295,   -1,  297,
  298,  299,  300,  301,   -1,   -1,   -1,   -1,  306,   -1,
  308,  309,  310,  311,  312,  313,  280,  281,  282,  283,
  284,   -1,   -1,   -1,   -1,   -1,  290,   -1,  292,   -1,
   -1,  295,   -1,  297,  298,  299,  300,  301,   -1,   -1,
   -1,   -1,  306,   -1,  308,  309,  310,  311,  312,  313,
  280,  281,  282,  283,  284,   -1,   -1,   -1,   -1,   -1,
  290,  291,   -1,  293,  294,  295,   -1,  297,  298,  299,
  300,  301,   -1,   -1,   -1,  305,   -1,  307,  280,  281,
  282,  283,  284,   -1,   -1,   -1,   -1,   -1,  290,  291,
   -1,  293,  294,   -1,   -1,  297,  298,  299,  300,  301,
   -1,   -1,   -1,  305,   -1,  307,  280,  281,  282,  283,
  284,   -1,   -1,   -1,   -1,   -1,  290,  291,   -1,  293,
  294,   -1,   -1,  297,  298,  299,  300,  301,  280,  281,
   -1,  305,   -1,  307,  280,  281,  282,  283,  284,  291,
   -1,  293,  294,   -1,  290,  291,   -1,  293,  294,   -1,
   -1,  297,  298,  305,  300,  307,   -1,   -1,   -1,  305,
   -1,  307,  280,  281,  282,  283,  284,   -1,   -1,   -1,
   -1,   -1,  290,  291,   -1,  293,  294,   -1,   -1,  297,
  298,   -1,  300,   -1,   -1,   -1,   -1,  305,   -1,  307,
  280,  281,  282,  283,  284,  280,  281,  282,  283,  284,
  290,  291,   -1,  293,  294,  290,  291,  297,  293,  294,
   -1,   -1,  297,   -1,   -1,  305,   -1,  307,   -1,   -1,
  305,   -1,  307,  280,  281,  282,  283,  284,  280,  281,
  282,  283,  284,   -1,  291,   -1,  293,  294,   -1,  291,
   -1,  293,  294,  280,  281,  282,  283,  284,  305,   -1,
  307,   -1,   -1,  305,  291,  307,  293,  294,   -1,   -1,
   -1,   -1,   -1,   -1,  290,   -1,  292,   -1,  305,  295,
  307,  297,  298,  299,  300,  301,   -1,   -1,   -1,   -1,
  306,   -1,  308,  309,  310,  311,  312,  313,  292,   -1,
   -1,  295,   -1,   -1,  298,  299,  300,  301,   -1,   -1,
   -1,   -1,  306,   -1,  308,  309,  310,  311,  312,  313,
  292,   -1,   -1,  295,   -1,   -1,   -1,  299,   -1,  301,
   -1,   -1,   -1,   -1,  306,   -1,  308,  309,  310,  311,
  312,  313,
  };

#line 730 "ProcessingParser.jay"
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
  public const int ERROR = 257;
  public const int EOF = 258;
  public const int IDENTIFIER = 259;
  public const int NUMERIC_LITERAL = 260;
  public const int STRING_LITERAL = 261;
  public const int CLASS = 262;
  public const int EXTENDS = 263;
  public const int IMPLEMENTS = 264;
  public const int WHILE = 265;
  public const int BREAK = 266;
  public const int RETURN = 267;
  public const int IF = 268;
  public const int ELSE = 269;
  public const int FOR = 270;
  public const int SWITCH = 271;
  public const int CASE = 272;
  public const int DEFAULT = 273;
  public const int NEW = 274;
  public const int TRUE = 275;
  public const int FALSE = 276;
  public const int THIS = 277;
  public const int SUPER = 278;
  public const int NULL = 279;
  public const int PLUS = 280;
  public const int MINUS = 281;
  public const int ASTERISK = 282;
  public const int SLASH = 283;
  public const int PERCENT = 284;
  public const int PLUS_EQUAL = 285;
  public const int MINUS_EQUAL = 286;
  public const int PLUS_PLUS = 287;
  public const int MINUS_MINUS = 288;
  public const int EQUAL = 289;
  public const int EQUAL2 = 290;
  public const int COMMA = 291;
  public const int DOT = 292;
  public const int COLON = 293;
  public const int SEMICOLON = 294;
  public const int QUESTION = 295;
  public const int EXCLAIM = 296;
  public const int EXCLAIM_EQUAL = 297;
  public const int AND = 298;
  public const int AND2 = 299;
  public const int BAR = 300;
  public const int BAR2 = 301;
  public const int OPEN_CURLY = 302;
  public const int CLOSE_CURLY = 303;
  public const int OPEN_PAREN = 304;
  public const int CLOSE_PAREN = 305;
  public const int OPEN_BRACE = 306;
  public const int CLOSE_BRACE = 307;
  public const int OPEN_ANGLE = 308;
  public const int OPEN_ANGLE2 = 309;
  public const int OPEN_ANGLE_EQUAL = 310;
  public const int CLOSE_ANGLE = 311;
  public const int CLOSE_ANGLE2 = 312;
  public const int CLOSE_ANGLE_EQUAL = 313;
  public const int PLUS2 = 314;
  public const int MINUS2 = 315;
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
