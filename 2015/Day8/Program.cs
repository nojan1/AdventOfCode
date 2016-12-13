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
            data = data.Substring(1, data.Length - 2);

            data = data.Replace(@"\\", @"\");
            data = data.Replace("\\\"", "\"");
            data = Regex.Replace(data, @"(\\x[\da-f]{2})", "-");
            //data = Regex.Replace(data, @"""(.*?)""", @"\1");

            return data;
        }

        static void Main(string[] args)
        {
            int countEscaped = 0, countUnEscaped = 0;

            foreach(var line in File.ReadAllLines("input.txt").Select(t => t.Trim())){
                countUnEscaped += line.Length;
                countEscaped += CustomEscape(line).Length;

                Console.WriteLine(line + " => " + CustomEscape(line));
            }

            Console.WriteLine($"The diff is {countUnEscaped - countEscaped}");
        }
    }
}
