using System;
using System.Collections.Generic;

namespace Fade.RegularExpressions
{
    internal static class Parser
    {
        private static readonly Dictionary<TokenType, HandlerDelegate> HandlerDictionary =
            new Dictionary<TokenType, HandlerDelegate> {
                [TokenType.Char] = HandleChar,
                [TokenType.Concat] = HandleConcat,
                [TokenType.Alt] = HandleAlt,
                [TokenType.Star] = HandleRepeat,
                [TokenType.QuestionMark] = HandleQuestionMark,
                [TokenType.Plus] = HandleRepeat
            };

        private static readonly Stack<Regex> RegexStack = new Stack<Regex>();

        private static Token lookaheadToken;

        private delegate void HandlerDelegate(Token token);

        public static void Init() {
            lookaheadToken = Lexer.GetToken();
            RegexStack.Clear();

        }

        public static Regex Parse() {
            ParseExpression();
            return RegexStack.Pop();
        }

        private static void Consume(TokenType tokenType) {
            if (lookaheadToken.Type == tokenType) {
                lookaheadToken = Lexer.GetToken();
                return;
            }
            throw new InvalidOperationException("Lookahead token tokenType not equal to argument tokenType.");
        }

        private static void Handle(Token token) {
            HandlerDictionary[token.Type](token);
        }

        private static void HandleAlt(Token token) {
            var n2 = RegexStack.Pop();
            var n1 = RegexStack.Pop();
            var s0 = new State();
            s0.Epsilon.Add(n1.Start);
            s0.Epsilon.Add(n2.Start);
            var s3 = new State();
            n1.End.Epsilon.Add(s3);
            n2.End.Epsilon.Add(s3);
            n1.End.IsFinalStates = false;
            n2.End.IsFinalStates = false;
            var regex = new Regex(s0, s3);
            RegexStack.Push(regex);
        }

        private static void HandleChar(Token token) {
            var s0 = new State();
            var s1 = new State();
            s0.Transitions.Add(token.Value, s1);
            var regex = new Regex(s0, s1);
            RegexStack.Push(regex);
        }

        private static void HandleConcat(Token token) {
            var n2 = RegexStack.Pop();
            var n1 = RegexStack.Pop();
            n1.End.IsFinalStates = false;
            n1.End.Epsilon.Add(n2.Start);
            var regex = new Regex(n1.Start, n2.End);
            RegexStack.Push(regex);
        }

        private static void HandleQuestionMark(Token token) {
            var n1 = RegexStack.Pop();
            n1.Start.Epsilon.Add(n1.End);
            RegexStack.Push(n1);
        }

        private static void HandleRepeat(Token token) {
            var n1 = RegexStack.Pop();
            var s0 = new State();
            var s1 = new State();
            s0.Epsilon.Add(n1.Start);
            if (token.Type == TokenType.Star) {
                s0.Epsilon.Add(s1);
            }
            n1.End.Epsilon.Add(s1);
            n1.End.Epsilon.Add(n1.Start);
            n1.End.IsFinalStates = false;
            var regex = new Regex(s0, s1);
            RegexStack.Push(regex);
        }

        private static void ParseExpression() {
            ParseTerm();
            if (lookaheadToken.Type != TokenType.Alt) return;
            var token = lookaheadToken;
            Consume(TokenType.Alt);
            ParseExpression();
            Handle(token);
        }

        private static void ParseFactor() {
            ParsePrimary();
            switch (lookaheadToken.Type) {
                case TokenType.Star:
                case TokenType.Plus:
                case TokenType.QuestionMark:
                    Handle(lookaheadToken);
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
                    Handle(lookaheadToken);
                    Consume(TokenType.Char);
                    break;

                case TokenType.None:
                    break;

                case TokenType.Star:
                    break;

                case TokenType.Plus:
                    break;

                case TokenType.Concat:
                    break;

                case TokenType.Alt:
                    break;

                case TokenType.QuestionMark:
                    break;

                case TokenType.RightParen:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ParseTerm() {
            ParseFactor();
            if (")|\0".Contains(lookaheadToken.Value.ToString())) return;
            ParseTerm();
            Handle(Token.Concat);
        }
    }
}