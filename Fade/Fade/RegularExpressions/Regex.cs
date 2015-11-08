using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fade.RegularExpressions
{
    public class Regex
    {
        public Regex(State start, State end) {
            Start = start;
            End = end;
            End.IsFinalStates = true;
        }

        public State End { get; }
        
        public State Start { get; }

        private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        public static Regex FromPatternString(string p) {
            if (RegexCache.ContainsKey(p)) {
                return RegexCache[p];
            }
            Lexer.Init(p);
            Parser.Init();
            var regex = Parser.Parse();
            RegexCache.Add(p, regex);
            return regex;
        }

        public override bool Equals(object obj) {
            var objA = obj as Regex;
            return objA != null && Equals(objA);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = End?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Start?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public bool Match(string matchString) {
            var currentStates = new HashSet<State>();
            AddState(Start, currentStates);
            foreach (var chr in matchString) {
                var nextStates = new HashSet<State>();
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
        
        private static void AddState(State state, ISet<State> states) {
            states.Add(state);
            foreach (var eps in state.Epsilon) {
                AddState(eps, states);
            }
        }
    }
}