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
            var integer_number = new NumberLiteral("INTEGER", NumberOptions.AllowSign | NumberOptions.IntOnly);
            var decimal_number = new NumberLiteral("DECIMAL", NumberOptions.AllowSign);
            var string_literal = new StringLiteral("STRING");

            integer_number.DefaultIntTypes = new[] { TypeCode.Int16, TypeCode.Int32, TypeCode.Int64 };
            decimal_number.DefaultFloatType = TypeCode.Single;

            // identifiers
            var identifier = new IdentifierTerminal("IDENTIFIER");

            // Non terminals
            // 10. Expressions
            var EXPRESSION = new NonTerminal("EXPRESSION");
            var BINARY_OPERATOR = new NonTerminal("BINARY_OPERATOR");
            var BINARY_EXPRESSION = new NonTerminal("BINARY_EXPRESSION", typeof(BinaryOperationNode));
            var PARENTHESIS_EXPRESSION = new NonTerminal("PARENTHESIS_EXPRESSION");
            var EXPRESSION_FACTOR = new NonTerminal("EXPRESSION_FACTOR");

            // 15. Assignments
            var ASSIGNMENT = new NonTerminal("ASSIGNMENT");

            // 20. Function calls
            var FUNCTION_ARGS = new NonTerminal("FUNCTION_ARGS");
            var FUNCTION_CALL = new NonTerminal("FUNCTION_CALL");

            // 50. Basic
            var LSB = new NonTerminal("LEFT_SQUARE_BRACKET");
            var RSB = new NonTerminal("RIGHT_SQUARE_BRACKET");
            var LPS = new NonTerminal("LEFT_PARENTHESIS");
            var RPS = new NonTerminal("RIGHT_PARENTHESIS");

            // 100. Program
            var PROGRAM = new NonTerminal("PROGRAM");

            // 150. Tuples
            var TUPLE_ELEMENT = new NonTerminal("TUPLE_ELEMENT");
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
            var DRAWING_COMMAND = new NonTerminal("DRAWING_COMMAND");

            // 250. Pen commands
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

            // TUPLE_ELEMENT.Rule = integer_number | decimal_number;
            // TUPLE_ELEMENT.Rule = EXPRESSION;
            // TUPLE.Rule = TUPLE + TUPLE_ELEMENT | TUPLE_ELEMENT; // Simply using TUPLE | TUPLE_ELEMENT will result in conflicts.
            TUPLE.Rule = MakeStarRule(TUPLE, EXPRESSION);
            TUPLEARGS.Rule = LSB + TUPLE + RSB;

            EXPRESSION.Rule = EXPRESSION_FACTOR | PARENTHESIS_EXPRESSION | BINARY_EXPRESSION | FUNCTION_CALL;
            EXPRESSION_FACTOR.Rule = integer_number | decimal_number | identifier;
            PARENTHESIS_EXPRESSION.Rule = LPS + EXPRESSION + RPS;
            BINARY_OPERATOR.Rule = ToTerm("+") | "-" | "*" | "/" | "^";
            BINARY_EXPRESSION.Rule = EXPRESSION + BINARY_OPERATOR + EXPRESSION;

            FUNCTION_ARGS.Rule = MakeStarRule(FUNCTION_ARGS, ToTerm(","), EXPRESSION);
            FUNCTION_CALL.Rule = identifier + LPS + FUNCTION_ARGS + RPS;

            ASSIGNMENT.Rule = identifier + "=" + EXPRESSION;

            LT.Rule = ToTerm("LT") | ToTerm("LEFT");
            RT.Rule = ToTerm("RT") | ToTerm("RIGHT");
            FD.Rule = ToTerm("FD") | ToTerm("FORWARD");
            BK.Rule = ToTerm("BK") | ToTerm("BACKWARD");
            ARC.Rule = ToTerm("ARC") | ToTerm("ARC2");
            DELAY.Rule = ToTerm("DELAY");
            DRAW.Rule = ToTerm("CLS") | ToTerm("DRAW") | ToTerm("CLEARSCR") | ToTerm("CLEARSCREEN");
            DRAWING_COMMAND.Rule = LT + EXPRESSION | 
                RT + EXPRESSION | 
                FD + EXPRESSION | 
                BK + EXPRESSION |
                ARC + EXPRESSION |
                DELAY + integer_number | DRAW;

            PC.Rule = ToTerm("SETPENCOLOR") | "SETPC" | "PC";
            PE.Rule = ToTerm("PENERASE") | "PE";
            PN.Rule = ToTerm("PENNORMAL") | "PN";
            PEN_DOWN.Rule = ToTerm("PD") | "PENDOWN";
            PEN_UP.Rule = ToTerm("PU") | "PENUP";
            SET_PEN_SIZE.Rule = ToTerm("SETPENSIZE") + TUPLEARGS;
            SET_PEN_COLOR.Rule = PC + TUPLEARGS;
            PEN_COMMAND.Rule = PEN_DOWN | PEN_UP | SET_PEN_COLOR | SET_PEN_SIZE | PE | PN;

            // REPEAT_BODY.Rule = REPEAT_BODY + BASIC_COMMAND | BASIC_COMMAND;
            REPEAT_BODY.Rule = MakeStarRule(REPEAT_BODY, COMMAND_LINE);
            REPEAT_COMMAND.Rule = ToTerm("REPEAT") + EXPRESSION + LSB + REPEAT_BODY + RSB;

            BASIC_COMMAND.Rule = DRAWING_COMMAND | PEN_COMMAND;
            FLOW_CONTROL_COMMAND.Rule = REPEAT_COMMAND;

            COMMAND_LINE.Rule = BASIC_COMMAND | FLOW_CONTROL_COMMAND | ASSIGNMENT;
            COMMAND.Rule = COMMAND_LINE | Empty + NewLine;
            PROGRAM.Rule = MakePlusRule(PROGRAM, COMMAND); 

            RegisterOperators(60, "^");
            RegisterOperators(50, "*", "/");
            RegisterOperators(40, "+", "-");

            RegisterBracePair("[", "]");
            RegisterBracePair("(", ")");

            MarkPunctuation(LPS, RPS);
            MarkPunctuation(LSB, RSB);

            MarkTransient(COMMAND, 
                COMMAND_LINE, 
                BASIC_COMMAND, 
                FLOW_CONTROL_COMMAND, 
                TUPLEARGS, 
                // TUPLE_ELEMENT, 
                EXPRESSION, 
                EXPRESSION_FACTOR, 
                BINARY_OPERATOR, 
                PARENTHESIS_EXPRESSION,
                FUNCTION_ARGS);

            this.Root = PROGRAM;
        }
    }
}
