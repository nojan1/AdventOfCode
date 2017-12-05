using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Day19
{
    class FinderProgress { }
    class WorkerProgress : FinderProgress
    {
        public int TaskId { get; set; }
        public int NumRetry { get; set; }
    }

    class NumStepProgress : FinderProgress
    {
        public int NumStep { get; set; }
    }

    class RandomFinder : GenericParallelTaskRunnerBaseWithProgress<int, int, FinderProgress>
    {
        private const int NUM_THREADS = 8;

        private List<KeyValuePair<string, string>> _replacements;
        private string _start;
        private string _target;

        private int _seed = 0;
        private int _lowestNumSteps = int.MaxValue;

        public RandomFinder(List<KeyValuePair<string, string>> replacements, string start, string target, IProgress<FinderProgress> progress) 
            : base(NUM_THREADS, progress)
        {
            _replacements = replacements;
            _start = start;
            _target = target;
        }

        protected override int CreateTaskParameter()
        {
            return _seed++;
        }

        protected override void OnTaskFinished(int seed, int numSteps)
        {
            if (numSteps < _lowestNumSteps)
            {
                _lowestNumSteps = numSteps;

                ReportProgress(new NumStepProgress { NumStep = numSteps });
            }
        }

        protected override int Worker(int taskId, int seed)
        {
            int numRetry = 0;
            int numSteps = 0;
            var rnd = new Random(seed);
            string workingMolecule = _target;

            while (workingMolecule != _start)
            {
                var possibleReplacements = _replacements.Where(x => workingMolecule.Contains(x.Value)).ToArray();

                if (possibleReplacements.Any())
                {
                    var randomReplacement = possibleReplacements[rnd.Next(0, possibleReplacements.Length)];

                    var indexOfValue = workingMolecule.IndexOf(randomReplacement.Value);
                    if (indexOfValue == -1)
                        throw new WtfException("But you where there a second ago :(");

                    workingMolecule = $"{workingMolecule.Substring(0, indexOfValue)}{randomReplacement.Key}{workingMolecule.Substring(indexOfValue + randomReplacement.Value.Length)}";
                    numSteps++;

                }

                if (numSteps >= 10000 || !possibleReplacements.Any())
                {
                    workingMolecule = _target;
                    numSteps = 0;

                    ReportProgress(new WorkerProgress
                    {
                        TaskId = taskId,
                        NumRetry = ++numRetry
                    });
                }
            }

            return numSteps;
        }
    }

    class Program
    {

        static List<string> NotSubSub(List<KeyValuePair<string, string>> replacements, string molecule)
        {
            var distänctMolykules = new List<string>();

            foreach (var replacement in replacements)
            {
                int indexOfKey = -1;

                while ((indexOfKey = molecule.IndexOf(replacement.Key, indexOfKey + 1)) > -1)
                {
                    foreach (var substitute in replacements.Where(x => x.Key == replacement.Key))
                    {
                        string newMolecule = $"{molecule.Substring(0, indexOfKey)}{substitute.Value}{molecule.Substring(indexOfKey + replacement.Key.Length)}";
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

            var replacements = new List<KeyValuePair<string, string>>();

            data.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(x =>
                {
                    replacements.Add(new KeyValuePair<string, string>(x[0].Trim(), x[1].Trim()));
                });

            var molecule = data.Last();


            int numSteps = -1;
            Dictionary<int, int> workerStats = new Dictionary<int, int>();

            var progressHandler = new Progress<FinderProgress>(progress =>
            {
                if (progress is NumStepProgress)
                {
                    numSteps = ((NumStepProgress)progress).NumStep;
                }
                else if (progress is WorkerProgress)
                {
                    var workerProgress = (WorkerProgress)progress;
                    workerStats[workerProgress.TaskId] = workerProgress.NumRetry;
                }
            });

            var randomFinder = new RandomFinder(replacements, "e", molecule, progressHandler);

            Task.Run(() =>
            {
                while (!randomFinder.RunComplete)
                {
                    Console.Clear();
                    Console.WriteLine($"Lowest number of steps: {numSteps}");
                    Console.WriteLine("=========================================");
                    foreach (var worker in workerStats.Keys.ToList())
                    {
                        Console.WriteLine($"Worker: {worker} - {workerStats[worker]} retries");
                    }

                    Task.Delay(500).Wait();
                }
            });

            randomFinder.RunToEnd();
        }
    }
}
