using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {
        static void Swap(Dictionary<char, int> programsLookup, char char1, char char2)
        {
            var indexBackup = programsLookup[char1];

            programsLookup[char1] = programsLookup[char2];
            programsLookup[char2] = indexBackup;
        }

        static void Main(string[] args)
        {
            var part = DayPart.Two;
            var input = File.ReadAllText("input.txt").Split(',');

            var previousOrders = new List<string>();

            var programsLookup = new Dictionary<char, int>();
            for (int i = 0; i < 16; i++)
                programsLookup[(char)('a' + i)] = i;

            previousOrders.Add("abcdefghijklmnop");

            var to = part == DayPart.One ? 1 : 1000000000;
            for (int i = 0; i < to; i++)
            {
                foreach (var x in input)
                {
                    var code = x[0];
                    var arguments = x.Substring(1).Split('/');

                    switch (code)
                    {
                        case 's':
                            int spinSize = Convert.ToInt32(arguments[0]);

                            foreach(var key in programsLookup.Keys.ToList())
                            {
                                programsLookup[key] = (programsLookup[key] + spinSize) % programsLookup.Count;
                            }

                            break;
                        case 'x':
                            var pos1 = Convert.ToInt32(arguments[0]);
                            var pos2 = Convert.ToInt32(arguments[1]);

                            var char1 = programsLookup.First(k => k.Value == pos1).Key;
                            var char2 = programsLookup.First(k => k.Value == pos2).Key;

                            Swap(programsLookup, char1, char2);

                            break;
                        case 'p':
                            Swap(programsLookup, arguments[0][0], arguments[1][0]);

                            break;
                    }
                }

                var order = string.Concat(programsLookup.OrderBy(x => x.Value).Select(x => x.Key));
                var lastSeen = previousOrders.IndexOf(order);

                if (lastSeen != -1)
                {
                    Console.WriteLine($"Order for iteration {i} was the same as {lastSeen}");

                    var correctOrderIndex = to % (i + 1);
                    previousOrders.Add(previousOrders[correctOrderIndex]);

                    break;
                }

                previousOrders.Add(order);
            }

            Console.WriteLine($"The final order is '{previousOrders.Last()}'");
        }
    }
}
