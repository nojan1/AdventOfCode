using System.Collections.Generic;

namespace Day9
{
    class Group
    {
        public int Start { get; set; }
        public int Stop { get; set; }
        public List<Group> Children { get; private set; } = new List<Group>();

        public int GetTotalScore(int baseScore = 1)
        {
            return baseScore + Children.Sum(c => c.GetTotalScore(baseScore + 1));
        }
    }
}
