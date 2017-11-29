using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Person
    {
        public string Name { get; set; }
        public Dictionary<string, int> Chemistry { get; set; } = new Dictionary<string, int>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var persons = new List<Person>();

            foreach (var line in File.ReadAllLines("fäcts.txt"))
            {
                var parts = line.Split(' ');

                var person = persons.FirstOrDefault(p => p.Name == parts[0]);
                if (person == null)
                {
                    person = new Person { Name = parts[0] };
                    person.Chemistry["Crazy santa"] = 0;

                    persons.Add(person);
                }

                int change = Convert.ToInt32(parts[3]) * (parts[2] == "lose" ? -1 : 1);
                var otherPerson = parts[10].TrimEnd('.');

                person.Chemistry[otherPerson] = change;
            }

            persons.Add(new Person
            {
                Name = "Crazy santa",
                Chemistry = persons.ToDictionary(p => p.Name, p => 0)
            });

            int bestChemistry = 0;

            var permutations = new Permutations<string>(persons.Select(p => p.Name).ToList());
            foreach(var permutation in permutations)
            {
                var chemistry = CalculateChemistryIndexValue(persons, permutation);

                if (chemistry > bestChemistry)
                    bestChemistry = chemistry;
            }

            Console.WriteLine($"The best chemistry change {bestChemistry}");
        }

        static int CalculateChemistryIndexValue(List<Person> persons, ICollection<string> seatings)
        {
            int chemistryIndexValue = 0;

            for (int i = 0; i < seatings.Count; i++)
            {
                var currentPerson = persons.First(p => p.Name == seatings.ElementAt(i));

                string left = seatings.ElementAt(i == 0 ? seatings.Count - 1 : i - 1);
                string right = seatings.ElementAt(i == seatings.Count - 1 ? 0 : i + 1);

                chemistryIndexValue += currentPerson.Chemistry[left] + currentPerson.Chemistry[right];
            }

            return chemistryIndexValue;
        }
    }
}
