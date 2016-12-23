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
        public List<int> Presents { get; set; }

        public Elf(int num)
        {
            ElfNum = num;
            Presents = new List<int> { num };
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
                    if (!elves[i].Presents.Any())
                        continue;

                    var elfToStealFrom = (i == elves.Count - 1 ? 0 : i + 1);
                    while (!elves[elfToStealFrom].Presents.Any())
                    {
                        elfToStealFrom++;

                        if (elfToStealFrom > elves.Count - 1)
                            elfToStealFrom = 0;
                    }

                    elves[i].Presents.AddRange(elves[elfToStealFrom].Presents);
                    elves[elfToStealFrom].Presents.Clear();

                    if (elves[i].Presents.Count == numElves)
                    {
                        Console.WriteLine($"Elf number {elves[i].ElfNum} got all the presents");
                        return;
                    }
                }
            }
        }

        static void B(List<Elf> elves, int numElves)
        {
            while (true)
            {
                for (int i = 0; i < elves.Count; i++)
                {
                    //if (!elves[i].Presents.Any())
                    //    continue;

                    var elfToStealFrom = i + Convert.ToInt32(Math.Floor(elves.Count / 2.0));
                    //while (!elves[elfToStealFrom].Presents.Any())
                    //{
                    //    elfToStealFrom++;

                    //    if (elfToStealFrom > elves.Count - 1)
                    //        elfToStealFrom = 0;
                    //}

                    elves[i].Presents.AddRange(elves[elfToStealFrom].Presents);

                    elves[elfToStealFrom].Presents.Clear();
                    elves.RemoveAt(elfToStealFrom);


                    if (elves[i].Presents.Count == numElves)
                    {
                        Console.WriteLine($"Elf number {elves[i].ElfNum} got all the presents");
                        return;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var numElves = 3004953;

            var elves = new List<Elf>();
            for(int i = 0; i < numElves; i++)
            {
                elves.Add(new Elf(i + 1));
            }

            A(elves, numElves);
            //B(elves, numElves);
        }
    }
}
