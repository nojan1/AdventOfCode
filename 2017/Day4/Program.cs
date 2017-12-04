using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = File.ReadAllLines("input.txt")
                            .Select(l => l.Split(' '))
                            .Select(x => x.GroupBy(s => s))
                            .Count(x => NoAnagrams(x.Select(g => g.Key).ToList()) && x.All(g => g.Count() == 1));

            Console.WriteLine($"The number of correct passwords is {count}");
        }

        private static bool NoAnagrams(List<string> words)
        {
            return !words.Any(w => new Permutations<char>(w.ToCharArray()).Any(x => words.Where(w2 => w != w2).Contains(string.Concat(x))) );
        }
    }
}
