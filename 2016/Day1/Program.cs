using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var santa = new MovingEntity();
            var locationsVisited = new List<(int X, int Y)> { (0, 0) };
            (int X, int Y) firstLocationVisitedTwice = (0, 0);

            File.ReadAllText("input.txt")
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList()
                .ForEach(command =>
                {
                    if (command[0] == 'R')
                        santa.TurnRight();
                    else
                        santa.TurnLeft();

                    var numSteps = Convert.ToInt32(command.Substring(1));
                    for (int i = 0; i < numSteps; i++)
                    {
                        santa.MoveCurrentDirection(1);

                        if (locationsVisited.Contains(santa.Position) && firstLocationVisitedTwice.Equals((0, 0)))
                        {
                            firstLocationVisitedTwice = santa.Position;
                        }

                        locationsVisited.Add(santa.Position);
                    }
                });

            var distanceAway = Math.Abs(santa.Position.X) + Math.Abs(santa.Position.Y);
            Console.WriteLine($"Santa is {distanceAway} blocks away");

            var firstDuplicateDistanceAway = Math.Abs(firstLocationVisitedTwice.X) + Math.Abs(firstLocationVisitedTwice.Y);
            Console.WriteLine($"But the first block visited twice is {firstDuplicateDistanceAway} blocks away");
        }
    }
}
