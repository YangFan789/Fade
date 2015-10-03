using System.Collections.Generic;

namespace Fade.RegularExpressions
{
    internal static class Lexer
    {
        private static readonly Dictionary<char, TokenType> Symbols = new Dictionary<char, TokenType> {
            ['('] = TokenType.LeftParen,
            [')'] = TokenType.RightParen,
            ['*'] = TokenType.Star,
            ['|'] = TokenType.Alt,
            ['\x08'] = TokenType.Concat,
            ['+'] = TokenType.Plus,
            ['?'] = TokenType.QuestionMark
        };

        private static int Current { get; set; }
        private static string Source { get; set; }

        public static void Init(string pattern) {
            Source = pattern;
            Current = 0;
        }

        public static Token GetToken() {
            if (Current >= Source.Length) return Token.Empty;
            var c = Source[Current++];
            return new Token(Symbols.ContainsKey(c) ? Symbols[c] : TokenType.Char, new string(c, 1));
        }
    }
}