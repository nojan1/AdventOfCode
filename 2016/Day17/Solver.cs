using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class Solver
    {
        private List<string> solutions;
        private string _passcode;

        public Solver(string passcode)
        {
            _passcode = passcode;
        }

        public ICollection<string> Solve(int x, int y)
        {
            solutions = new List<string>();

            SolveRecursive(new Room(x, y, "", _passcode), new StringBuilder());

            return solutions;
        }

        private string[] directionConstants = new string[] { "U", "R", "D", "L" };
        private bool SolveRecursive(Room room, StringBuilder pathBuilder)
        {
            for (int i = 0; i < directionConstants.Length; i++)
            {
                if (room.Doors[i])
                {
                    int newX = room.X;
                    int newY = room.Y;

                    if (i == 0)
                    {
                        newY--;
                    }
                    else if (i == 1)
                    {
                        newX++;
                    }
                    else if (i == 2)
                    {
                        newY++;
                    }
                    else if (i == 3)
                    {
                        newX--;
                    }

                    var pathBuilderCopy = pathBuilder.ToString();

                    pathBuilder.Append(directionConstants[i]);
                    if (newX == 3 && newY == 3)
                    {
                        if (!solutions.Contains(pathBuilder.ToString()))
                        {
                            solutions.Add(pathBuilder.ToString());
                        }

                        pathBuilder.Clear();
                        pathBuilder.Append(pathBuilderCopy);
                        continue;
                    }

                    if (!SolveRecursive(new Room(newX, newY, pathBuilder.ToString(), _passcode), pathBuilder))
                    {
                        pathBuilder.Clear();
                        pathBuilder.Append(pathBuilderCopy);
                    }
                }
            }

            return false;
        }
    }
}
