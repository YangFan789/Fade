using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fade.RegularExpressions
{
    public class Regex
    {
        private static readonly Dictionary<TokenType, HandlerDelegate> HandlerDictionary =
            new Dictionary<TokenType, HandlerDelegate> {
                [TokenType.Char] = HandleChar,
                [TokenType.Concat] = HandleConcat,
                [TokenType.Alt] = HandleAlt,
                [TokenType.Star] = HandleRep,
                [TokenType.QuestionMark] = HandleQmark,
                [TokenType.Plus] = HandleRep
            };

        private static readonly Stack<Regex> RegexStack = new Stack<Regex>();

        private Regex(State start, State end) {
            Start = start;
            End = end;
            end.IsEnd = true;
        }

        private static int StateCount { get; set; }
        private State End { get; }

        private string Pattern { get; set; }
        private State Start { get; }

        public static Regex Compile(string p) {
            Lexer.Init(p);
            Parser.Init();
            var tokens = Parser.Parse();
            foreach (var token in tokens) {
                Handle(token);
            }

            Debug.Assert(RegexStack.Count != 1, "RegexStack.Count != 1");
            var regex = RegexStack.Pop();
            regex.Pattern = p;
            return regex;
        }

        public override bool Equals(object obj) {
            var objA = obj as Regex;
            if (objA != null) {
                return Equals(objA);
            }
            return false;
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = End?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Start?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public bool Match(string s) {
            var currentStates = new HashSet<State>();
            AddState(Start, currentStates);
            foreach (var c in s) {
                var nextStates = new HashSet<State>();
                foreach (var transState in from state
                    in currentStates
                    where state.Transitions.ContainsKey(c.ToString())
                    select state.Transitions[c.ToString()]) {
                    AddState(transState, nextStates);
                }
                currentStates = nextStates;
            }

            return currentStates.Any(state => state.IsEnd);
        }

        public override string ToString() {
            return Pattern;
        }

        private static void AddState(State state, ISet<State> stateSet) {
            if (stateSet.Contains(state)) {
                return;
            }
            stateSet.Add(state);
            foreach (var eps in state.Epsilon) {
                AddState(eps, stateSet);
            }
        }

        private static State CreateState() {
            return new State(StateCount++);
        }

        private static void Handle(Token token) {
            HandlerDictionary[token.Type](token);
        }

        private static void HandleAlt(Token token) {
            var n2 = RegexStack.Pop();
            var n1 = RegexStack.Pop();
            var s0 = CreateState();
            s0.Epsilon = new List<State> {n1.Start, n2.Start};
            var s3 = CreateState();
            n1.End.Epsilon.Add(s3);
            n2.End.Epsilon.Add(s3);
            n1.End.IsEnd = false;
            n2.End.IsEnd = false;
            var nfa = new Regex(s0, s3);
            RegexStack.Push(nfa);
        }

        private static void HandleChar(Token token) {
            var s0 = CreateState();
            var s1 = CreateState();
            s0.Transitions.Add(token.Value, s1);
            var nfa = new Regex(s0, s1);
            RegexStack.Push(nfa);
        }

        private static void HandleConcat(Token token) {
            var n2 = RegexStack.Pop();
            var n1 = RegexStack.Pop();
            n1.End.IsEnd = false;
            n1.End.Epsilon.Add(n2.Start);
            var nfa = new Regex(n1.Start, n2.End);
            RegexStack.Push(nfa);
        }

        private static void HandleQmark(Token token) {
            var n1 = RegexStack.Pop();
            n1.Start.Epsilon.Add(n1.End);
            RegexStack.Push(n1);
        }

        private static void HandleRep(Token token) {
            var n1 = RegexStack.Pop();
            var s0 = CreateState();
            var s1 = CreateState();
            s0.Epsilon = new List<State> {n1.Start};
            if (token.Type == TokenType.Star) {
                s0.Epsilon.Add(s1);
            }
            n1.End.Epsilon.Add(s1);
            n1.End.Epsilon.Add(n1.Start);
            n1.End.IsEnd = false;
            var nfa = new Regex(s0, s1);
            RegexStack.Push(nfa);
        }

        private bool Equals(Regex regex) {
            return Pattern == regex.Pattern;
        }

        private delegate void HandlerDelegate(Token token);
    }
}