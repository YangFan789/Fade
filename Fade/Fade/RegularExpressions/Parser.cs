using System;
using System.Collections.Generic;

namespace Fade.RegularExpressions
{
    internal static class Parser
    {
        private static readonly List<Token> Tokens = new List<Token>();
        private static Token lookaheadToken;

        public static void Init() {
            lookaheadToken = Lexer.GetToken();
        }

        private static void Consume(TokenType tokenType) {
            if (lookaheadToken.Type == tokenType) {
                lookaheadToken = Lexer.GetToken();
            }
            else if (lookaheadToken.Type != tokenType) {
                throw new InvalidOperationException("Lookahead token tokenType not equal to argument tokenType.");
            }
        }

        public static IEnumerable<Token> Parse() {
            ParseExpression();
            return Tokens;
        }

        private static void ParseExpression() {
            ParseTerm();
            if (lookaheadToken.Type != TokenType.Alt) return;
            var token = lookaheadToken;
            Consume(TokenType.Alt);
            ParseExpression();
            Tokens.Add(token);
        }

        private static void ParseTerm() {
            ParseFactor();
            if (")|".Contains(lookaheadToken.Value)) return;
            ParseTerm();
            Tokens.Add(Token.Concat);
        }

        private static void ParseFactor() {
            ParsePrimary();
            if (!new List<TokenType> {
                TokenType.Star,
                TokenType.Plus,
                TokenType.QuestionMark
            }.Contains(lookaheadToken.Type))
                return;
            Tokens.Add(lookaheadToken);
            Consume(lookaheadToken.Type);
        }

        private static void ParsePrimary() {
            switch (lookaheadToken.Type) {
                case TokenType.LeftParen:
                    Consume(TokenType.LeftParen);
                    ParseExpression();
                    Consume(TokenType.RightParen);
                    break;
                case TokenType.Char:
                    Tokens.Add(lookaheadToken);
                    Consume(TokenType.Char);
                    break;
            }
        }
    }
}