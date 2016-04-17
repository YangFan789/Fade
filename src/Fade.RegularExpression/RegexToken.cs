using System.Collections.Generic;
using Fade.Common;

namespace Fade.RegularExpression
{
    internal static class RegexToken
    {
        public static readonly Dictionary<char, Token> ReTokenDictionary = new Dictionary<char, Token> {
            ['('] = LeftParen,
            [')'] = RightParen,
            ['*'] = Star,
            ['|'] = Alt,
            ['+'] = Plus,
            ['?'] = QuestionMark
        };

        public static Token LeftParen { get; } = new Token("(", TokenType.Symbol);
        public static Token RightParen { get; } = new Token(")", TokenType.Symbol);
        public static Token Alt { get; } = new Token("|", TokenType.Symbol);
        public static Token Star { get; } = new Token("*", TokenType.Symbol);
        public static Token Plus { get; } = new Token("+", TokenType.Symbol);
        public static Token QuestionMark { get; } = new Token("?", TokenType.Symbol);
        public static Token Concat { get; } = new Token("Concat", TokenType.Symbol);
    }
}