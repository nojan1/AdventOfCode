using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        const int SIDE = 100;
        const int NUM_STEPS = 100;

        static void Main(string[] args)
        {
            Dictionary<(int, int), bool> lights = new Dictionary<(int, int), bool>();

            var matRisContent = File.ReadAllLines("Matris.txt");
            for (int y = 0; y < SIDE; y++)
            {
                for (int x = 0; x < SIDE; x++)
                {
                    lights[(x, y)] = matRisContent[y][x] == '#';
                }
            }

            lights[(0, 0)] = true;
            lights[(0, SIDE - 1)] = true;
            lights[(SIDE - 1, 0)] = true;
            lights[(SIDE - 1, SIDE - 1)] = true;

            for (int i = 1; i <= NUM_STEPS; i++)
            {
                var state = lights.ToDictionary(x => (x.Key.Item1, x.Key.Item2), x => x.Value);

                for (int y = 0; y < SIDE; y++)
                {
                    for (int x = 0; x < SIDE; x++)
                    {
                        if (x == 0 && y == 0 ||
                           x == 0 && y == SIDE - 1 ||
                           x == SIDE - 1 && y == 0 ||
                           x == SIDE - 1 && y == SIDE - 1)
                            continue;

                        var neighboursOn = CountNeighboursOn(state, x, y);

                        if (state[(x, y)] && (neighboursOn < 2 || neighboursOn > 3))
                        {
                            lights[(x, y)] = false;
                        }
                        else if (!state[(x, y)] && neighboursOn == 3)
                        {
                            lights[(x, y)] = true;
                        }
                    }
                }
            }

            int countOn = lights.Count(x => x.Value);
            Console.WriteLine($"There is {countOn} lights on");
        }

        private static int CountNeighboursOn(Dictionary<(int, int), bool> lights, int x, int y)
        {
            return (x - 1 >= 0 && y - 1 >= 0 && lights[(x - 1, y - 1)] ? 1 : 0) +    //Top-Left
                   (x - 1 >= 0 && lights[(x - 1, y)] ? 1 : 0) +                      //Left
                   (x - 1 >= 0 && y + 1 < SIDE && lights[(x - 1, y + 1)] ? 1 : 0) +   //Bottom-Left
                   (y - 1 >= 0 && lights[(x, y - 1)] ? 1 : 0) +                      //Top
                   (y + 1 < SIDE && lights[(x, y + 1)] ? 1 : 0) +                     //Bottom
                   (x + 1 < SIDE && y - 1 >= 0 && lights[(x + 1, y - 1)] ? 1 : 0) +   //Top-Right
                   (x + 1 < SIDE && lights[(x + 1, y)] ? 1 : 0) +                     //Right
                   (x + 1 < SIDE && y + 1 < SIDE && lights[(x + 1, y + 1)] ? 1 : 0);   //Bottom-Right
        }
    }
}
