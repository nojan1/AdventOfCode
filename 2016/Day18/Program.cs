using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static ICollection<string> GetRows(string input, int num)
        {
            var rows = new List<string>() { input };

            for (int y = 0; y < (num - 1); y++)
            {
                var rowBuilder = new StringBuilder();
                for (int x = 0; x < rows[y].Length; x++)
                {
                    var left = (x > 0 ? rows[y][x - 1] : '.') == '^';
                    var center = rows[y][x] == '^';
                    var right = (x < rows[y].Length - 1 ? rows[y][x + 1] : '.') == '^';

                    if ((left && center && !right) ||
                        (center && right && !left) ||
                        (left && !center && !right) ||
                        (right && !center && !left))
                    {
                        rowBuilder.Append('^');
                    }
                    else
                    {
                        rowBuilder.Append('.');
                    }
                }

                rows.Add(rowBuilder.ToString());
            }

            return rows;
        }

        static void Main(string[] args)
        {
            string input = ".^^^.^.^^^^^..^^^..^..^..^^..^.^.^.^^.^^....^.^...^.^^.^^.^^..^^..^.^..^^^.^^...^...^^....^^.^^^^^^^";

            var rows = GetRows(input, 400000);

            var numSafe = rows.Sum(r => r.Count(c => c == '.'));
            Console.WriteLine($"There are {numSafe} safe tiles");
        }
    }
}
