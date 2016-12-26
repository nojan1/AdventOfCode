using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    public static class WeightedCoordinateArrayExtension
    {
        public static void PrintToFile(this List<WeightedCoordinate> coords, string filename)
        {
            int maxY = coords.Max(c => c.Y);
            int maxX = coords.Max(c => c.X);

            using (var writer = new StreamWriter(filename))
            {
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        var weight = coords.FirstOrDefault(c => c.X == x && c.Y == y);
                        if (weight != null)
                        {
                            writer.Write(" " + weight.Count.ToString("d3") + " ");
                        }
                        else
                        {
                            writer.Write("#####");
                        }
                    }

                    writer.WriteLine();
                }
            }
        }
    }

    public interface IMaze
    {
        bool IsWall(int x, int y);
    }

    public class Coordinate : IComparable<Coordinate>
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

        public int CompareTo(Coordinate other)
        {
            return Equals(other) ? 0 : 1;
        }
    }

    public class WeightedCoordinate : Coordinate
    {
        public int Count { get; set; }

        public WeightedCoordinate(int x, int y, int count) : base(x, y)
        {
            Count = count;
        }
    }

    public class Maze : IMaze
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

    public class PathFinder
    {
        private IMaze _maze;

        public PathFinder(IMaze maze)
        {
            _maze = maze;
        }

        public List<WeightedCoordinate> AssignWeight(Coordinate from, Coordinate to)
        {
            var weightedPositions = new List<WeightedCoordinate>() { new WeightedCoordinate(to.X, to.Y, 0) };
            var positionsRequiringCheck = new List<WeightedCoordinate>(weightedPositions);

            List<WeightedCoordinate> possibleNewPositions = null;
            while (positionsRequiringCheck.Any())
            {
                var position = positionsRequiringCheck.First();

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

                positionsRequiringCheck.RemoveAt(0);

                if (possibleNewPositions.Any())
                {
                    weightedPositions.AddRange(possibleNewPositions);
                    positionsRequiringCheck.AddRange(possibleNewPositions);

                    if (from != null && weightedPositions.Contains(from))
                    {
                        return weightedPositions;
                    }
                }
            }

            return weightedPositions;
        }

        public List<WeightedCoordinate> FindAllPathsFrom(Coordinate from)
        {
            return AssignWeight(null, from);
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
