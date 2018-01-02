using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var floors = new FloorState("part2-input.txt");

            var stateSolver = new StateBasedSolver();
            var count = stateSolver.Solve(floors);

            Console.WriteLine($"The number of steps required is {count}");
        }
    }
}
