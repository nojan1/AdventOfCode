using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {
        static void Swap(List<char> programs, int index1, int index2)
        {
            var backup = programs[index1];

            programs[index1] = programs[index2];
            programs[index2] = backup;
        }

        static void Main(string[] args)
        {
            var part = DayPart.Two;
            var input = File.ReadAllText("input.txt").Split(',');

            var programs = Enumerable.Range('a', 16).Select(x => (char)x).ToList();

            for (int i = 0; i < (part == DayPart.One ? 1 : 1000000000); i++)
            {
                foreach (var x in input)
                {
                    var code = x[0];
                    var arguments = x.Substring(1).Split('/');

                    switch (code)
                    {
                        case 's':
                            int spinSize = Convert.ToInt32(arguments[0]);

                            int lowerIndex = programs.Count - spinSize;
                            var firstHalf = programs.Skip(lowerIndex);
                            var secondHalf = programs.Take(lowerIndex);

                            programs = Enumerable.Concat(firstHalf, secondHalf).ToList();

                            break;
                        case 'x':
                            var pos1 = Convert.ToInt32(arguments[0]);
                            var pos2 = Convert.ToInt32(arguments[1]);

                            Swap(programs, pos1, pos2);
                            break;
                        case 'p':
                            var index1 = programs.IndexOf(arguments[0][0]);
                            var index2 = programs.IndexOf(arguments[1][0]);

                            Swap(programs, index1, index2);
                            break;
                    }
                }
            }

            var order = string.Concat(programs);
            Console.WriteLine($"The final order is '{order}'");
        }
    }
}
