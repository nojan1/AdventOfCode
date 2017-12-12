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
        public ulong QuantumEntanglement => FootCompartment.Aggregate((a, b) => a * b);
        public List<ulong> FootCompartment { get; set; }

        public bool IsBetterThan(Result other)
        {
            return FootCompartment.Count < other.FootCompartment.Count ||
                    (FootCompartment.Count == other.FootCompartment.Count && QuantumEntanglement < other.QuantumEntanglement);
        }
    }

    class RandomWeightAllocator : GenericParallelTaskRunnerBaseWithProgress<int, Result, Result>
    {
        const int NUM_THREADS = 8;
        const int NUM_COMPARTMENTS = 4;

        private List<ulong> _weights;
        private int _seed = int.MaxValue;

        public Result BestResult { get; private set; }

        public RandomWeightAllocator(List<int> weights, IProgress<Result> progress) : base(NUM_THREADS, progress)
        {
            _weights = weights.Select(x => (ulong)x).ToList();
        }

        protected override int CreateTaskParameter()
        {
            if (_seed == 0)
                RunComplete = true;

            return _seed--;
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

            var currentWeight = 0ul;
            var targetWeight = _weights.Aggregate((a,b) => a + b) / NUM_COMPARTMENTS;
            var footCompartment = new List<ulong>();

            foreach(var weight in _weights)
            {
                if(rnd.Next(NUM_COMPARTMENTS) == 0)
                {
                    footCompartment.Add(weight);
                    currentWeight += weight;
                }

                if (currentWeight > targetWeight)
                    return null;
            }

            if (currentWeight != targetWeight)
                return null;

            return new Result
            {
                FootCompartment = footCompartment
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var weights = File.ReadAllLines("input.txt").Select(x => Convert.ToInt32(x.Trim())).ToList();

            Func<List<ulong>, int, string> elementOrBlank = (arr, index) =>
            {
                if (index > arr.Count - 1)
                    return "".PadLeft(6);

                return arr[index].ToString().PadLeft(6);
            };

            var qeProgress = new Progress<Result>(res =>
            {
                Console.Clear();
                Console.WriteLine($"New low quantum entanglement: {res.QuantumEntanglement}");
            });

            var allocator = new RandomWeightAllocator(weights, qeProgress);
            allocator.RunToEnd();
        }
    }
}
