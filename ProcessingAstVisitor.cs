using System;
using System.Collections.Generic;
using System.Reflection;
using Irony.Compiler;

namespace ProcessingDLR
{
	class ProcessingAstVisitorContext
	{
		public ProcessingAstVisitorContext ()
		{
			ClassContext = new ClassContext ();
		}
		public ClassContext ClassContext { get; set; }
	}

	class ClassContext
	{
		List<string> ifaces = new List<string> ();

		public string BaseClassName { get; set; }
		public List<string> Interfaces {
			get { return ifaces; }
		}
	}

	public class ProcessingAstVisitor : IAstVisitor
	{
		static ProcessingAstVisitor ()
		{
			// FIXME: create automatic node coverage checker
		}

		ProcessingAstVisitorContext ctx = new ProcessingAstVisitorContext ();

		public void BeginVisit (AstNode node)
		{
			switch (node.Term.Name) {
			case "top_level_contents":
			case "top_level_content":

			case "opt_derivation_indication":
			case "opt_implementation_indications":
				return; // thru

			case "global_function_definition":
				throw new NotImplementedException ();

			case "class_declaration":
				throw new NotImplementedException ();

			case "derivation_indication":
				Console.WriteLine (node.ChildNodes [1].GetType ());
				throw new Exception ();
				//ctx.ClassContext.BaseClassName = node.ChildNodes [1].hogehoge;
			case "implementation_indications":
				throw new NotImplementedException ();

			case "identifier_list_comma":
			case "class_member_definitions":
			case "class_member_definition":
			case "field_definition":
			case "constructor_definition":
			case "function_definition":
			case "function_definition_base":
			case "argument_definitions":
			case "argument_definition":
			case "statement_block":
			case "statements":
			case "statement":
			case "statement_with_semicolon":
			case "statement_without_semicolon":
			case "abstract_assignment_statement":
			case "add_assignment_statement":
			case "subtract_assignment_statement":
			case "increment_statement":
			case "decrement_statement":
			case "call_super_statement":
			case "return_statement":
			case "assignment_statement":
			case "while_statement":
			case "break_statement":
			case "variable_declaration_statement":
			case "type_name":
			case "opt_array_type_indicator":
			case "variable_declaration_pairs":
			case "variable_declaration_pair":
			case "opt_variable_initializer":
			case "variable_initializer":
			case "if_statement":
			case "opt_else_block":
			case "else_block":
			case "for_statement":
			case "for_initializer":
			case "abstract_assignment_list":
			case "for_continuations":
			case "switch_statement":
			case "switch_case_list":
			case "switch_case_default":
			case "switch_case":
			case "switch_default":
			case "expression":
			case "variable":
			case "constant":
			case "literal":
			case "new_expression":
			case "new_object_expression":
			case "new_array_expression":
			case "callable_expression":
			case "function_call_expression":
			case "function_call_base":
			case "function_args":
			case "function_arg_list":
			case "operation_expression":
			case "comparison_expression":
			case "logical_operation_expression":
			case "arithmetic_expression":
			case "additive_expression":
			case "multiplicative_expression":
			case "field_access_expression":
			case "function_access_expression":
			case "super_expression":
			case "false_expression":
			case "this_expression":
			case "true_expression":
			case "array_access_expression":
			case "null_expression":
			case "parenthesized_expression":
			case "less_than_expression":
			case "less_eq_expression":
			case "greater_than_expression":
			case "greater_eq_expression":
			case "eq_expression":
			case "not_eq_expression":
			case "conditional_expression":
			case "logical_or_expression":
			case "logical_and_expression":
			case "logical_not_expression":
			case "add_expression":
			case "subtract_expression":
			case "numeric_negation_expression":
			case "multiply_expression":
			case "divide_expression":
			case "modulo_expression":
			case "bitwise_and_expression":
			case "bitwise_or_expression":
			case "left_shift_expression":
			case "right_shift_expression":

			default:
				throw new NotImplementedException ();
			}
		}

		public void EndVisit (AstNode node)
		{
		}
	}
}
