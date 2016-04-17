using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Fade.RegularExpression;

namespace Fade.Diagnostics
{
    public static class RegexVisualizer
    {
        private static void Draw(RegexState regexState, StringBuilder sb, HashSet<string> set) {
            if (set.Contains(regexState.ToString())) {
                return;
            }
            set.Add(regexState.ToString());
            if (!regexState.IsFinalStates) {
                sb.Append($"    {regexState} [shape=\"circle\"];\n");
            }
            foreach (var transition in regexState.Transitions) {
                sb.Append($"    {regexState} -> {transition.Value}[label=\"{transition.Key}\"];\n");
                Draw(transition.Value, sb, set);
            }
            foreach (var state in regexState.Epsilon) {
                sb.Append($"    {regexState} -> {state}[label=\"ε\"];\n");
                Draw(state, sb, set);
            }
        }

        public static void Visual(Regex re, string path) {
            path = path.Replace('/', '\\');

            var sb = new StringBuilder();
            var set = new HashSet<string>();
            sb.Append("digraph {\n    rankdir = \"LR\";\n    shape=\"circle\";\n");
            sb.Append($"    {re.End} [shape=\"doublecircle\"];\n");
            Draw(re.Start, sb, set);
            sb.Append("}");
            if (!Directory.Exists("Temp")) {
                Directory.CreateDirectory("Temp");
            }
            var dotFilePath = $"Temp\\{Path.GetFileNameWithoutExtension(path)}.dot";
            File.WriteAllText(dotFilePath, sb.ToString());

            var process = new Process();
            var startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "..\\..\\tools\\Graphviz\\dot.exe",
                Arguments = $"-Tpng {dotFilePath} -o {path}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}