using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            Coordinate other = (Coordinate)obj;
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return $"{X}x{Y}".GetHashCode();
        }
    }

    class WeightedCoordinate : Coordinate
    {
        public int Count { get; set; }

        public WeightedCoordinate(int x, int y, int count) : base(x, y)
        {
            Count = count;
        }
    }

    class Maze
    {
        private int _designersMagicNumber;

        public Maze(int designersMagicNumber)
        {
            _designersMagicNumber = designersMagicNumber;
        }

        public bool IsWall(int x, int y)
        {
            var value = (x * x + 3 * x + 2 * x * y + y + y * y) + _designersMagicNumber;
            return Convert.ToString(value, 2).Count(c => c == '1') % 2 != 0;
        }
    }

    class PathFinder
    {
        private Maze _maze;

        public PathFinder(Maze maze)
        {
            _maze = maze;
        }

        public List<WeightedCoordinate> AssignWeight(Coordinate from, Coordinate to)
        {
            var weightedPositions = new List<WeightedCoordinate>() { new WeightedCoordinate(to.X, to.Y, 0) };

            while (true)
            {
                List<WeightedCoordinate> possibleNewPositions = null;
                foreach (var position in weightedPositions)
                {
                    possibleNewPositions = new List<WeightedCoordinate>
                    {
                        new WeightedCoordinate(position.X, position.Y + 1, position.Count + 1),
                        new WeightedCoordinate(position.X + 1, position.Y, position.Count + 1),
                        new WeightedCoordinate(position.X, position.Y - 1, position.Count + 1),
                        new WeightedCoordinate(position.X - 1, position.Y, position.Count + 1)
                    };

                    var possibleNewPositionsCopy = possibleNewPositions.ToList();
                    foreach (var possibleNewPosition in possibleNewPositionsCopy)
                    {
                        if (_maze.IsWall(possibleNewPosition.X, possibleNewPosition.Y))
                        {
                            possibleNewPositions.Remove(possibleNewPosition);
                            continue;
                        }

                        if (weightedPositions.Contains(possibleNewPosition))
                        {
                            possibleNewPositions.Remove(possibleNewPosition);
                            continue;
                        }

                        if (possibleNewPosition.X < 0 || possibleNewPosition.Y < 0)
                        {
                            possibleNewPositions.Remove(possibleNewPosition);
                            continue;
                        }
                    }

                    if (possibleNewPositions.Any())
                        break;
                }

                if (possibleNewPositions.Any())
                {
                    weightedPositions.AddRange(possibleNewPositions);

                    if (weightedPositions.Contains(from))
                    {
                        return weightedPositions;
                    }
                }
                else
                {
                    return weightedPositions;
                }

            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var start = new Coordinate(1, 1);
            var destination = new Coordinate(31, 39);

            var maze = new Maze(1358);
            var pathFinder = new PathFinder(maze);

            var weightedPositions = pathFinder.AssignWeight(start, destination);
            var minSteps = weightedPositions.FirstOrDefault(p => p.Equals(start)).Count;

            Console.WriteLine($"The minimum number of steps is {minSteps}");

            weightedPositions = pathFinder.AssignWeight(destination, start);
            var availableSteps = weightedPositions.Where(p => p.Count <= 50);

            Console.WriteLine($"There are {availableSteps.Count()} positions that can be reached within 50 steps");
        }
    }
}
