using System.Collections.Generic;

namespace Fade.RegularExpressions
{
    internal class State
    {
        public State(int name) {
            Name = name;
            Transitions = new Dictionary<string, State>();
            Epsilon = new List<State>();
        }

        public int Name { get; set; }
        public bool IsEnd { get; set; }
        public Dictionary<string, State> Transitions { get; }
        public List<State> Epsilon { get; set; }
    }
}