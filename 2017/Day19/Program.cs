using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = File.ReadAllLines("input.txt").Select(l => l.ToList()).ToList();

            var letters = new List<char>();
            var foundTheEnd = false;
            int stepCount = 0;

            (int y, int x) pos = (0, grid[0].IndexOf('|'));
            (int yDir, int xDir) direction = (1, 0);

            while (!foundTheEnd)
            {

                switch (grid[pos.y][pos.x])
                {
                    case '+':
                        if(direction.xDir == 0)
                        {
                            //Vertical
                            if(grid[pos.y][pos.x - 1] == '-')
                            {
                                pos = (pos.y, pos.x - 1);
                                direction = (0, -1);
                            }
                            else if(grid[pos.y][pos.x + 1] == '-')
                            {
                                pos = (pos.y, pos.x + 1);
                                direction = (0, 1);
                            }
                            else
                            {
                                throw new WtfException($"Nowhere to go from { pos }");
                            }
                        }
                        else
                        {
                            //Horizontal
                            if(grid[pos.y - 1][pos.x] == '|')
                            {
                                pos = (pos.y - 1, pos.x);
                                direction = (-1, 0);
                            }
                            else if(grid[pos.y + 1][pos.x] == '|')
                            {
                                pos = (pos.y + 1, pos.x);
                                direction = (1, 0);
                            }
                            else
                            {
                                throw new WtfException($"Nowhere to go from {pos}");
                            }
                        }

                        stepCount++;
                        break;
                    case ' ':
                        foundTheEnd = true;
                        break;
                    default:
                        if(grid[pos.y][pos.x] != '|' && grid[pos.y][pos.x] != '-')
                            letters.Add(grid[pos.y][pos.x]);

                        pos = (pos.y + direction.yDir, pos.x + direction.xDir);
                        stepCount++;

                        break;
                }
            }

            var letterString = string.Concat(letters);
            Console.WriteLine($"The letters seen are '{letterString}' with a total of {stepCount} steps");
        }
    }
}
