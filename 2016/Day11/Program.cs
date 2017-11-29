﻿using System;
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
            var floors = new FloorCollection("sampleInput.txt");
            var solutions = new Solver().Solve(floors);

            Console.WriteLine($"The shortest amount of moves is {solutions.Min(s => s.Count)}");
        }
    }
}