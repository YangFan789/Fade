namespace Fade.RegularExpressions
{
    internal class Token
    {
        public static readonly Token Empty = new Token(TokenType.None, string.Empty);
        public static readonly Token Concat = new Token(TokenType.Concat, "\x08");

        public Token(TokenType type, string value) {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; }
        public string Value { get; }

        public override string ToString() {
            return $"{Type}::{Value}";
        }
    }
}