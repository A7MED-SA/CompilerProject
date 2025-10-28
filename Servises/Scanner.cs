using MainConsole.DataStructure;
using MainConsole.Wah_Ya_Saeidi_L;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainConsole.Servises
{
    public class Scanner
    {
        private class TokenDefinition
        {
            public Regex Pattern { get; }
            public TokenType Type { get; }
            public bool Ignore { get; } 

            public TokenDefinition(string pattern, TokenType type, bool ignore = false)
            {
                Pattern = new Regex($"^{pattern}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                Type = type;
                Ignore = ignore;
            }
        }

        private readonly List<TokenDefinition> _tokenDefinitions = new List<TokenDefinition>();
        private readonly List<Token> _tokens = new List<Token>();
        private string _remainingSource; 
        private int _line = 1;

        public Scanner(string source)
        {
            _remainingSource = source;
            InitializeTokenDefinitions();
        }

        private void InitializeTokenDefinitions()
        {
            _tokenDefinitions.Add(new TokenDefinition(@"\s+", TokenType.Unknown, ignore: true));
            _tokenDefinitions.Add(new TokenDefinition(@"سامو عليكم", TokenType.SamoAlikom));
            _tokenDefinitions.Add(new TokenDefinition(@"حيسبة", TokenType.Hesba));
            _tokenDefinitions.Add(new TokenDefinition(@"رقم", TokenType.Rakam));
            _tokenDefinitions.Add(new TokenDefinition(@"كلام", TokenType.Kalam));
            _tokenDefinitions.Add(new TokenDefinition(@"كسر", TokenType.Kasr));
            _tokenDefinitions.Add(new TokenDefinition(@"صحغلط", TokenType.SahGhalat));
            _tokenDefinitions.Add(new TokenDefinition(@"اكتوب", TokenType.Ektob));
            _tokenDefinitions.Add(new TokenDefinition(@"اظهره", TokenType.Ezhroh));
            _tokenDefinitions.Add(new TokenDefinition(@"لو", TokenType.Law));
            _tokenDefinitions.Add(new TokenDefinition(@"والا", TokenType.Walla));
            _tokenDefinitions.Add(new TokenDefinition(@"علطول", TokenType.Alatol));
            _tokenDefinitions.Add(new TokenDefinition(@"لفلهم", TokenType.Leflohom));
            _tokenDefinitions.Add(new TokenDefinition(@"لف", TokenType.Lef));
            _tokenDefinitions.Add(new TokenDefinition(@"الجوف", TokenType.ElGof));
            _tokenDefinitions.Add(new TokenDefinition(@"تسهيل", TokenType.Tasheel));
            _tokenDefinitions.Add(new TokenDefinition(@"عيلة", TokenType.Eila));
            _tokenDefinitions.Add(new TokenDefinition(@"ولا حاجة", TokenType.WalaHaga));
            _tokenDefinitions.Add(new TokenDefinition(@"جاعد", TokenType.Gaed));
            _tokenDefinitions.Add(new TokenDefinition(@"""[^""]*""", TokenType.StringLiteral));
            _tokenDefinitions.Add(new TokenDefinition(@"[0-9٠-٩]+(?![\p{L}0-9٠-٩_])", TokenType.NumberLiteral));
            _tokenDefinitions.Add(new TokenDefinition(@"[\p{L}_][\p{L}0-9_]*", TokenType.Identifier));
            _tokenDefinitions.Add(new TokenDefinition(@"\+\+", TokenType.Increment));
            _tokenDefinitions.Add(new TokenDefinition(@"==", TokenType.EqualsEquals));
            _tokenDefinitions.Add(new TokenDefinition(@"!=", TokenType.BangEquals));
            _tokenDefinitions.Add(new TokenDefinition(@">=", TokenType.GreaterOrEqual));
            _tokenDefinitions.Add(new TokenDefinition(@"<=", TokenType.LessOrEqual));
            _tokenDefinitions.Add(new TokenDefinition(@"\+", TokenType.Plus));
            _tokenDefinitions.Add(new TokenDefinition(@"-", TokenType.Minus));
            _tokenDefinitions.Add(new TokenDefinition(@"\*", TokenType.Star));
            _tokenDefinitions.Add(new TokenDefinition(@"/", TokenType.Slash));
            _tokenDefinitions.Add(new TokenDefinition(@"=", TokenType.Equals));
            _tokenDefinitions.Add(new TokenDefinition(@"<", TokenType.Less));
            _tokenDefinitions.Add(new TokenDefinition(@">", TokenType.Greater));
            _tokenDefinitions.Add(new TokenDefinition(@"!", TokenType.Bang));
            _tokenDefinitions.Add(new TokenDefinition(@"\(", TokenType.OpenParen));
            _tokenDefinitions.Add(new TokenDefinition(@"\)", TokenType.CloseParen));
            _tokenDefinitions.Add(new TokenDefinition(@"\{", TokenType.OpenBrace));
            _tokenDefinitions.Add(new TokenDefinition(@"\}", TokenType.CloseBrace));
            _tokenDefinitions.Add(new TokenDefinition(@"\[", TokenType.OpenBracket));
            _tokenDefinitions.Add(new TokenDefinition(@"\]", TokenType.CloseBracket));
            _tokenDefinitions.Add(new TokenDefinition(@";", TokenType.Semicolon));
            _tokenDefinitions.Add(new TokenDefinition(@",", TokenType.Comma));
            _tokenDefinitions.Add(new TokenDefinition(@"\.", TokenType.Dot));
        }

        public List<Token> ScanTokens()
        {
            while (!string.IsNullOrEmpty(_remainingSource))
            {
                bool matchFound = false;
                foreach (var def in _tokenDefinitions)
                {
                    Match match = def.Pattern.Match(_remainingSource);

                    if (match.Success)
                    {
                        matchFound = true;
                        string lexeme = match.Value;

                        _line += lexeme.Count(c => c == '\n');

                        if (!def.Ignore)
                        {
                            object? literal = null;
                            if (def.Type == TokenType.StringLiteral)
                            {
                                literal = lexeme.Substring(1, lexeme.Length - 2);
                            }
                            else if (def.Type == TokenType.NumberLiteral)
                            {
                                literal = lexeme; 
                            }

                            _tokens.Add(new Token(def.Type, lexeme, literal, _line));
                        }

                        _remainingSource = _remainingSource.Substring(lexeme.Length);

                        break;
                    }
                }

                if (!matchFound)
                {
                    // جمع كل الحروف/الأرقام المتصلة كـ Unknown token واحد
                    int length = 1;
                    while (length < _remainingSource.Length)
                    {
                        char c = _remainingSource[length];
                        if (!char.IsLetterOrDigit(c) && c != '_')
                            break;
                        length++;
                    }
                    
                    string unknownToken = _remainingSource.Substring(0, length);
                    _tokens.Add(new Token(TokenType.Unknown, unknownToken, null, _line));
                    _remainingSource = _remainingSource.Substring(length);
                }
            }

            _tokens.Add(new Token(TokenType.EndOfFile, "", null, _line));
            return _tokens;
        }
    }
   
}
