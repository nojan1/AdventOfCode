using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    class FromToDistance
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Distance { get; set; }
    }

    class LocationDatabase
    {

        public ICollection<string> KnownLocations
        {
            get
            {
                return fromToDistances.Select(x => x.From).Distinct().ToList();
            }
        }

        private List<FromToDistance> fromToDistances;

        public LocationDatabase()
        {
            fromToDistances = new List<FromToDistance>();

            foreach (var line in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                var parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                fromToDistances.Add(new FromToDistance
                {
                    From = parts[0],
                    To = parts[2],
                    Distance = Convert.ToInt32(parts[4])
                });
            }
        }

        public int GetDistance(string from, string to)
        {
            var fromToDistance = fromToDistances.FirstOrDefault(x => (x.From == from && x.To == to));// ||
                                                                                                     // (x.To == from && x.From == to));
            if (fromToDistance == null)
                //throw new Exception($"No distance logged between {from} and {to}");
                return -1;

            return fromToDistance.Distance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var locationDb = new LocationDatabase();
            var distances = GeneratePermutations(locationDb.KnownLocations.ToList()).Select(l =>
            {
                var distance = 0;

                for (int i = 0; i < l.Count - 1; i++)
                {
                    var localDistance = locationDb.GetDistance(l.ElementAt(i), l.ElementAt(i + 1));
                    if (localDistance == -1)
                        return 9999;

                    distance += localDistance;
                }

                return distance;
            }).ToArray();

            Console.WriteLine($"The shortest distance is: {distances.Min()}");
        }

        private static List<List<T>> GeneratePermutations<T>(List<T> items)
        {
            // Make an array to hold the
            // permutation we are building.
            T[] current_permutation = new T[items.Count];

            // Make an array to tell whether
            // an item is in the current selection.
            bool[] in_selection = new bool[items.Count];

            // Make a result list.
            List<List<T>> results = new List<List<T>>();

            // Build the combinations recursively.
            PermuteItems<T>(items, in_selection,
                current_permutation, results, 0);

            // Return the results.
            return results;
        }

        private static void PermuteItems<T>(List<T> items, bool[] in_selection,
    T[] current_permutation, List<List<T>> results,
    int next_position)
        {
            // See if all of the positions are filled.
            if (next_position == items.Count)
            {
                // All of the positioned are filled.
                // Save this permutation.
                results.Add(current_permutation.ToList());
            }
            else
            {
                // Try options for the next position.
                for (int i = 0; i < items.Count; i++)
                {
                    if (!in_selection[i])
                    {
                        // Add this item to the current permutation.
                        in_selection[i] = true;
                        current_permutation[next_position] = items[i];

                        // Recursively fill the remaining positions.
                        PermuteItems<T>(items, in_selection,
                            current_permutation, results,
                            next_position + 1);

                        // Remove the item from the current permutation.
                        in_selection[i] = false;
                    }
                }
            }
        }
    }
}
