using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var cpu = new BunnyPU();
            cpu.GetRegister("c").Value = 1;
            cpu.ProcessInstructions(File.ReadAllLines("input.txt").Select(t => t.Trim()).ToArray());

            Console.Write(string.Join(Environment.NewLine, cpu.Registers.Select(r => r.Name + ": " + r.Value.ToString())));
        }
    }
}
