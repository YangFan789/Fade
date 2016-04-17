using System;
using System.Collections.Generic;
using System.Linq;
using Fade.Common;

namespace Fade.RegularExpression
{
    internal class RegexParser
    {
        private readonly RegexLexer _lexer;

        private readonly Stack<Regex> _regexStack = new Stack<Regex>();

        private Token _lookaheadToken;

        public RegexParser(RegexLexer lexer) {
            _lexer = lexer;
            _lookaheadToken = _lexer.GetNextToken();
            _regexStack.Clear();
        }

        public Regex Parse() {
            ParseExpression();
            return _regexStack.Pop();
        }

        private void Consume(Token token) {
            if (token == null) {
                throw new ArgumentNullException(nameof(token));
            }
            if (_lookaheadToken != token) {
                throw new InvalidOperationException("Lookahead token tokenType not equal to argument tokenType.");
            }
            _lookaheadToken = _lexer.GetNextToken();
        }

        private void Consume() {
            _lookaheadToken = _lexer.GetNextToken();
        }

        private void Handle(Token token) {
            if (token.Type == TokenType.Identifier) {
                HandleChar(token);
            }
            if (token.Text == "*" || token.Text == "+") {
                HandleRepeat(token);
            }
            else if (token.Text == "|") {
                HandleAlt(token);
            }
            else if (token.Text == "?") {
                HandleQuestionMark(token);
            }
            else if (token.Text == "Concat") {
                HandleConcat(token);
            }
        }

        private void HandleAlt(Token token) {
            var n2 = _regexStack.Pop();
            var n1 = _regexStack.Pop();
            var s0 = new RegexState();
            s0.Epsilon.Add(n1.Start);
            s0.Epsilon.Add(n2.Start);
            var s3 = new RegexState();
            n1.End.Epsilon.Add(s3);
            n2.End.Epsilon.Add(s3);
            n1.End.IsFinalStates = false;
            n2.End.IsFinalStates = false;
            var regex = new Regex(s0, s3);
            _regexStack.Push(regex);
        }

        private void HandleChar(Token token) {
            var s0 = new RegexState();
            var s1 = new RegexState();
            s0.Transitions.Add(token.Text.First(), s1);
            var regex = new Regex(s0, s1);
            _regexStack.Push(regex);
        }

        private void HandleConcat(Token token) {
            var n2 = _regexStack.Pop();
            var n1 = _regexStack.Pop();
            n1.End.IsFinalStates = false;
            n1.End.Epsilon.Add(n2.Start);
            var regex = new Regex(n1.Start, n2.End);
            _regexStack.Push(regex);
        }

        private void HandleQuestionMark(Token token) {
            var n1 = _regexStack.Pop();
            n1.Start.Epsilon.Add(n1.End);
            _regexStack.Push(n1);
        }

        private void HandleRepeat(Token token) {
            var s0 = _regexStack.Pop();
            var s1 = new RegexState();
            var s2 = new RegexState();
            s1.Epsilon.Add(s0.Start);
            if (token.Text == "*") {
                s1.Epsilon.Add(s2);
            }
            s0.End.Epsilon.Add(s2);
            s0.End.Epsilon.Add(s0.Start);
            s0.End.IsFinalStates = false;
            var regex = new Regex(s1, s2);
            _regexStack.Push(regex);
        }

        private void ParseExpression() {
            ParseTerm();
            if (_lookaheadToken != RegexToken.Alt) {
                return;
            }
            var token = _lookaheadToken;
            Consume(RegexToken.Alt);
            ParseExpression();
            Handle(token);
        }

        private void ParseFactor() {
            ParsePrimary();
            if (_lookaheadToken.Text == "*" || _lookaheadToken.Text == "+" || _lookaheadToken.Text == "?") {
                Handle(_lookaheadToken);
                Consume();
            }
        }

        private void ParsePrimary() {
            if (_lookaheadToken.Text == "(") {
                Consume(RegexToken.LeftParen);
                ParseExpression();
                Consume(RegexToken.RightParen);
            }
            else if (_lookaheadToken.Type == TokenType.Identifier) {
                Handle(_lookaheadToken);
                Consume();
            }
        }

        private void ParseTerm() {
            ParseFactor();
            if (_lookaheadToken == RegexToken.RightParen
                || _lookaheadToken == RegexToken.Alt
                || _lookaheadToken == Token.Eof) {
                return;
            }
            ParseTerm();
            Handle(RegexToken.Concat);
        }
    }
}