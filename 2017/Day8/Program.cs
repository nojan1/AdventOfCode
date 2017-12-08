using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8
{
    class Registries
    {
        private Dictionary<string, int> _registryValues = new Dictionary<string, int>();

        public int this[string name]
        {
            get
            {
                return _registryValues.ContainsKey(name) ? _registryValues[name] : 0;
            }
            set
            {
                _registryValues[name] = value;
            }
        }

        public int GetHighestValue()
        {
            return _registryValues.Values.Any() ? _registryValues.Values.Max() : 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt").Select(x => x.Trim()).ToArray();
            var registries = new Registries();
            int allTimeHigh = 0;

            foreach(var instruction in instructions)
            {
                var parts = instruction.Split(' ');

                var comparisonRegistryValue = registries[parts[4]];
                var comparisonRightHandValue = Convert.ToInt32(parts[6]);

                if(IsTrue(comparisonRegistryValue, comparisonRightHandValue, parts[5]))
                {
                    var changeAmount = Convert.ToInt32(parts[2]);

                    if (parts[1] == "inc")
                        registries[parts[0]] += changeAmount;
                    else
                        registries[parts[0]] -= changeAmount;
                }

                allTimeHigh = Math.Max(allTimeHigh, registries.GetHighestValue());
            }

            int highestValue = registries.GetHighestValue();
            Console.WriteLine($"The highest value is {highestValue}, with an all time high of {allTimeHigh}");
        }

        private static bool IsTrue(int left, int right, string op)
        {
            switch (op)
            {
                case "<":
                    return left < right;
                case ">":
                    return left > right;
                case "<=":
                    return left <= right;
                case ">=":
                    return left >= right;
                case "!=":
                    return left != right;
                default:
                    return left == right;
            }
        }
    }
}
