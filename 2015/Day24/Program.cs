using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var weights = File.ReadAllLines("input.txt").Select(x => Convert.ToInt32(x.Trim())).ToList();

            var weightList = weights.OrderBy(x => x).ToList();
            var groups = new List<int>[] { new List<int>(), new List<int>(), new List<int>() };

            while (weightList.Any())
            {
                groups[0].Add(weightList.Last());
                weightList.RemoveAt(weightList.Count - 1);


            }

        }
    }
}
