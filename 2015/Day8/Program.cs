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
        static string CustomUnEscape(string data)
        {
            data = data.Substring(1, data.Length - 2);

            data = data.Replace(@"\\", @"S");
            data = data.Replace("\\\"", "\"");
            data = Regex.Replace(data, @"(\\x[\da-f]{2})", "-");

            return data;
        }

        static string CustomEscape(string data)
        {
            data = data.Substring(1, data.Length - 2);

            data = data.Replace(@"\\", @"SSSS");
            data = data.Replace("\\\"", "|||\"");
            data = Regex.Replace(data, @"(\\x[\da-f]{2})", @"||xaa");

            return "\"|\"" + data + "|\"\"";
        }

        static void Main(string[] args)
        {
            int countRaw = 0, countUnEscaped = 0, countEscaped = 0;

            foreach(var line in File.ReadAllLines("input.txt").Select(t => t.Trim())){
                countRaw += line.Length;
                countUnEscaped += CustomUnEscape(line).Length;

                countEscaped += CustomEscape(line).Length;

                Console.WriteLine(line + " => " + CustomEscape(line));
            }

            Console.WriteLine($"The diff is {countEscaped - countRaw}");
        }
    }
}
