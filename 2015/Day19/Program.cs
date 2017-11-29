using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        static int SubsubInner(Dictionary<string, List<string>> replacements, string molecule, string target, int chainLength, List<string> previousMolecules, List<int> chainLengths, string previousReplacement = "")
        {
            foreach (var replacement in replacements)
            {
                var indexOfKey = molecule.IndexOf(replacement.Key);
                if (indexOfKey > -1)
                {
                    foreach (var substitute in replacement.Value)
                    {
                        if (previousReplacement != "" && substitute.Contains(previousReplacement))
                            continue;

                        string newMolecule = $"{molecule.Substring(0, indexOfKey)}{substitute}{molecule.Substring(indexOfKey + replacement.Key.Length)}";

                        if (newMolecule == target)
                            return chainLength;

                        if (!previousMolecules.Contains(newMolecule))
                        {
                            previousMolecules.Add(newMolecule);

                            var length = SubsubInner(replacements, newMolecule, target, chainLength + 1, previousMolecules, chainLengths, replacement.Key);
                            if(length > -1)
                                chainLengths.Add(length);
                        }
                    }
                }
            }

            return -1;
        }

        static List<int> SubSub(Dictionary<string, List<string>> replacements, string molecule, string target)
        {
            var chainLengths = new List<int>();
            SubsubInner(replacements, molecule, target, 0, new List<string>(), chainLengths);

            return chainLengths;
        }

        static List<string> NotSubSub(Dictionary<string, List<string>> replacements, string molecule)
        {
            var distänctMolykules = new List<string>();

            foreach (var replacement in replacements)
            {
                int indexOfKey = -1;

                while ((indexOfKey = molecule.IndexOf(replacement.Key, indexOfKey + 1)) > -1)
                {
                    foreach (var substitute in replacement.Value)
                    {
                        string newMolecule = $"{molecule.Substring(0, indexOfKey)}{substitute}{molecule.Substring(indexOfKey + replacement.Key.Length)}";
                        if (!distänctMolykules.Contains(newMolecule))
                        {
                            distänctMolykules.Add(newMolecule);
                        }
                    }
                }
            }

            return distänctMolykules;
        }

        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Molekul.txt");

            var replacements = new Dictionary<string, List<string>>();

            data.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(x =>
                {
                    var key = x[0].Trim();

                    var list = replacements.ContainsKey(key) ? replacements[key] : new List<string>();
                    list.Add(x[1].Trim());

                    replacements[key] = list;
                });

            var startingMolecule = data.Last();

            var chainLengths = SubSub(replacements, "e", startingMolecule);

            //var someList = NotSubSub(replacements, startingMolecule);

            //if(someList.Count >= 674)
            //    Console.ForegroundColor = ConsoleColor.Red;

            //Console.WriteLine($"Something something == {someList.Count}");
        }
    }
}
