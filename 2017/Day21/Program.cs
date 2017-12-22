using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var part = DayPart.Two;

            var rules = File.ReadAllLines("input.txt")
                .Select(l => l.Split(new string[] { "=>" }, StringSplitOptions.None))
                .ToDictionary(x => new DynamicGrid<char>(x[0].Trim().Split('/')), x => new DynamicGrid<char>(x[1].Trim().Split('/')));

            var image = new DynamicGrid<char>(new string[] { ".#.", "..#", "###" });

            for (int i = 0; i < (part == DayPart.One ? 5 : 18); i++)
            {
                int squareSize = image.Width % 2 == 0 ? 2 : 3;
                int numSquares = image.Width / squareSize;

                var squares = EnumerableExtensions.Sequence(0, image.Width, squareSize)
                    .Select(y => EnumerableExtensions.Sequence(0, image.Height, squareSize)
                        .Select(x =>
                        {
                            var subGrid = image.SubGrid(y, x, squareSize, squareSize);

                            var matchingReplacement = FindReplacementMatch(subGrid, rules);
                            if (matchingReplacement == null)
                                throw new WtfException("This shouldn't happen yo!");

                            return matchingReplacement;
                        }).ToList()).ToList();

                image = new DynamicGrid<char>(squares);
            }

            var numPixelsOn = image.Count(c => c.Value == '#');
            Console.WriteLine($"There are {numPixelsOn} pixels that are on");
        }

        private static DynamicGrid<char> FindReplacementMatch(DynamicGrid<char> subGrid, Dictionary<DynamicGrid<char>, DynamicGrid<char>> rules)
        {
            Func<DynamicGrid<char>, DynamicGrid<char>, bool> MatchesThroughRotation = (left, right) =>
          {
              for (int a = 0; a <= 270; a += 90)
              {
                  if (left == right)
                      return true;

                  left = left.Rotate();
              }

              return false;
          };

            var matches = rules.Where(r =>
            {
                if (r.Key.Width != subGrid.Width)
                    return false;

                if (MatchesThroughRotation(r.Key, subGrid))
                    return true;

                if (MatchesThroughRotation(r.Key.Flip(false), subGrid))
                    return true;

                if (MatchesThroughRotation(r.Key.Flip(true), subGrid))
                    return true;

                return false;
            });

            if (!matches.Any())
                return null;

            return matches.First().Value;
        }
    }
}
