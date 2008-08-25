using System;
using System.Collections.Generic;
using System.Reflection;
using Irony.Compiler;

namespace ProcessingDLR
{
	public class ProcessingGrammar : Grammar
	{
		static readonly Terminal
			numeric_literal = new NumberLiteral ("number"),
			string_literal = new StringLiteral ("string literal", TermOptions.None),
			identifier = new IdentifierTerminal ("identifier")
			;

		readonly NonTerminal top_level_contents,
			top_level_content,
			global_function_definition,
			class_declaration,
			opt_derivation_indication,
			opt_implementation_indications,
			derivation_indication,
			implementation_indications,
			identifier_list_comma,
			class_member_definitions ,
			class_member_definition ,
			field_definition ,
			constructor_definition ,
			function_definition ,
			function_definition_base ,
			argument_definitions ,
			argument_definition ,
			statement_block ,
			statements ,
			statement ,
			statement_with_semicolon ,
			statement_without_semicolon ,
			abstract_assignment_statement ,
			add_assignment_statement ,
			subtract_assignment_statement ,
			increment_statement ,
			decrement_statement ,
			call_super_statement ,
			return_statement ,
			assignment_statement ,
			while_statement ,
			break_statement ,
			variable_declaration_statement ,
			type_name ,
			opt_array_type_indicator ,
			variable_declaration_pairs ,
			variable_declaration_pair ,
			opt_variable_initializer ,
			variable_initializer ,
			if_statement ,
			opt_else_block ,
			else_block ,
			for_statement ,
			for_initializer ,
			abstract_assignment_list ,
			for_continuations ,
			switch_statement ,
			switch_case_list ,
			switch_case_default ,
			switch_case ,
			switch_default ,
			expression ,
			variable ,
			constant ,
			literal ,
			new_expression ,
			new_object_expression ,
			new_array_expression ,
			callable_expression ,
			function_call_expression ,
			function_call_base ,
			function_args ,
			function_arg_list ,
			operation_expression ,
			comparison_expression ,
			logical_operation_expression ,
			arithmetic_expression ,
			additive_expression ,
			multiplicative_expression ,
			field_access_expression ,
			function_access_expression ,
			super_expression ,
			false_expression ,
			this_expression ,
			true_expression ,
			array_access_expression ,
			null_expression ,
			parenthesized_expression ,
			less_than_expression ,
			less_eq_expression ,
			greater_than_expression ,
			greater_eq_expression ,
			eq_expression ,
			not_eq_expression ,
			conditional_expression ,
			logical_or_expression ,
			logical_and_expression ,
			logical_not_expression ,
			add_expression ,
			subtract_expression ,
			numeric_negation_expression ,
			multiply_expression ,
			divide_expression ,
			modulo_expression ,
			bitwise_and_expression ,
			bitwise_or_expression ,
			left_shift_expression ,
			right_shift_expression
			;

		static readonly SymbolTerminal
			symbol_super = Symbol ("super"),
			symbol_false = Symbol ("false"),
			symbol_true = Symbol ("true"),
			symbol_this = Symbol ("this"),
			symbol_null = Symbol ("null"),
			symbol_class = Symbol ("class"),
			symbol_extends = Symbol ("extends")
			;

		public ProcessingGrammar ()
		{
			NonGrammarTerminals.Add (new CommentTerminal ("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029"));
			NonGrammarTerminals.Add (new CommentTerminal("DelimitedComment", "/*", "*/"));

			// am lazy ;-)
			foreach (FieldInfo fi in GetType ().GetFields (BindingFlags.NonPublic | BindingFlags.Instance))
				if (fi.FieldType.IsAssignableFrom (typeof (NonTerminal)))
					if (fi.GetValue (this) == null)
						fi.SetValue (this, new NonTerminal (fi.Name.Replace ("_", "-")));

			this.Root = top_level_contents;

			top_level_contents.Rule =
				Empty
				| top_level_content + top_level_contents
				;

			top_level_content.Rule =
				statement
				| callable_expression
				| class_declaration
				| global_function_definition
				;

			global_function_definition.Rule = function_definition ;

			class_declaration.Rule =
				symbol_class + identifier +
				opt_derivation_indication +
				opt_implementation_indications +
				"{" + class_member_definitions + "}"
				;

			opt_derivation_indication.Rule =
				Empty
				| derivation_indication
				;

			opt_implementation_indications.Rule =
				Empty
				| implementation_indications
				;

			derivation_indication.Rule = symbol_extends + identifier ;
			implementation_indications.Rule = Symbol ("implements") + identifier_list_comma ;

			identifier_list_comma.Rule =
				identifier
				| identifier + "," + identifier_list_comma ;

			class_member_definitions.Rule =
				Empty
				| class_member_definition + class_member_definitions ;

			class_member_definition.Rule =
				field_definition
				| constructor_definition
				| function_definition
				;

			field_definition.Rule = variable_declaration_statement + ";" ;

			constructor_definition.Rule = function_definition_base ;

			function_definition.Rule =
				type_name + function_definition_base ;

			function_definition_base.Rule =
				identifier + "(" + argument_definitions + ")" + statement_block ;

			argument_definitions.Rule =
				Empty
				| argument_definition + "," + argument_definitions ;

			argument_definition.Rule =
				type_name + identifier ;

			// statements

			statement_block.Rule = Symbol ("{") + statements + "}" ;

			statements.Rule =
				Empty
				| statement + statements ;

			statement.Rule =
				statement_with_semicolon + ";"
				| statement_without_semicolon ;

			statement_with_semicolon.Rule =
				return_statement
				| abstract_assignment_statement
				| break_statement
				| increment_statement
				| decrement_statement
				| variable_declaration_statement
				| call_super_statement
				;

			statement_without_semicolon.Rule =
				if_statement
				| for_statement
				| while_statement
				| switch_statement
				;

			abstract_assignment_statement.Rule =
				assignment_statement
				| add_assignment_statement
				| subtract_assignment_statement
				;

			add_assignment_statement.Rule = expression + "+=" + expression ;
			subtract_assignment_statement.Rule = expression + "-=" + expression ;
			increment_statement.Rule = expression + "++" ;
				// FIXME: no pre-positioned increment?
			decrement_statement.Rule = expression + "--" ;
				// FIXME: no pre-positioned increment?
			call_super_statement.Rule = Symbol ("super") + "(" + function_args + ")" ;
				// valid only in constructor
			return_statement.Rule = Symbol ("return") + expression ;
			assignment_statement.Rule = variable + "=" + expression ;
			while_statement.Rule = Symbol ("while") + "(" + expression + ")" + statement_block ;
			break_statement.Rule = Symbol ("break");

			// variable-declaration

			variable_declaration_statement.Rule =
				type_name + opt_array_type_indicator + variable_declaration_pairs + ";" ;

			type_name.Rule = identifier ;

			opt_array_type_indicator.Rule =
				Empty
				| Symbol ("[") + "]" ;

			variable_declaration_pairs.Rule =
				variable_declaration_pair
				| variable_declaration_pair + "," + variable_declaration_pairs ;

			variable_declaration_pair.Rule =
				identifier + opt_variable_initializer ;

			opt_variable_initializer.Rule =
				Empty
				| variable_initializer ;

			variable_initializer.Rule =
				Symbol ("=") + expression ;

			// if-else

			if_statement.Rule =
				Symbol ("if") + "(" + expression + ")" + statement_block + opt_else_block ;

			opt_else_block.Rule =
				Empty
				| else_block ;

			else_block.Rule = Symbol ("else") + statement_block ;

			// for

			for_statement.Rule =
				Symbol ("for") + "(" + for_initializer + ";" + expression + ";" + for_continuations + ")" + statement_block ;

			for_initializer.Rule =
				Empty
				| variable_declaration_statement
				| abstract_assignment_list ;

			abstract_assignment_list.Rule =
				abstract_assignment_statement
				| abstract_assignment_statement + "," + abstract_assignment_list ;

			for_continuations.Rule = statements ; // FIXME: no constraints?

			// switch-case

			switch_statement.Rule = Symbol ("switch") + "(" + expression + ")" + "{" + switch_case_list + "}" ;

			switch_case_list.Rule =
				Empty
				| switch_case_default + switch_case_list ;
					// switch_default cannot appear twice

			switch_case_default.Rule =
				switch_case
				| switch_default ;

			switch_case.Rule = Symbol ("case") + constant + ":" + statements ;
				// must end with break_statement
			switch_default.Rule = Symbol ("default") + ":" + statements ;
				// must end with break_statement

			// expressions

			expression.Rule =
				callable_expression
				| operation_expression
				| variable
				| literal
				| field_access_expression
				| super_expression
				| this_expression
				| new_expression
				| array_access_expression
				| null_expression
				| parenthesized_expression
				| numeric_negation_expression
				;

			variable.Rule = identifier ;

			literal.Rule =
				string_literal
				| numeric_literal
				| true_expression
				| false_expression
				;

			// new

			new_expression.Rule =
				new_object_expression
				| new_array_expression
				;

			new_object_expression.Rule = Symbol ("new") + function_call_base ;

			new_array_expression.Rule = Symbol ("new") + identifier + "[" + expression + "]" ;

			// callable expressions

			callable_expression.Rule =
				function_call_expression
				| function_access_expression
				;

			function_call_expression.Rule = function_call_base ;

			function_call_base.Rule =
				identifier + "(" + function_args + ")" ;

			function_args.Rule =
				Empty
				| function_arg_list ;

			function_arg_list.Rule =
				expression
				| expression + "," + function_arg_list ;

			// operation expressions

			operation_expression.Rule =
				comparison_expression
				| logical_operation_expression
				| arithmetic_expression
				;

			comparison_expression.Rule =
				less_than_expression
				| less_eq_expression
				| greater_than_expression
				| greater_eq_expression
				| eq_expression
				| not_eq_expression
				| conditional_expression
				;

			logical_operation_expression.Rule =
				logical_or_expression
				| logical_and_expression
				| logical_not_expression
				;

			arithmetic_expression.Rule =
				additive_expression
				| multiplicative_expression
				;

			additive_expression.Rule =
				add_expression
				| subtract_expression
				| bitwise_and_expression
				| bitwise_or_expression
				| left_shift_expression
				| right_shift_expression
				;

			multiplicative_expression.Rule =
				multiply_expression
				| divide_expression
				| modulo_expression
				;

			// FIXME: "expression" in below items need to be more specific.
			// For example, First expression in "additive_expression" is 
			// "multiplicative_expression" (for operator priority)

			field_access_expression.Rule = expression + "." + identifier ;
			function_access_expression.Rule = expression + "." + function_call_expression ;
			super_expression.Rule = symbol_super;
			false_expression.Rule = symbol_false;
			this_expression.Rule = symbol_this;
			true_expression.Rule = symbol_true;
			array_access_expression.Rule = expression + "[" + expression + "]" ;
			null_expression.Rule = symbol_null;
			parenthesized_expression.Rule = Symbol ("(") + expression + ")" ;
			less_than_expression.Rule = expression + "<" + expression;
			less_eq_expression.Rule = expression + "<=" + expression;
			greater_than_expression.Rule = expression + ">" + expression;
			greater_eq_expression.Rule = expression + ">=" + expression;
			eq_expression.Rule = expression + "==" + expression;
			not_eq_expression.Rule = expression + "!=" + expression;
			conditional_expression.Rule = expression + "?" + expression + ":" + expression;
			logical_or_expression.Rule = expression + "||" + expression;
			logical_and_expression.Rule = expression + "&&" + expression;
			logical_not_expression.Rule = Symbol ("!") + expression;
			add_expression.Rule = expression + "+" + expression;
			subtract_expression.Rule = expression + "-" + expression;
			numeric_negation_expression.Rule = Symbol ("-") + expression;
			multiply_expression.Rule = expression + "*" + expression;
			divide_expression.Rule = expression + "/" + expression;
			modulo_expression.Rule = expression + "%" + expression;
			bitwise_and_expression.Rule = expression + "&" + expression;
			bitwise_or_expression.Rule = expression + "|" + expression;
			left_shift_expression.Rule = expression + "<<" + expression;
			right_shift_expression.Rule = expression + ">>" + expression;
		}

		public override void OnActionSelected(Parser parser, Token input, ActionRecord action)
		{
			Console.WriteLine ("OnActionSelected: {0} / {1}", input, action);
		}

		public override ActionRecord OnActionConflict (Parser parser, Token input, ActionRecord action)
		{
			throw new Exception (String.Format ("Action conflict: {0} {1}", ((AstNode) input).Location, action));
		}
	}
}
