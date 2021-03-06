﻿using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;

namespace LogoSharp
{
    [Language("Logo Sharp", "1.0", "Logo programming language for managed world.")]
    public class LogoGrammar : Grammar
    {
        public LogoGrammar()
            : base(false)
        {
            // Literals
            var decimal_number = new NumberLiteral("DECIMAL")
            {
                // integer_number.DefaultIntTypes = new[] { TypeCode.Int16, TypeCode.Int32, TypeCode.Int64 };
                DefaultFloatType = TypeCode.Single
            };

            // identifiers
            var identifier = new IdentifierTerminal("IDENTIFIER");

            // comments
            var COMMENT = new CommentTerminal("COMMENT", ";", "\r", "\n", "\u2085", "\u2028", "\u2029");
            NonGrammarTerminals.Add(COMMENT);

            // Non terminals
            // 10. Expressions
            var VARIABLE = new NonTerminal("VARIABLE");
            var VARIABLE_REF = new NonTerminal("VARIABLE_REF");
            var EXPRESSION = new NonTerminal("EXPRESSION");
            var BINARY_OPERATOR = new NonTerminal("BINARY_OPERATOR");
            var BINARY_EXPRESSION = new NonTerminal("BINARY_EXPRESSION", typeof(BinaryOperationNode));
            var UNARY_OPERATOR = new NonTerminal("UNARY_OPERATOR");
            var UNARY_EXPRESSION = new NonTerminal("UNARY_EXPRESSION");
            var REP_COUNT_EXPRESSION = new NonTerminal("REP_COUNT");

            // 15. Assignments
            var ASSIGNMENT = new NonTerminal("ASSIGNMENT");

            // 20. Procedures
            var PROCEDURE_DECLARE = new NonTerminal("PROCEDURE_DECLARE");
            var PROCEDURE_BODY = new NonTerminal("PROCEDURE_BODY");
            var PROCEDURE_END = new NonTerminal("PROCEDURE_END");
            var PROCEDURE = new NonTerminal("PROCEDURE");
            var PROCEDURE_CALL = new NonTerminal("PROCEDURE_CALL");
            var PROCEDURE_CALL_ARGUMENTS = new NonTerminal("PROCEDURE_CALL_ARGUMENTS");
            var PROCEDURE_ARGUMENT = new NonTerminal("PROCEDURE_ARGUMENT");
            var PROCEDURE_ARGUMENTS = new NonTerminal("PROCEDURE_ARGUMENTS");

            // 30. Functions
            var FUNCTION_CALL = new NonTerminal("FUNCTION_CALL");
            var FUNCTION_CALL_ARGUMENTS = new NonTerminal("FUNCTION_CALL_ARGUMENTS");

            // 50. Basic
            var LSB = new NonTerminal("LEFT_SQUARE_BRACKET");
            var RSB = new NonTerminal("RIGHT_SQUARE_BRACKET");
            var LPS = new NonTerminal("LEFT_PARENTHESIS");
            var RPS = new NonTerminal("RIGHT_PARENTHESIS");

            // 100. Program
            var PROGRAM = new NonTerminal("PROGRAM");

            // 150. Tuples
            var TUPLE = new NonTerminal("TUPLE");
            var TUPLEARGS = new NonTerminal("TUPLEARGS");

            // 200. Drawing commands
            var LT = new NonTerminal("LEFT");
            var RT = new NonTerminal("RIGHT");
            var BK = new NonTerminal("BACKWARD");
            var FD = new NonTerminal("FORWARD");
            var ARC = new NonTerminal("ARC");
            var DELAY = new NonTerminal("DELAY");
            var DRAW = new NonTerminal("DRAW");
            var HOME = new NonTerminal("HOME");
            var SHOW_TURTLE = new NonTerminal("SHOWTURTLE");
            var HIDE_TURTLE = new NonTerminal("HIDETURTLE");
            var DRAWING_COMMAND = new NonTerminal("DRAWING_COMMAND");
            var BASIC_CONTROL_COMMAND = new NonTerminal("BASIC_CONTROL_COMMAND");

            // 250. Pen commands
            var COLOR_VALUE = new NonTerminal("COLOR_VALUE");
            var COLOR_VALUE_TUPLE = new NonTerminal("COLOR_VALUE_TUPLE");

            var PC = new NonTerminal("PEN_COLOR");
            var PE = new NonTerminal("PEN_ERASE");
            var PN = new NonTerminal("PEN_NORMAL");
            var PEN_DOWN = new NonTerminal("PEN_DOWN");
            var PEN_UP = new NonTerminal("PEN_UP");
            var SET_PEN_SIZE = new NonTerminal("SET_PEN_SIZE");
            var SET_PEN_COLOR = new NonTerminal("SET_PEN_COLOR");
            var PEN_COMMAND = new NonTerminal("PEN_COMMAND");

            var BASIC_COMMAND = new NonTerminal("BASIC_COMMAND");

            // 300. Flow Controls
            var REPEAT_COMMAND = new NonTerminal("REPEAT_COMMAND");
            var REPEAT_BODY = new NonTerminal("REPEAT_BODY");

            var FLOW_CONTROL_COMMAND = new NonTerminal("FLOW_CONTROL_COMMAND");

            // 350. Final command
            var COMMAND_LINE = new NonTerminal("COMMAND_LINE");
            var COMMAND = new NonTerminal("COMMAND");

            LSB.Rule = ToTerm("[");
            RSB.Rule = ToTerm("]");
            LPS.Rule = ToTerm("(");
            RPS.Rule = ToTerm(")");

            TUPLE.Rule = MakeStarRule(TUPLE, EXPRESSION);
            TUPLEARGS.Rule = LSB + TUPLE + RSB;

            VARIABLE.Rule = "\"" + identifier;
            VARIABLE_REF.Rule = ":" + identifier;

            REP_COUNT_EXPRESSION.Rule = ToTerm("REPCOUNT");

            EXPRESSION.Rule = decimal_number |
                VARIABLE_REF | FUNCTION_CALL |
                REP_COUNT_EXPRESSION |
                BINARY_EXPRESSION |
                "(" + EXPRESSION + ")" |
                UNARY_EXPRESSION;

            UNARY_OPERATOR.Rule = ToTerm("-") | "+";
            UNARY_EXPRESSION.Rule = UNARY_OPERATOR + EXPRESSION;

            BINARY_OPERATOR.Rule = ToTerm("+") | "-" | "*" | "/" | "^";
            BINARY_EXPRESSION.Rule = EXPRESSION + BINARY_OPERATOR + EXPRESSION;

            ASSIGNMENT.Rule = "make" + VARIABLE + EXPRESSION;

            LT.Rule = ToTerm("LT") | ToTerm("LEFT");
            RT.Rule = ToTerm("RT") | ToTerm("RIGHT");
            FD.Rule = ToTerm("FD") | ToTerm("FORWARD");
            BK.Rule = ToTerm("BK") | ToTerm("BACKWARD") | ToTerm("BACK");
            ARC.Rule = ToTerm("ARC") | ToTerm("ARC2");
            DELAY.Rule = ToTerm("DELAY");
            DRAW.Rule = ToTerm("CLS") | ToTerm("DRAW") | ToTerm("CLEARSCR") | ToTerm("CLEARSCREEN") | ToTerm("CS");
            HOME.Rule = ToTerm("HOME");
            SHOW_TURTLE.Rule = ToTerm("SHOWTURTLE") | ToTerm("ST");
            HIDE_TURTLE.Rule = ToTerm("HIDETURTLE") | ToTerm("HT");
            DRAWING_COMMAND.Rule = LT + EXPRESSION |
                RT + EXPRESSION |
                FD + EXPRESSION |
                BK + EXPRESSION |
                ARC + EXPRESSION |
                DELAY + decimal_number | DRAW;
            BASIC_CONTROL_COMMAND.Rule = HOME | SHOW_TURTLE | HIDE_TURTLE;

            COLOR_VALUE_TUPLE.Rule = TUPLEARGS;
            COLOR_VALUE.Rule = COLOR_VALUE_TUPLE | EXPRESSION;

            PC.Rule = ToTerm("SETPENCOLOR") | "SETPC" | "PC";
            PE.Rule = ToTerm("PENERASE") | "PE";
            PN.Rule = ToTerm("PENNORMAL") | "PN";
            PEN_DOWN.Rule = ToTerm("PD") | "PENDOWN";
            PEN_UP.Rule = ToTerm("PU") | "PENUP";
            SET_PEN_SIZE.Rule = ToTerm("SETPENSIZE") + TUPLEARGS;
            SET_PEN_COLOR.Rule = PC + COLOR_VALUE;
            PEN_COMMAND.Rule = PEN_DOWN | PEN_UP | SET_PEN_COLOR | SET_PEN_SIZE | PE | PN;

            REPEAT_BODY.Rule = MakeStarRule(REPEAT_BODY, COMMAND_LINE);
            REPEAT_COMMAND.Rule = ToTerm("REPEAT") + EXPRESSION + LSB + REPEAT_BODY + RSB;

            BASIC_COMMAND.Rule = DRAWING_COMMAND | PEN_COMMAND | BASIC_CONTROL_COMMAND;
            FLOW_CONTROL_COMMAND.Rule = REPEAT_COMMAND;

            COMMAND_LINE.Rule = BASIC_COMMAND | FLOW_CONTROL_COMMAND | ASSIGNMENT | PROCEDURE_CALL;
            COMMAND.Rule = COMMAND_LINE | Empty + NewLine;

            PROCEDURE_ARGUMENT.Rule = VARIABLE_REF;
            PROCEDURE_ARGUMENTS.Rule = MakeStarRule(PROCEDURE_ARGUMENTS, PROCEDURE_ARGUMENT);

            PROCEDURE_DECLARE.Rule = ToTerm("TO") + identifier + PROCEDURE_ARGUMENTS + NewLine;
            PROCEDURE_BODY.Rule = MakeStarRule(PROCEDURE_BODY, COMMAND);
            PROCEDURE_END.Rule = ToTerm("END") + NewLine;
            PROCEDURE.Rule = PROCEDURE_DECLARE + PROCEDURE_BODY + PROCEDURE_END;

            PROCEDURE_CALL_ARGUMENTS.Rule = MakeStarRule(PROCEDURE_CALL_ARGUMENTS, EXPRESSION);
            PROCEDURE_CALL.Rule = identifier  + PROCEDURE_CALL_ARGUMENTS;

            FUNCTION_CALL_ARGUMENTS.Rule = MakeStarRule(FUNCTION_CALL_ARGUMENTS, EXPRESSION);
            FUNCTION_CALL.Rule = "{" + identifier + FUNCTION_CALL_ARGUMENTS + "}";

            var COMMANDS = new NonTerminal("COMMANDS");
            COMMANDS.Rule = MakePlusRule(COMMANDS, COMMAND);

            var PROCEDURES = new NonTerminal("PROCEDURES");
            PROCEDURES.Rule = MakeStarRule(PROCEDURES, PROCEDURE);


            var PROGRAM_BODY = new NonTerminal("PROGRAM_BODY");
            PROGRAM_BODY.Rule = PROCEDURE | COMMAND;

            PROGRAM.Rule = MakePlusRule(PROGRAM, PROGRAM_BODY);

            RegisterOperators(60, "^");
            RegisterOperators(50, "*", "/");
            RegisterOperators(40, "+", "-");

            RegisterBracePair("[", "]");
            RegisterBracePair("(", ")");
            RegisterBracePair("{", "}");

            MarkPunctuation(LPS, RPS);
            MarkPunctuation(LSB, RSB);

            MarkTransient(COMMAND,
                COMMAND_LINE,
                COLOR_VALUE,
                COLOR_VALUE_TUPLE,
                PROCEDURE_ARGUMENT,
                BASIC_COMMAND,
                FLOW_CONTROL_COMMAND,
                TUPLEARGS,
                PROGRAM_BODY,
                BINARY_OPERATOR, 
                UNARY_OPERATOR
                );

            this.Root = PROGRAM;
        }
    }
}
