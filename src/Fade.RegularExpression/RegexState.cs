using System.Collections.Generic;

namespace Fade.RegularExpression
{
    public class RegexState
    {
        public bool IsFinalStates { get; set; }
        public Dictionary<char, RegexState> Transitions { get; } = new Dictionary<char, RegexState>();
        public HashSet<RegexState> Epsilon { get; } = new HashSet<RegexState>();
#if DEBUG
        private static int _stateCount;
        private readonly int _count;

        public override string ToString() {
            return $"S{_count}";
        }

        public RegexState() {
            _count = _stateCount++;
        }

#endif
    }
}