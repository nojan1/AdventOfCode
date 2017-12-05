using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static int FollowJumps(int[] jumpsArg, bool stepOne)
        {
            var jumps = jumpsArg.ToArray(); //Make copy
            int numSteps = 0;
            int pointer = 0;

            while (pointer >= 0 && pointer < jumps.Length)
            {
                if (stepOne)
                {
                    pointer += jumps[pointer]++;
                }
                else
                {
                    var offset = jumps[pointer];
                    jumps[pointer] += jumps[pointer] >= 3 ? -1 : 1;

                    pointer += offset;
                }

                numSteps++;
            }

            return numSteps;
        }

        static void Main(string[] args)
        {
            var jumps = File.ReadAllLines("jumps.txt").Select(x => Convert.ToInt32(x)).ToArray();

            var numSteps = FollowJumps(jumps, false);
            Console.WriteLine($"The number of steps was: {numSteps}");
        }
    }
}
