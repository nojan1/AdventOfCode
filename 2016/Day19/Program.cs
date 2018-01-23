using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Elf
    {
        public int ElfNum { get; set; }
        public int Presents { get; set; } = 1;

        public Elf(int num)
        {
            ElfNum = num;
        }

        public override string ToString()
        {
            return $"Elf: {ElfNum} ({Presents})";
        }
    }

    class Program
    {
        static void A(List<Elf> elves, int numElves)
        {
            while (true)
            {
                for (int i = 0; i < elves.Count; i++)
                {
                    if (elves[i].Presents == 0)
                        continue;

                    var elfToStealFrom = (i == elves.Count - 1 ? 0 : i + 1);
                    while (elves[elfToStealFrom].Presents == 0)
                    {
                        elfToStealFrom++;

                        if (elfToStealFrom > elves.Count - 1)
                            elfToStealFrom = 0;
                    }

                    elves[i].Presents += elves[elfToStealFrom].Presents;
                    elves[elfToStealFrom].Presents = 0;

                    if (elves[i].Presents == numElves)
                    {
                        Console.WriteLine($"Elf number {elves[i].ElfNum} got all the presents");
                        return;
                    }
                }
            }
        }

        static void B(List<Elf> elvesArg, int numElves)
        {
            int i = 0;
            var elves = new List<Elf>(elvesArg);
            int lastCheck = -1;
            int lastCount = -1;

            while (elves.Count > 1)
            {
                var elfToStealFromIndex = (i + elves.Count / 2) % elves.Count;
                var elfToStealFrom = elves[elfToStealFromIndex];

                elves.RemoveAt(elfToStealFromIndex);

                if (elfToStealFromIndex > i)
                    i++;

                if (i > elves.Count - 1)
                    i = 0;

                if (elves.Count % 1000 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Elves remaining: {elves.Count}");

                    if (lastCheck != -1)
                    {
                        var timeSpent = (Environment.TickCount - lastCheck) / 1000d;
                        var removed = lastCount - elves.Count;
                        var removedPerSecond = removed / timeSpent;

                        Console.WriteLine($"Speed: {removedPerSecond:0.0} elves/sec");
                    }

                    var percentageDone = ((((long)numElves - (long)elves.Count) * 1000) / (long)numElves) / 10d;
                    Console.WriteLine($"Done: {percentageDone}%");

                    lastCheck = Environment.TickCount;
                    lastCount = elves.Count;
                }
            }

            var lastElf = elves.Single();
            Console.WriteLine($"Elf number {lastElf.ElfNum} got all the presents");
            return;
        }

        static void Main(string[] args)
        {
            var numElves = 3004953;

            var elves = Enumerable.Range(0, numElves).Select(i => new Elf(i + 1)).ToList();

            //A(elves, numElves);
            //B(elves, numElves);

            var circularCollection = new CircularLinkedCollection<Elf>(elves);
            var node = circularCollection.FirstNode;

            while (circularCollection.Count > 1)
            {
                node.Value.Presents += node.Opposite.Value.Presents;
                circularCollection.Remove(node.Opposite);

                node = node.Next;
            }
        }
    }
}
