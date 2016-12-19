using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string passcode = "qljzarfv"; //"rrrbmfta";

            var solver = new Solver(passcode);
            var solutions = solver.Solve(0, 0);

            Console.WriteLine($"The shortest path is {solutions.FirstOrDefault(s => s.Length == solutions.Select(x => x.Length).Min())}");
            Console.WriteLine($"The length of the longest part is {solutions.Select(x => x.Length).Max()}");
        }
    }
}
