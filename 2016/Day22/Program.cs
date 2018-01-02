using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    enum Direction {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    class Pair
    {
        public Drive Drive1 { get; set; }
        public Drive Drive2 { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Pair;
            return Drive1 == other.Drive1 && Drive2 == other.Drive2 ||
                   Drive1 == other.Drive2 && Drive2 == other.Drive1;
        }

        public override int GetHashCode()
        {
            return Drive1.GetHashCode() & Drive2.GetHashCode();
        }
    }

    class Drive
    {
        public int GridX { get; set; }
        public int GridY { get; set; }

        public string Name { get; set; }
        public int Size { get; set; }
        public int Used { get; set; }
        public int Avail
        {
            get
            {
                return Size - Used;
            }
        }
    }

    class Program
    {
        static ICollection<Drive> ParseDfOutput(string filename)
        {
            return File.ReadAllLines(filename).Skip(2).Select(t => t.Trim()).Select(line =>
            {
                var parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var drive = new Drive
                {
                    Name = parts[0],
                    Size = Convert.ToInt32(parts[1].Substring(0, parts[1].Length - 1)),
                    Used = Convert.ToInt32(parts[2].Substring(0, parts[2].Length - 1))
                };

                var nameParts = drive.Name.Split('-');
                drive.GridX = Convert.ToInt32(nameParts[1].Substring(1));
                drive.GridY = Convert.ToInt32(nameParts[2].Substring(1));

                return drive;
            }).ToList();
        }

        private static void PrintGridToFile(Drive[,] driveGrid, string filename, Drive goalDrive = null)
        {
            using (var writer = new StreamWriter(filename))
            {
                if (goalDrive == null)
                    goalDrive = driveGrid[0, driveGrid.GetLength(1) - 1];

                for (int y = 0; y < driveGrid.GetLength(0); y++)
                {
                    for (int x = 0; x < driveGrid.GetLength(1); x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            writer.Write("(");
                        }
                        else
                        {
                            writer.Write(" ");
                        }

                        if (visited.Any(v => v.X == x && v.Y == y))
                        {
                            writer.Write("#");
                        }
                        else
                        {
                            if (y == goalDrive.GridY && x == goalDrive.GridX)
                            {
                                writer.Write("G");
                            }
                            else
                            {
                                if (driveGrid[y, x].Used == 0)
                                {
                                    writer.Write("_");
                                }
                                else
                                {
                                    if (driveGrid[y, x].Size < goalDrive.Used)
                                    {
                                        writer.Write("#");
                                    }
                                    else
                                    {
                                        writer.Write(".");
                                    }
                                }
                            }
                        }

                        if (x == 0 && y == 0)
                        {
                            writer.Write(")");
                        }
                        else
                        {
                            writer.Write(" ");
                        }
                    }

                    writer.WriteLine();
                }
            }
        }

        private static Drive[,] ToGrid(ICollection<Drive> drives)
        {
            int maxY = drives.Max(d => d.GridY);
            int maxX = drives.Max(d => d.GridX);
            var retval = new Drive[maxY + 1, maxX + 1];

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    retval[y, x] = drives.Single(d => d.GridX == x && d.GridY == y);
                }
            }

            return retval;
        }
        static List<(int X, int Y)> visited = new List<(int X, int Y)>();
        static void Main(string[] args)
        {
            var drives = ParseDfOutput("input.txt");
            var viablePairs = drives.SelectMany(d1 =>
            {
                if (d1.Used == 0)
                    return new List<Pair>();

                var localPairs = new List<Pair>();

                foreach (var d2 in drives.Where(d => d1 != d && d.Avail >= d1.Used))
                {
                    localPairs.Add(new Pair
                    {
                        Drive1 = d1,
                        Drive2 = d2
                    });
                }

                return localPairs;
            }).Distinct().ToList();
            var driveGrid = ToGrid(drives);

            var targetDrive = drives.Where(d => d.GridY == 0).OrderByDescending(d => d.GridX).First();
            var emptyDrive = drives.First(d => d.Used == 0);

            PrintGridToFile(driveGrid, "grid.txt", targetDrive);

            int numSteps = 0;

            numSteps += MoveTo(driveGrid, (emptyDrive.GridX, emptyDrive.GridY), (targetDrive.GridX - 1, targetDrive.GridY));
            PrintGridToFile(driveGrid, "grid2.txt", targetDrive);

            numSteps += MoveTo(driveGrid, (targetDrive.GridX, targetDrive.GridY), (0, 0));
            PrintGridToFile(driveGrid, "grid3.txt", targetDrive);

            Console.WriteLine($"How about {numSteps} steps?");

        }

        static void Swap(Drive[,] driveGrid, int y1, int x1, int y2, int x2)
        {
            if (driveGrid[y1, x1].Used > driveGrid[y2, x2].Size || driveGrid[y2, x2].Used > driveGrid[y1, x1].Size)
                throw new Exception("Drive to small :/");

            int temp = driveGrid[y1, x1].Used;
            driveGrid[y1, x1].Used = driveGrid[y2, x2].Used;
            driveGrid[y2, x2].Used = temp;
        }

        static int MoveTo(Drive[,] driveGrid, (int X, int Y) current, (int X, int Y) to)
        {
            int steps = 0;
            Direction direction;
            (int X, int Y) tempPosition;
            visited.Add(current);

            while (current.X != to.X || current.Y != to.Y)
            {
                direction = GetDirection(current, to);
                tempPosition = (current.X + MovementFromDirection(direction).MoveX, current.Y + MovementFromDirection(direction).MoveY);

                while (driveGrid[current.Y, current.X].Used > driveGrid[tempPosition.Y, tempPosition.X].Size || 
                       driveGrid[tempPosition.Y, tempPosition.X].Used > driveGrid[current.Y, current.X].Size)
                {
                    var newDirection = ((int)direction) + (current.X > to.X ? 1 : -1);

                    if (newDirection > 3)
                        newDirection = 0;
                    else if (newDirection < 0)
                        newDirection = 3;

                    direction = (Direction)newDirection;

                    tempPosition = (current.X + MovementFromDirection(direction).MoveX, current.Y + MovementFromDirection(direction).MoveY);
                }

                Swap(driveGrid, current.Y, current.X, tempPosition.Y, tempPosition.X);
                current = tempPosition;
                steps++;

                visited.Add(current);
            }

            return steps;
        }

        static Direction GetDirection((int X, int Y) from, (int X, int Y) to)
        {
            if(to.Y == from.Y)
            {
                if(to.X < from.X)
                {
                    return Direction.West;
                }
                else
                {
                    return Direction.East;
                }
            }
            else if(to.Y > from.Y)
            {
                return Direction.South;
            }
            else
            {
                return Direction.North;
            }
        }

        static (int MoveX, int MoveY) MovementFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return (0, -1);
                case Direction.East:
                    return (1, 0);
                case Direction.South:
                    return (0, 1);
                default:
                    return (-1, 0);
            }
        }
    }
}