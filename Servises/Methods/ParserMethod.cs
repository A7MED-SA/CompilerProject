using MainConsole.DataStructure;
using MainConsole.Wah_Ya_Saeidi_L;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MainConsole.Servises
{
    public partial class Parser
    {
        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private Token Peek()
        {
            return _tokens[_current];
        }


        private Token PeekNext()
        {
            if (_current + 1 >= _tokens.Count) return _tokens[_tokens.Count - 1];
            return _tokens[_current + 1];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private bool IsAtEnd()
        {
            return _current >= _tokens.Count || Peek().Type == TokenType.EndOfFile;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

            AddError(message, Peek().Lexeme, type.ToString());
            throw new Exception(message);
        }

        private void AddError(string message, string found, string expected = "")
        {
            Errors.Add(new ParserError
            {
                Line = Peek().Line,
                Message = message,
                TokenFound = found,
                Expected = expected
            });
        }

        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.Semicolon) return;

                switch (Peek().Type)
                {
                    case TokenType.Rakam:
                    case TokenType.Kalam:
                    case TokenType.Law:
                    case TokenType.Lef:
                    case TokenType.Ektob:
                    case TokenType.Hesba:  
                        return;
                }

                Advance();
            }
        }
    }
}
