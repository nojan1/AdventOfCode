using Combinatorics.Collections;
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
                var places = fromToDistances.Select(x => x.From).ToList();
                places.AddRange(fromToDistances.Select(x => x.To));

                return places.Distinct().ToList();
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
            var fromToDistance = fromToDistances.FirstOrDefault(x => (x.From == from && x.To == to) ||
                                                                     (x.To == from && x.From == to));
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
       
            var distances = new Permutations<string>(locationDb.KnownLocations.ToList(), GenerateOption.WithoutRepetition).Select(l =>
            {
                var distance = 0;

                for (int i = 0; i < l.Count - 1; i++)
                {
                    var localDistance = locationDb.GetDistance(l.ElementAt(i), l.ElementAt(i + 1));
                    if (localDistance == -1)
                        return int.MaxValue;

                    distance += localDistance;
                }

                return distance;
            }).ToArray();

            Console.WriteLine($"The shortest distance is: {distances.Min()}");
            Console.WriteLine($"The longest distance is: {distances.Max()}");
        }
    }
}
