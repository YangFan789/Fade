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
                return;
            }
            throw new InvalidOperationException("Lookahead token tokenType not equal to argument tokenType.");
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
            if (")|\0".Contains(lookaheadToken.Value.ToString())) return;
            ParseTerm();
            Tokens.Add(Token.Concat);
        }

        private static void ParseFactor() {
            ParsePrimary();
            switch (lookaheadToken.Type) {
                case TokenType.Star:
                case TokenType.Plus:
                case TokenType.QuestionMark:
                    Tokens.Add(lookaheadToken);
                    Consume(lookaheadToken.Type);
                    break;
                case TokenType.None:
                    break;
                case TokenType.Char:
                    break;
                case TokenType.Concat:
                    break;
                case TokenType.Alt:
                    break;
                case TokenType.LeftParen:
                    break;
                case TokenType.RightParen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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