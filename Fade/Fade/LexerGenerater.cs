using System.IO;
using System.Linq;
using System.Text;
using FadeJson;

namespace Fade
{
    public class LexerGenerater
    {
        private readonly JsonValue root;

        private LexerGenerater(JsonValue j) {
            root = j;
        }

        public static LexerGenerater FromFile(string path) {
            return new LexerGenerater(JsonValue.FromFile(path));
        }

        public static LexerGenerater FromString(string content) {
            return new LexerGenerater(JsonValue.FromString(content));
        }

        public void Generate(string path) {
            //read the config
            var config = root["config"];
            var skipList = (from object key in config["skip"].Keys select config["skip"][key].ToString()).ToList();
            var namespaceName = config["namespace"].ToString();
            var lexRuleName = config["lexRuleName"].ToString();
            //prepare template
            var ruleList = root["rule"];
            var ruleTemplate = new StringBuilder();
            var tokenTypeTemplate = new StringBuilder();
            foreach (var key in ruleList.Keys) {
                var value = ruleList[key].ToString();
                tokenTypeTemplate.Append($"        {key},\n");
                ruleTemplate.Append(
                    $"            ruleDictionary.Add(\"{key}\", new Regex(\"{value}\", RegexOptions.Compiled));\n");
            }
            tokenTypeTemplate.Remove(tokenTypeTemplate.Length - 2, 1);

            var skipListTemplate = new StringBuilder();
            foreach (var skipRule in skipList) {
                skipListTemplate.Append($"            skipRuleList.Add(\"{skipRule}\");\n");
            }

            //read the lexer template
            var template = File.ReadAllText("LexerTemplate.txt", Encoding.UTF8);
            File.WriteAllText(path, string.Format(template, namespaceName,
                tokenTypeTemplate,
                lexRuleName,
                lexRuleName,
                skipListTemplate,
                ruleTemplate
                ));
        }
    }
}