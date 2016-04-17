using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fade.Common
{
    public class Lexer : ITokenSource
    {
        private readonly Cache<TextReader, char> _cache;
        public List<Token> KeywordList = new List<Token>();
        public List<string> SplitStringList = new List<string>();

        public List<Token> SymbolList = new List<Token>();

        public Lexer(Cache<TextReader, char> cache) {
            _cache = cache;
        }

        public List<Token> Keywords { get; } = new List<Token>();

        public Token GetNextToken() {
            if (_cache.IsCompleted) {
                return Token.Eof;
            }
            //keywords or symbols
            foreach (var token in SymbolList
                .Concat(KeywordList)
                .Where(token => _cache.CheckLookaheads(token.Text))) {
                _cache.Consume(token.Text.Length);
                return token;
            }
            //identifier
            return GetNextIdToken();
        }

        private Token GetNumberToken() {
            //-?[1-9][0-9]*(.[0-9]+)? | 0

            throw new NotImplementedException();
        }

        private Token GetNextIdToken() {
            // (.^[\r\n\t\各种符号                                                          ])+

            throw new NotImplementedException();
        }
    }
}