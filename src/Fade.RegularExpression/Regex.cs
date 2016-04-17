using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fade.Common;

namespace Fade.RegularExpression
{
    public class Regex
    {
        private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        public Regex(RegexState start, RegexState end) {
            Start = start;
            End = end;
            End.IsFinalStates = true;
        }

        public RegexState End { get; }

        public RegexState Start { get; }

        public static Regex FromPatternString(string pattern) {
            if (RegexCache.ContainsKey(pattern)) {
                return RegexCache[pattern];
            }
            var lexer = new RegexLexer(
                new Cache<TextReader, char>(new CharCacheSource(new StringReader(pattern)))
                );
            var parser = new RegexParser(lexer);
            var regex = parser.Parse();
            RegexCache.Add(pattern, regex);
            return regex;
        }

        public bool IsMatch(string matchString) {
            var currentStates = new HashSet<RegexState>();
            AddState(Start, currentStates);
            foreach (var chr in matchString) {
                var nextStates = new HashSet<RegexState>();
                var stateList = from state in currentStates
                    where state.Transitions.Keys.Contains(chr)
                    select state.Transitions[chr];
                foreach (var transState in stateList) {
                    AddState(transState, nextStates);
                }
                currentStates = nextStates;
            }
            return currentStates.Any(state => state.IsFinalStates);
        }

        public override string ToString() {
            return $"{Start} => {End}";
        }

        private static void AddState(RegexState state, ISet<RegexState> stateSet) {
            stateSet.Add(state);
            foreach (var eps in state.Epsilon) {
                AddState(eps, stateSet);
            }
        }
    }
}