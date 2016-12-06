using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(t => t.Trim()).ToArray();

            var message = "";
            for(int x = 0; x < input[0].Length; x++)
            {
                var dict = new Dictionary<char, int>();
                for(int y = 0; y < input.Length; y++)
                {
                    if (!dict.ContainsKey(input[y][x]))
                    {
                        dict[input[y][x]] = 1;
                    }
                    else
                    {
                        dict[input[y][x]]++;
                    }
                }

               message += dict.FirstOrDefault(d => d.Value == dict.Values.Min()).Key;
            }

            Console.WriteLine($"The message is: '{message}'");
        }
    }
}
