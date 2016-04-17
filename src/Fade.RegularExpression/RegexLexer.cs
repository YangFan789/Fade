using System.IO;
using Fade.Common;

namespace Fade.RegularExpression
{
    internal class RegexLexer : ITokenSource
    {
        private readonly Cache<TextReader, char> _cache;

        public RegexLexer(Cache<TextReader, char> cache) {
            _cache = cache;
        }

        public Token GetNextToken() {
            var c = _cache.Consume();
            if (c == _cache.Eof) {
                return Token.Eof;
            }

            return RegexToken.ReTokenDictionary.ContainsKey(c)
                ? RegexToken.ReTokenDictionary[c]
                : new Token(c.ToString(), TokenType.Identifier);
        }
    }
}