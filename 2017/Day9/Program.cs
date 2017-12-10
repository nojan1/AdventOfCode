using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day9
{
    class Group
    {
        public int Score { get; private set; }

        public List<Group> Children { get; private set; } = new List<Group>();

        public Group(int score, string stream)
        {
            Score = score;

            int childrenGroupStartIndex = stream.IndexOf('{');
            int childrenGroupStopIndex = stream.LastIndexOf('{');
        }

        public int GetTotalScore()
        {
            return Score + Children.Sum(c => c.GetTotalScore());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var stream = File.ReadAllText("input.txt");

            var rootGroup = new Group(1, stream.Substring(1, stream.Length - 2));
            var totalScore = rootGroup.GetTotalScore();

            Console.WriteLine($"The total is {totalScore}");
        }
    }
}
