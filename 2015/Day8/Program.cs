using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day8
{
    class Program
    {
        static string CustomEscape(string data)
        {
            data = Regex.Replace(data, @"\\", @"\");
            data = Regex.Replace(data, @"\""", @"""");
            return Regex.Replace(data, @"(\\x\d{2})", "-");      
        }

        static void Main(string[] args)
        {
            int countEscaped = 0, countUnEscaped = 0;

            foreach(var line in File.ReadAllLines("input.txt").Select(t => t.Trim())){
                countUnEscaped += line.Length;
                countEscaped += CustomEscape(line).Length - 2; 
            }

            Console.WriteLine($"The diff is {countUnEscaped - countEscaped}");
        }
    }
}
