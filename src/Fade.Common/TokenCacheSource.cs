namespace Fade.Common
{
    public class TokenCacheSource : ICacheSource<Lexer, Token>
    {
        public TokenCacheSource(Lexer source) {
            Source = source;
        }

        public Token Eof { get; } = Token.Eof;

        public Lexer Source { get; }

        public Token GetNextResult() {
            return Source.GetNextToken();
        }
    }
}