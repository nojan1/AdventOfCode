using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalUsage = 0;
            int totalRibbon = 0;

            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var parts = line.Trim().Split('x').Select(p => Convert.ToInt32(p)).OrderBy(d => d).ToArray();

                int l = parts[0];
                int w = parts[1];
                int h = parts[2];

                int side1 = l * w;
                int side2 = w * h;
                int side3 = h * l;

                int usage = (2 * side1) + (2 * side2) + (2 * side3) + Math.Min(side1, Math.Min(side2, side3));
                totalUsage += usage;

                totalRibbon += (parts[0] * 2) + (parts[1] * 2) + (l * w * h);
            }

            Console.WriteLine("Total wrapping usage is {0} sqf", totalUsage);
            Console.WriteLine("Total ribbon length {0}", totalRibbon);
            Console.Read();
        }
    }
}
