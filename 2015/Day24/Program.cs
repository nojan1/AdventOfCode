using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Day24
{
    class Result
    {
        public long QuantumEntanglement { get; set; }
        public List<long>[] Compartments { get; set; }

        public bool IsBetterThan(Result other)
        {
            return Compartments[0].Count < other.Compartments[0].Count ||
                    (Compartments[0].Count == other.Compartments[0].Count && QuantumEntanglement < other.QuantumEntanglement);
        }
    }

    class RandomWeightAllocator : GenericParallelTaskRunnerBaseWithProgress<int, Result, Result>
    {
        const int NUM_THREADS = 8;

        private List<int> _weights;
        private int _seed = 0;

        public Result BestResult { get; private set; }

        public RandomWeightAllocator(List<int> weights, IProgress<Result> progress) : base(NUM_THREADS, progress)
        {
            _weights = weights;
        }

        protected override int CreateTaskParameter()
        {
            if (_seed == int.MaxValue)
                RunComplete = true;

            return _seed++;
        }

        protected override void OnTaskFinished(int seed, Result result)
        {
            if (result == null || result.QuantumEntanglement < 0)
                return;


            if (BestResult == null || result.IsBetterThan(BestResult))
            {
                BestResult = result;
                ReportProgress(result);
            }
        }

        protected override Result Worker(int taskId, int seed)
        {
            var rnd = new Random(seed);
            var weights = new Queue<int>(_weights.OrderBy(x => rnd.Next()));
            var compartments = Enumerable.Range(0, 3).Select(x => new List<long>()).ToArray();

            while (weights.Any())
            {
                compartments[rnd.Next(0, compartments.Length)].Add(weights.Dequeue());
            }

            var sums = compartments.Select(x => x.Sum()).ToArray();

            if (sums[0] == sums[1] && sums[1] == sums[2] &&
                compartments[0].Count <= compartments[1].Count && compartments[0].Count <= compartments[2].Count)
            {
                return new Result
                {
                    Compartments = compartments,
                    QuantumEntanglement = compartments[0].Aggregate((a, b) => a * b)
                };
            }
            else
                return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var weights = File.ReadAllLines("input.txt").Select(x => Convert.ToInt32(x.Trim())).ToList();

            Func<List<long>, int, string> elementOrBlank = (arr, index) =>
            {
                if (index > arr.Count - 1)
                    return "".PadLeft(6);

                return arr[index].ToString().PadLeft(6);
            };

            var qeProgress = new Progress<Result>(res =>
            {

                Console.Clear();
                Console.WriteLine($"New low quantum entanglement: {res.QuantumEntanglement}");
                Console.WriteLine("--------------------------");

                for (int i = 0; i < res.Compartments.Max(x => x.Count); i++)
                {
                    Console.WriteLine($"{elementOrBlank(res.Compartments[0], i)}{elementOrBlank(res.Compartments[1], i)}{elementOrBlank(res.Compartments[2], i)}");
                }
            });

            var allocator = new RandomWeightAllocator(weights, qeProgress);
            allocator.RunToEnd();
        }
    }
}
