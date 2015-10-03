namespace Fade.RegularExpressions
{
    internal enum TokenType
    {
        None,
        Star,
        Plus,
        Char,
        Concat,
        Alt,
        QuestionMark,
        LeftParen,
        RightParen
    }
}