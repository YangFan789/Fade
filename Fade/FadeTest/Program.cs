//#define TEST_CLASS_GENERATED

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Fade;
using Regex = Fade.RegularExpressions.Regex;

#if TEST_CLASS_GENERATED
using FadeLexerGenTest;
#endif

namespace FadeTest
{
    static class Program
    {
        static void Main(string[] args) {
            LexerGeneraterTest();
            GeneratedLexerTest();
            RegularExpressionsTest();
            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void GeneratedLexerTest() {
#if TEST_CLASS_GENERATED
            {
                var lexer = new HelloFade(" \t123456");
                var token = lexer.GetToken();
                while (token != null) {
                    Console.WriteLine($"{token.TokenType}::{token.Value}");
                    token = lexer.GetToken();
                }
            }
            Console.WriteLine();
#endif
        }

        private static void LexerGeneraterTest() {
            LexerGenerater.FromFile("LexerGenTest.fade").Generate("LexerGenTest.cs");
            Console.WriteLine("Generate LexerGenTest.cs from LexerGenTest.fade ...Done");
            Console.WriteLine();
        }

        private static void RegularExpressionsTest() {
            var testSuite = new Dictionary<string, string> {
                ["foo"] = "foo",
                ["ab*bc"] = "abbbbc",
                ["(ab)c|abc"] = "abc",
                ["(a*)(b?)(b+)"] = "aaabbbb",
                ["((a|a)|a)a"] = "aa",
                ["(a*)(a|aa)"] = "aaaa",
                ["((((((((((x))))))))))*"] = "xxxxxxx",
                ["((((((((((x))))))))))"] = "x",
                ["abcd*efg"] = "abcdefg",
                ["((a|ab)(c|bcd))(d*)"] = "abcd",
                ["((a*)(b|abc))(c*)"] = "abc",
                ["(a*)((b|abc)(c*))"] = "abc",
                ["我是火车*王！"] = "我是火车车车王！"
            };

            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var loopCount = 0;
                foreach (var suite in testSuite) {
                    var re = Regex.FromPatternString(suite.Key);
                    var result = re.Match(suite.Value) ? "Pass" : "Failed";
                    Console.WriteLine($"RegexTest {loopCount++} ...{result}");
                }
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
            }
#if !DEBUG
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var loopCount = 0;
                foreach (var suite in testSuite) {
                    var re = new System.Text.RegularExpressions.Regex(suite.Key);
                    var result = re.IsMatch(suite.Value) ? "Pass" : "Failed";
                    Console.WriteLine($"RegexTest {loopCount++} ...{result}");
                }
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
            }
#endif
            Console.WriteLine();
        }
    }
}
