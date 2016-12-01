using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class House
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NumVisits { get; set; } = 0;
    }

    class PresentGiver
    {
        public int giverX { get; set; }
        public int giverY { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var instructions = new Queue<char>(File.ReadAllText("input.txt"));

            var houses = new List<House>()
            {
                new House
                {
                    X = 0,
                    Y = 0,
                    NumVisits = 2
                }
            };

            var santa = new PresentGiver
            {
                giverX = 0,
                giverY = 0
            };

            var roboSanta = new PresentGiver
            {
                giverX = 0,
                giverY = 0
            };

            while(instructions.Count > 0)
            {
                FollowInstruction(santa, houses, instructions.Dequeue());
                FollowInstruction(roboSanta, houses, instructions.Dequeue());
            }

            Console.WriteLine("{0} houses got at least one present", houses.Count);
            Console.Read();
        }

        static void FollowInstruction(PresentGiver presentGiver, List<House> houses, char instruction)
        {
            switch (instruction)
            {
                case '^':
                    presentGiver.giverY--;
                    break;
                case '>':
                    presentGiver.giverX++;
                    break;
                case 'v':
                    presentGiver.giverY++;
                    break;
                case '<':
                    presentGiver.giverX--;
                    break;
            }

            var houseAtPosition = houses.SingleOrDefault(h => h.X == presentGiver.giverX && h.Y == presentGiver.giverY);
            if (houseAtPosition != null)
            {
                houseAtPosition.NumVisits++;
            }
            else
            {
                houses.Add(new House
                {
                    X = presentGiver.giverX,
                    Y = presentGiver.giverY,
                    NumVisits = 1
                });
            }
        }
    }
}
