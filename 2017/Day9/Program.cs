using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = File.ReadAllText("input.txt");

            var parser = new Parser(stream);
            var rootGroup = parser.GetRootGroup(out int garbageCount);
            var totalScore = rootGroup.GetTotalScore();

            Console.WriteLine($"The total is {totalScore} with a total of {garbageCount} garbage characters");
        }
    }
}
