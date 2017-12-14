using Common;
using Day10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Square
    {
        public (int Row, int Column) Position { get; set; }

        public List<Square> RegionMembers { get; private set; } = new List<Square>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = "hxtvlmkl";

            var rowHashes = Enumerable.Range(0, 128)
                .Select(x =>
                {
                    var value = $"{input}-{x}";
                    var hash = KnotHasher.CalculateDenseHash(value);

                    return String.Concat(hash.Select(c =>
                    {
                        var number = Convert.ToUInt32(c.ToString(), 16);
                        var binary = Convert.ToString(number, 2).PadLeft(4, '0');
                        return binary;
                    }));
                })
                .ToArray();

            var usageCount = String.Concat(rowHashes).Count(c => c == '1');
            Console.WriteLine($"There are {usageCount} squares used");

            var coords = new List<(int Row, int Column)>();
            for (int row = 0; row < rowHashes.Length; row++)
            {
                for (int column = 0; column < rowHashes[0].Length; column++)
                {
                    if (rowHashes[row][column] == '1')
                        coords.Add((row, column));

                }
            }

            var regions = new List<List<(int Row, int Column)>>();
            while (coords.Any())
            {
                var regionCoords = new List<(int Row, int Column)>();
                BuildNeighbours((coords.First().Row, coords.First().Column), coords, regionCoords);
                regions.Add(regionCoords);
            }
            
            Console.WriteLine($"There are {regions.Count} regions"); 
        }

        private static void BuildNeighbours((int Row, int Column) centerAround, List<(int Row, int Column)> coords, List<(int Row, int Column)> regionCoords)
        {
            if (centerAround.Row < 0 || centerAround.Column < 0 || centerAround.Row > 127 || centerAround.Column > 127)
                return;

            if (regionCoords.Contains(centerAround))
                return;

            if (coords.Any(c => c.Row == centerAround.Row && c.Column == centerAround.Column)) { 
                regionCoords.Add(centerAround);
                coords.RemoveAll(c => c.Row == centerAround.Row && c.Column == centerAround.Column);
            }
            else
            {
                return;
            }

            BuildNeighbours((centerAround.Row - 1, centerAround.Column), coords, regionCoords);
            BuildNeighbours((centerAround.Row, centerAround.Column + 1), coords, regionCoords);
            BuildNeighbours((centerAround.Row + 1, centerAround.Column), coords, regionCoords);
            BuildNeighbours((centerAround.Row, centerAround.Column - 1), coords, regionCoords);
        }
    }
}
