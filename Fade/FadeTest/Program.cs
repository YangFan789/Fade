using System;
using Fade;
//using FadeLexerGenTest;
using Fade.RegularExpressions;

namespace FadeTest
{
    static class Program
    {
        static void Main(string[] args) {
            //LexerGenerater.FromFile("LexerGenTest.fade").Generate("LexerGenTest.cs");
            //Console.WriteLine("Done.");
            //{
            //    var lexer = new HelloFade(" \t123456");
            //    var token = lexer.GetToken();
            //    while (token != null) {
            //        Console.WriteLine($"{token.TokenType}::{token.Value}");
            //        token = lexer.GetToken();
            //    }
            //}
            var regex = Regex.Compile("(abab)*");
            Console.WriteLine(regex.Match("abab"));
            Console.ReadKey();
        }
    }
}
