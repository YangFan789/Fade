using System;

namespace Fade.Common
{
    public class Token : IEquatable<Token>
    {
        public Token(string text, TokenType type) {
            Text = text;
            Type = type;
        }

        public static Token Eof { get; } = new Token("\uffff", TokenType.Eof);

        public static Token Null { get; } = new Token(null, TokenType.Null);

        public string Text { get; }

        public TokenType Type { get; }

        public bool Equals(Token other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(Text, other.Text) && Type == other.Type;
        }

        public static bool operator !=(Token lhs, Token rhs) {
            return !(lhs == rhs);
        }

        public static bool operator ==(Token lhs, Token rhs) {
            return !ReferenceEquals(null, lhs) && lhs.Equals(rhs);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            return obj.GetType() == GetType() && Equals((Token) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Text?.GetHashCode() ?? 0)*397) ^ (int) Type;
            }
        }

        public override string ToString() {
            return $"Token::({Enum.GetName(typeof (TokenType), Type)}, \"{Text}\" )";
        }
    }
}