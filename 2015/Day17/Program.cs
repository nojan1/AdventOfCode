using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Container
    {
        public int Capacity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var containers = File.ReadAllLines("DockerThing.txt").Select(x => new Container { Capacity = Convert.ToInt32(x) }).ToList();

            int lowestCount = int.MaxValue;
            int count = 0;
            for (int i = 2; i <= containers.Count; i++)
            {
                var containerCombinations = new Combinations<Container>(containers, i, GenerateOption.WithoutRepetition);
                foreach(var combo in containerCombinations)
                {
                    if (combo.Sum(x => x.Capacity) == 150) { 
                        count++;

                        if (combo.Count < lowestCount)
                            lowestCount = combo.Count;
                    }
                }
            }

            Console.WriteLine($"Number is {count}");

            var newCount = new Combinations<Container>(containers, lowestCount, GenerateOption.WithoutRepetition).Count(x => x.Sum(w => w.Capacity) == 150);
            Console.WriteLine($"The new number is {newCount}");
        }
    }
}
