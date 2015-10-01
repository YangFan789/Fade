using System;
using Fade;

namespace FadeTest
{
    static class Program
    {
        static void Main(string[] args) {
            LexerGenerater.FromFile("LexerGenTest.fade").Generate("LexerGenTest.cs");
            Console.WriteLine("Done.");
        }
    }
}
