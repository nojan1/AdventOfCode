using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var rulesetA = new Func<string, bool>[]
            {
                new Func<string, bool>(s => s.Count(c => "aeiou".Contains(c)) >= 3),
                new Func<string, bool>(s => s.Any(c => s.Contains(c.ToString() + c.ToString()))),
                new Func<string, bool>(s => !new string[] { "ab", "cd", "pq", "xy" }.Any(b => s.Contains(b)))
            };

            var rulesetB = new Func<string, bool>[]
            {
                new Func<string, bool>(s => Regex.IsMatch(s, @"(..).*?\1")),
                new Func<string, bool>(s => Regex.IsMatch(s, @"(.)(.)\1"))
            };

            var nice = File.ReadAllLines("Input.txt")
                           .Count(s => rulesetB.All(r => r.Invoke(s)));

            Console.WriteLine("{0} strings are nice", nice);
            Console.Read();
        }
    }
}
