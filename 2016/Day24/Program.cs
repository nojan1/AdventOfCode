using Combinatorics.Collections;
using Day13;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class HvacMaze : IMaze
    {
        public List<Coordinate> Locations { get; set; }

        private bool[,] walls;

        public HvacMaze(string inputFile)
        {
            var locations = Enumerable.Range(0, 20).Select<int, Coordinate>(x => null).ToList();
            var lines = File.ReadAllLines(inputFile);

            walls = new bool[lines.Length, lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        walls[y, x] = true;
                        continue;
                    }

                    int number;
                    if (int.TryParse(lines[y][x].ToString(), out number))
                    {
                        locations[number] = new Coordinate(x, y);
                    }
                }
            }

            Locations = locations.TakeWhile(x => x != null).ToList();
        }

        public bool IsWall(int x, int y)
        {
            return walls[y, x];
        }
    }

    class Program
    {
        static int GetLengthOfClosestPath(PathFinder pathFinder, List<Coordinate> coordinatesToVisit, bool includeReturn)
        {
            var weightCache = new ConcurrentDictionary<Coordinate, List<WeightedCoordinate>>();
            Parallel.ForEach(coordinatesToVisit, coord =>
            {
                var weights = pathFinder.FindAllPathsFrom(coord);
                //weights.PrintToFile($"weights-{coord.X}x{coord.Y}.txt");
                weightCache.TryAdd(coord, weights);
            });

            var startingCoordinate = coordinatesToVisit.First();
            coordinatesToVisit = coordinatesToVisit.Skip(1).ToList();

            var paths = new Permutations<Coordinate>(coordinatesToVisit, GenerateOption.WithoutRepetition);
            var distances = new ConcurrentQueue<int>();

            Parallel.ForEach(paths, path =>
            {
                int distance = 0;
                var current = new Coordinate(startingCoordinate.X, startingCoordinate.Y);

                while (path.Any())
                {
                    distance += weightCache[current].First(c => c.X == path.First().X && c.Y == path.First().Y).Count;

                    current = path.First();
                    path.RemoveAt(0);
                }

                if(includeReturn)
                    distance += weightCache[current].First(c => c.X == startingCoordinate.X && c.Y == startingCoordinate.Y).Count;

                distances.Enqueue(distance);
            });

            return distances.Min(); ;
        }

        static void Main(string[] args)
        {
            var maze = new HvacMaze("map.txt");
            var pathFinder = new PathFinder(maze);

            var length = GetLengthOfClosestPath(pathFinder, maze.Locations, true);

            Console.WriteLine($"The shortest path is {length}");
        }
    }
}
