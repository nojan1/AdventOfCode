using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Position
    {
        public double X { get; set; }
        public double Y { get; set; }

        public void Move(string direction)
        {
            switch (direction)
            {
                case "n":
                    Y -= 1;
                    break;
                case "ne":
                    Y -= 0.5;
                    X += 0.5;
                    break;
                case "se":
                    Y += 0.5;
                    X += 0.5;
                    break;
                case "s":
                    Y += 1;
                    break;
                case "sw":
                    Y += 0.5;
                    X -= 0.5;
                    break;
                case "nw":
                    Y -= 0.5;
                    X -= 0.5;
                    break;
                default:
                    throw new WtfException("You said there would not be any other directions!!!");

            }
        }

        public override string ToString()
        {
            return $"{X};{Y}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllText("input.txt").Split(',').ToList();
            var numSteps = GetNumSteps(inputs);
            var maxSteps = numSteps.Max();

            Console.WriteLine($"The number of steps is {numSteps.Last()} with a maximum of {maxSteps}");
        }

        static List<int> GetNumSteps(List<string> inputs)
        {
            var numSteps = new List<int>();
            var position = new Position { X = 0, Y = 0 };

            inputs.ForEach(x =>
            {
                position.Move(x);
                numSteps.Add(GetStepsFromZero(position));
            });

            return numSteps;
        }

        static int GetStepsFromZero(Position position)
        {
            var current = new Position { X = 0, Y = 0 };
            int numSteps = 0;

            while (position.X != current.X || position.Y != current.Y)
            {
                var direction = FindDirection(current, position);
                current.Move(direction);

                numSteps++;
            }

            return numSteps;
        }

        static string FindDirection(Position current, Position target)
        {
            if (current.X == target.X)
            {
                if (current.Y > target.Y)
                {
                    return "n";
                }
                else
                {
                    return "s";
                }
            }
            else
            {
                if (current.Y > target.Y)
                {
                    if (current.X > target.X)
                    {
                        return "nw";
                    }
                    else
                    {
                        return "ne";
                    }
                }
                else
                {
                    if (current.X > target.X)
                    {
                        return "sw";
                    }
                    else
                    {
                        return "se";
                    }
                }
            }
        }

    }
}
