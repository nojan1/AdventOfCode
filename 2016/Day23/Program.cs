using Day12;
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
            var cpu = new BunnyPU(File.ReadAllLines("assembunny.txt").Select(t => t.Trim()).ToArray());
            cpu.GetRegister("a").Value = 12;
            cpu.RunToEnd();
        }
    }
}
