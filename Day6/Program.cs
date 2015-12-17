using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var lights = new int[1000, 1000];
            lights.Initialize();

            foreach(var line in File.ReadAllLines("Input.txt"))
            {
                var coordMatches = Regex.Matches(line, @"(\d*,\d*)");
                var from = coordMatches[0].Groups[1].Value.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                var to = coordMatches[1].Groups[1].Value.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                for(int x = from[0]; x <= to[0]; x++)
                {
                    for(int y = from[1]; y <= to[1]; y++)
                    {
                        if (line.Contains("on"))
                        {
                            lights[x, y]++;
                        }else if (line.Contains("toggle"))
                        {
                            lights[x, y] += 2;
                        }
                        else
                        {
                            lights[x, y]--;

                            if (lights[x, y] < 0)
                                lights[x, y] = 0;
                        }
                    }
                }
            }

            var count = 0;

            for (int x = 0; x < 1000; x++)
                for (int y = 0; y < 1000; y++)
                    count += lights[x, y];

            Console.WriteLine("Light count is: " + count.ToString());
            Console.Read();
        }
    }
}
