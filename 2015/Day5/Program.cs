using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var vowels = "aeiou".ToCharArray();
            var bad = new string[] { "ab", "cd", "pq", "xy" };
            int nice = 0;

            foreach (var str in File.ReadAllLines("Input.txt"))
            {
                if(str.ToCharArray().Count(c => vowels.Contains(c)) >= 3 
                    && LetterRepeats(str) 
                    && !bad.Any(b => str.Contains(b))){

                    nice++;
                }
            }

            Console.WriteLine("{0} strings are nice", nice);
            Console.Read();
        }

        static bool LetterRepeats(string text)
        {
            foreach(char letter in text)
            {
                string search = letter.ToString() + letter.ToString();
                if (text.Contains(search))
                {
                    return true;
                }                
            }

            return false;
        }
    }
}
