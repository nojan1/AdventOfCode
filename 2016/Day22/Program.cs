using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
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
                if(goalDrive == null)
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

            PrintGridToFile(driveGrid, "grid.txt");


        }


    }
}
