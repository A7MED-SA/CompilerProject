using MainConsole.DataStructure;
using MainConsole.Wah_Ya_Saeidi_L;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainConsole.Servises.Grammers;

namespace MainConsole.Servises
{
    public partial class Parser
    {
        private List<Token> _tokens;
        private int _current = 0;

        public ProgramNode? AST { get; private set; }
        public List<ParserError> Errors { get; private set; }
        public List<string> Warnings { get; private set; }

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            Errors = new List<ParserError>();
            Warnings = new List<string>();
        }

        public bool Parse()
        {
            try
            {
                var astRoot = new ProgramNode();

                // Parse: عيلة ClassName {
                if (!Match(TokenType.Eila))
                {
                    AddError("البرنامج لازم يبدأ بـ 'عيلة'", "");
                    return false;
                }
                Token className = Consume(TokenType.Identifier, "توقع اسم الكلاس");
                astRoot.ClassName = className.Lexeme;
                Consume(TokenType.OpenBrace, "توقع '{'");

                // Parse multiple functions
                while (!Check(TokenType.CloseBrace) && !IsAtEnd())
                {
                    try
                    {
                        var functionNode = ParseFunction();
                        if (functionNode != null)
                            astRoot.Functions.Add(functionNode);
                    }
                    catch (Exception ex)
                    {
                        AddError(ex.Message, Peek().Lexeme);
                        SynchronizeFunction();
                    }
                }

                Consume(TokenType.CloseBrace, "توقع '}' لإغلاق الكلاس");

                AST = astRoot;
                return Errors.Count == 0;
            }
            catch (Exception ex)
            {
                AddError("خطأ عام: " + ex.Message, "");
                return false;
            }
        }

        private FunctionNode ParseFunction()
        {
            // Parse: [جاعد (optional)] حيسبة (required) [return_type (optional)] FunctionName (required) () { ... }
            bool isStatic = false;
            Token? returnTypeToken = null;
            Token? nameToken = null;

            // Parse optional 'جاعد' (static modifier)
            if (Match(TokenType.Gaed))
            {
                isStatic = true;
            }

            // Parse required 'حيسبة' (function start keyword)
            if (!Match(TokenType.Hesba))
            {
                AddError("توقع 'حيسبة' لبداية الدالة", Peek().Lexeme);
                throw new Exception("توقع 'حيسبة' لبداية الدالة");
            }

            // Parse optional return type (رقم, كلام, كسر, صحغلط, or identifier)
            if (Check(TokenType.Rakam) || Check(TokenType.Kalam) || 
                Check(TokenType.Kasr) || Check(TokenType.SahGhalat))
            {
                returnTypeToken = Advance();
            }
            else if (Check(TokenType.Identifier))
            {
                // Could be return type or function name - need to check if next token is identifier or '('
                Token next = PeekNext();
                if (next.Type == TokenType.Identifier || next.Type == TokenType.SamoAlikom)
                {
                    // Current token is return type, next is function name
                    returnTypeToken = Advance();
                }
                // else: current token is function name (no return type)
            }

            // Parse function name (either 'سامو عليكم' or identifier)
            if (Match(TokenType.SamoAlikom))
            {
                nameToken = Previous();
            }
            else if (Check(TokenType.Identifier))
            {
                nameToken = Advance();
            }
            else
            {
                AddError("توقع اسم الدالة", Peek().Lexeme);
                throw new Exception("توقع اسم الدالة");
            }

            // Parse parameters ()
            Consume(TokenType.OpenParen, "توقع '('");
            Consume(TokenType.CloseParen, "توقع ')'");

            // Parse body
            Consume(TokenType.OpenBrace, "توقع '{'");
            var body = new BlockNode();

            while (!Check(TokenType.CloseBrace) && !IsAtEnd())
            {
                try
                {
                    var stmtAST = ParseStatement();
                    if (stmtAST != null)
                        body.Statements.Add(stmtAST);
                }
                catch (Exception ex)
                {
                    AddError(ex.Message, Peek().Lexeme);
                    Synchronize();
                }
            }

            Consume(TokenType.CloseBrace, "توقع '}' لإغلاق الدالة");

            return new FunctionNode
            {
                IsStatic = isStatic,
                ReturnTypeToken = returnTypeToken,
                NameToken = nameToken,
                Body = body
            };
        }

        private void SynchronizeFunction()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Check(TokenType.Hesba) || Check(TokenType.CloseBrace))
                    return;

                Advance();
            }
        }

        private ASTNode ParseStatement()
        {
            if (Check(TokenType.Rakam) || Check(TokenType.Kalam) ||
                Check(TokenType.Kasr) || Check(TokenType.SahGhalat))
            {
                Token type = Advance();
                Token name = Consume(TokenType.Identifier, "توقع اسم متغير");

                ASTNode value = null;
                if (Match(TokenType.Equals))
                {
                    value = ParseExpression();
                }
                Consume(TokenType.Semicolon, "توقع ';'");

                return new VarDeclNode { Type = type.Lexeme, Name = name.Lexeme, Value = value };
            }
            else if (Match(TokenType.Ektob))
            {
                var expr = ParseExpression();
                Consume(TokenType.Semicolon, "توقع ';'");
                return new PrintNode { Expression = expr };
            }
            else if (Match(TokenType.Law))
            {
                Consume(TokenType.OpenParen, "توقع '('");
                var condition = ParseExpression();
                Consume(TokenType.CloseParen, "توقع ')'");
                var thenBranch = ParseStatement();

                ASTNode elseBranch = null;
                if (Match(TokenType.Walla))
                {
                    elseBranch = ParseStatement();
                }

                return new IfNode { Condition = condition, ThenBranch = thenBranch, ElseBranch = elseBranch };
            }
            else if (Match(TokenType.Alatol)) 
            {
                Consume(TokenType.OpenParen, "توقع '(' بعد 'علطول'");
                var condition = ParseExpression();
                Consume(TokenType.CloseParen, "توقع ')'");
                var body = ParseStatement();
                return new WhileNode { Condition = condition, Body = body };
            }
            else if (Match(TokenType.Lef)) 
            {
                Consume(TokenType.OpenParen, "توقع '(' بعد 'لف'");

                Token next = PeekNext();
                if (Check(TokenType.Rakam) || Check(TokenType.Kalam) || Check(TokenType.Kasr) || Check(TokenType.SahGhalat) ||
                    Check(TokenType.Semicolon) ||
                   (Check(TokenType.Identifier) && next.Type == TokenType.Equals))
                {
                    return ParseForStatement();
                }
                else
                {
                    var condition = ParseExpression();
                    Consume(TokenType.CloseParen, "توقع ')'");
                    var body = ParseStatement();
                    return new WhileNode { Condition = condition, Body = body };
                }
            }
            else if (Match(TokenType.OpenBrace))
            {
                var block = new BlockNode();
                while (!Check(TokenType.CloseBrace) && !IsAtEnd())
                {
                    block.Statements.Add(ParseStatement());
                }
                Consume(TokenType.CloseBrace, "توقع '}'");
                return block;
            }
            else if (Check(TokenType.Identifier))
            {
                Token name = Advance();
                Consume(TokenType.Equals, "توقع '='");
                var value = ParseExpression();
                Consume(TokenType.Semicolon, "توقع ';'");

                return new AssignmentNode { Name = name.Lexeme, Value = value };
            }
            else if (Match(TokenType.ElGof))
            {
                var node = new ReturnNode();

                if (!Check(TokenType.Semicolon))
                {
                    node.ReturnValue = ParseExpression();
                }

                Consume(TokenType.Semicolon, "توقع ';' بعد 'الجوف'");
                return node;
            }

            throw new Exception("جملة غير متوقعة.");
        }

        private ASTNode ParseForStatement()
        {

            ASTNode initializer = null;
            if (Check(TokenType.Rakam) || Check(TokenType.Kalam) || Check(TokenType.Kasr) || Check(TokenType.SahGhalat))
            {
                Token type = Advance();
                Token name = Consume(TokenType.Identifier, "توقع اسم متغير في جملة 'لف'");
                ASTNode value = null;
                if (Match(TokenType.Equals))
                {
                    value = ParseExpression();
                }
                initializer = new VarDeclNode { Type = type.Lexeme, Name = name.Lexeme, Value = value };
            }
            else if (!Check(TokenType.Semicolon))
            {
                initializer = ParseExpression();
            }

            Consume(TokenType.Semicolon, "توقع ';' بعد الجزء الأول من 'لف'");

            ASTNode condition = null;
            if (!Check(TokenType.Semicolon))
            {
                condition = ParseExpression();
            }
            Consume(TokenType.Semicolon, "توقع ';' بعد شرط 'لف'");

            ASTNode increment = null;
            if (!Check(TokenType.CloseParen))
            {
                increment = ParseExpression();
            }
            Consume(TokenType.CloseParen, "توقع ')' لإغلاق جملة 'لف'");

            ASTNode body = ParseStatement();

            return new ForNode
            {
                Initializer = initializer,
                Condition = condition,
                Increment = increment,
                Body = body
            };
        }


        private ASTNode ParseExpression()
        {
            return ParseComparison();
        }

        private ASTNode ParseComparison()
        {
            var left = ParseTerm();

            while (Match(TokenType.Greater, TokenType.GreaterOrEqual,
                           TokenType.Less, TokenType.LessOrEqual,
                           TokenType.EqualsEquals, TokenType.BangEquals))
            {
                string op = Previous().Lexeme;
                var right = ParseTerm();
                left = new BinaryOpNode { Left = left, Operator = op, Right = right };
            }

            return left;
        }

        private ASTNode ParseTerm()
        {
            var left = ParseFactor();

            while (Match(TokenType.Plus, TokenType.Minus))
            {
                string op = Previous().Lexeme;
                var right = ParseFactor();
                left = new BinaryOpNode { Left = left, Operator = op, Right = right };
            }

            return left;
        }

        private ASTNode ParseFactor()
        {
            var left = ParsePostfix();

            while (Match(TokenType.Star, TokenType.Slash))
            {
                string op = Previous().Lexeme;
                var right = ParsePostfix();
                left = new BinaryOpNode { Left = left, Operator = op, Right = right };
            }

            return left;
        }

        private ASTNode ParsePostfix()
        {
            var expr = ParsePrimary();

            if (expr is VariableNode && Match(TokenType.Increment))
            {
                return new PostfixIncrementNode { Variable = (VariableNode)expr };
            }

            return expr;
        }


        private ASTNode ParsePrimary()
        {
            if (Match(TokenType.NumberLiteral))
            {
                return new LiteralNode { Value = Previous().Literal };
            }
            if (Match(TokenType.StringLiteral))
            {
                return new LiteralNode { Value = Previous().Literal };
            }
            if (Match(TokenType.Identifier))
            {
                return new VariableNode { Name = Previous().Lexeme };
            }
            if (Match(TokenType.OpenParen))
            {
                var expr = ParseExpression();
                Consume(TokenType.CloseParen, "توقع ')'");
                return expr;
            }

            throw new Exception("تعبير غير متوقع");
        }

        public string GetASTString()
        {
            if (AST == null) return "لا يوجد AST";
            return "===== AST =====\n" + AST.Print();
        }

        public string GetErrorsString()
        {
            if (Errors.Count == 0) return "✓ لا توجد أخطاء";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"===== الأخطاء ({Errors.Count}) =====");
            foreach (var error in Errors)
            {
                sb.AppendLine(error.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // public string GetWarningsString()
        // {
        //     if (Warnings.Count == 0) return "✓ لا توجد تحذيرات";

        //     StringBuilder sb = new StringBuilder();
        //     sb.AppendLine($"===== التحذيرات ({Warnings.Count}) =====");
        //     foreach (var warning in Warnings)
        //     {
        //         sb.AppendLine($"⚠ {warning}");
        //     }
        //     return sb.ToString();
        // }
    }
}