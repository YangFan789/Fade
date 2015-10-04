namespace Fade.RegularExpressions
{
    internal class Token
    {
        public static readonly Token Empty = new Token(TokenType.None, '\0');
        public static readonly Token Concat = new Token(TokenType.Concat, '\x08');

        public Token(TokenType type, char value) {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; }
        public char Value { get; }

        public override string ToString() {
            return $"{Type}::{Value}";
        }
    }
}