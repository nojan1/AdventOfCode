using Day1;
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
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var keyList = new List<char>();
            var keys = new char[] { ' ', ' ', '1', ' ', ' ', ' ', '2', '3', '4', ' ', '5', '6', '7', '8', '9', ' ', 'A', 'B', 'C', ' ', ' ', ' ', 'D', ' ', ' ' };
            var position = new Position { X = 1, Y = 1 };
            var badPositions = new List<Position>
            {
                new Position { X = 0, Y = 0 },
                new Position { X = 1, Y = 0 },
                new Position { X = 3, Y = 0 },
                new Position { X = 4, Y = 0 },

                new Position { X = 0, Y = 1 },
                new Position { X = 4, Y = 1 },

                new Position { X = 0, Y = 3 },
                new Position { X = 4, Y = 3 },

                new Position { X = 0, Y = 4 },
                new Position { X = 1, Y = 4 },
                new Position { X = 3, Y = 4 },
                new Position { X = 4, Y = 4 }
            };

            foreach(var line in input)
            {
                foreach(var command in line)
                {
                    var backupPosition = position.Clone();

                    switch (command)
                    {
                        case 'U':
                            position.Y -= 1;
                            break;
                        case 'R':
                            position.X += 1;
                            break;
                        case 'D':
                            position.Y += 1;
                            break;
                        default:
                        case 'L':
                            position.X -= 1;
                            break;
                    }

                    if (badPositions.Contains(position) || position.X < 0 || position.X > 4 || position.Y < 0 || position.Y > 4)
                    {
                        position = backupPosition;
                    }
                }

                var keyIndex = (position.Y * 5) + position.X;
                keyList.Add(keys[keyIndex]);
            }

            Console.WriteLine($"The code is: {string.Join("", keyList)}");
        }
    }
}
