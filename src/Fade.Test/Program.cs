using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fade.Diagnostics;
using Fade.RegularExpression;

namespace Fade.Test
{
    internal class Program
    {
        private static void Main(string[] args) {
            //RegularExpressionsTest();
            RegexVisualizer.Visual(Regex.FromPatternString("((a*)(b|abc))(c*)"), "123.png");
            //Console.ReadKey();
        }

        private static void RegularExpressionsTest() {
            var testSuite = new Dictionary<string, string> {
                ["foo"] = "foo",
                ["ab*bc"] = "abbbbc",
                ["(ab)c|abc"] = "a",
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
                foreach (var result in from suite in testSuite
                    let re = Regex.FromPatternString(suite.Key)
                    select re.IsMatch(suite.Value) ? "Passed" : "Failed") {
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
                    var result = re.IsMatch(suite.Value) ? "Passed" : "Failed";
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