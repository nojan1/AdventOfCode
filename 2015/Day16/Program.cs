using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Aunt
    {
        public int Number { get; private set; }
        public Dictionary<string, int> Properties { get; private set; }

        public Aunt(string dataLine)
        {
            var parts = dataLine.Split(' ');

            Number = Convert.ToInt32(parts[1].TrimEnd(':'));

            Properties = new Dictionary<string, int>
            {
                {  parts[2].TrimEnd(':'), Convert.ToInt32(parts[3].TrimEnd(',')) },
                {  parts[4].TrimEnd(':'), Convert.ToInt32(parts[5].TrimEnd(',')) },
                {  parts[6].TrimEnd(':'), Convert.ToInt32(parts[7].TrimEnd(',')) }
            };
        }
    }

    class Mfcsam
    {
        private ICollection<Aunt> _aunts;

        public Mfcsam(ICollection<Aunt> aunts)
        {
            _aunts = aunts;
        }

        public Aunt GetMatchingAunt(Dictionary<string, int> properties)
        {
            var matchingAunts = new List<Aunt>();

            var moreThen = new string[] { "cats", "trees" };
            var lesserThen = new string[] { "pomeranians", "goldfish" };

            foreach (var aunt in _aunts)
            {
                if (!aunt.Properties.Where(p => moreThen.Contains(p.Key))
                                    .All(p => properties[p.Key] < p.Value))
                    continue;

                if(!aunt.Properties.Where(p => lesserThen.Contains(p.Key))
                                    .All(p => properties[p.Key] > p.Value))
                    continue;

                if (!aunt.Properties.Where(p => !moreThen.Contains(p.Key) && !lesserThen.Contains(p.Key))
                                .All(p => properties[p.Key] == p.Value))
                    continue;

                matchingAunts.Add(aunt);
            }

            return matchingAunts.Single();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var aunts = File.ReadAllLines("AllTheSues.txt").Select(x => new Aunt(x)).ToList();
            var mfcsam = new Mfcsam(aunts);

            var evidence = new Dictionary<string, int>
            {
                {"children", 3},
                {"cats", 7},
                {"samoyeds", 2},
                {"pomeranians", 3},
                {"akitas", 0},
                {"vizslas", 0},
                {"goldfish", 5},
                {"trees", 3},
                {"cars", 2 },
                {"perfumes", 1}
            };

            var theAunt = mfcsam.GetMatchingAunt(evidence);
            Console.WriteLine($"And the winner is... aunt number {theAunt.Number}");
        }
    }
}
