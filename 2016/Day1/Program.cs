using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    enum DirectionChange
    {
        Left = -1,
        Right = 1
    }

    enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    class DirectionalCommand
    {
        public DirectionChange DirectionChange { get; set; }
        public int Length { get; set; }
    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Position;
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return (X.ToString() + ":" + Y.ToString()).GetHashCode();
        }

        public Position Clone()
        {
            return new Position
            {
                X = X,
                Y = Y
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var commands = File.ReadAllText("input.txt").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(t => t.Trim())
                                                     .Select(t =>
                                                     {
                                                         return new DirectionalCommand
                                                         {
                                                             DirectionChange = t[0] == 'R' ? DirectionChange.Right : DirectionChange.Left,
                                                             Length = Convert.ToInt32(t.Substring(1))
                                                         };
                                                     });

            //commands = new DirectionalCommand[]
            //{
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 2 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 2 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 2 }
            //};

            //commands = new DirectionalCommand[]
            //{
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 2 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Left, Length = 3 }
            //};

            //commands = new DirectionalCommand[]
            //{
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 5 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Left, Length = 5 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 5 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 3 }
            //};

            //commands = new DirectionalCommand[]
            //{
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 8 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 4 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 4 },
            //    new DirectionalCommand {DirectionChange = DirectionChange.Right, Length = 8 }
            //};

            var previousPositions = new List<Position>();
            var currentDirection = Direction.North;
            var position = new Position { X = 0, Y = 0 };

            foreach(var command in commands)
            {
                int directionInt = (int)currentDirection + (int)command.DirectionChange;
                if (directionInt > 3)
                    currentDirection = Direction.North;
                else if (directionInt < 0)
                    currentDirection = Direction.West;
                else
                    currentDirection = (Direction)directionInt;

                for (int i = 0; i < command.Length; i++)
                {
                    switch (currentDirection)
                    {
                        case Direction.North:
                            position.Y += 1;
                            break;
                        case Direction.East:
                            position.X += 1;
                            break;
                        case Direction.South:
                            position.Y -= 1;
                            break;
                        case Direction.West:
                            position.X -= 1;
                            break;
                    }

                    if (previousPositions.Contains(position))
                    {
                        int distance = position.X + position.Y;

                        Console.WriteLine("Block distance: " + distance.ToString());
                        Console.ReadKey();

                        return;
                    }

                    previousPositions.Add(position.Clone());
                }
            }
        }
    }
}
