using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt").Select(x => x.Trim()).ToArray();
            var cpu = new CPU(instructions);

            cpu.GetRegister("a").Value = 1;

            cpu.RunToEnd();

            var value = cpu.GetRegister("b").Value;
            Console.WriteLine($"The value is {value}");
        }
    }
}
