using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        private static readonly Dictionary<char, Direction> _translation = new Dictionary<char, Direction>
            {
                { 'U', Direction.North },
                { 'R', Direction.East },
                { 'D', Direction.South },
                { 'L', Direction.West }
            };

        static string GetCode(char[,] keypad, (int X, int Y) start, IEnumerable<string> instructions)
        {
            var codeBuilder = new StringBuilder();
            var finger = new MovingEntity { Position = start };
            
            foreach(var line in instructions)
            {
                line.Select(c => _translation[c])
                    .ToList()
                    .ForEach(d =>
                    {
                        finger.CurrentDirection = d;

                        finger.MoveCurrentDirection(1, pos => pos.X >= 0 && 
                                                              pos.Y >= 0 && 
                                                              pos.X < keypad.GetLength(1) && 
                                                              pos.Y < keypad.GetLength(0) && 
                                                              keypad[pos.Y, pos.X] != ' ');
                    });

                codeBuilder.Append(keypad[finger.Position.Y, finger.Position.X]);
            }

            return codeBuilder.ToString();
        }


        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
           
            var keypadA = new char[,] { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };
            var codeA = GetCode(keypadA, (1, 1), input);

            var keypadB = new char[,] { { ' ', ' ', '1', ' ', ' ' }, { ' ', '2', '3', '4', ' ' }, { '5', '6', '7', '8', '9' }, { ' ', 'A', 'B', 'C', ' ' }, { ' ', ' ', 'D', ' ', ' ' } };
            var codeB = GetCode(keypadB, (0, 2), input);

            Console.WriteLine($"The code for A is {codeA}");
            Console.WriteLine($"....And the real (weird) code for B is {codeB}, but hey easter bunnies are loonies");
        }
    }
}
