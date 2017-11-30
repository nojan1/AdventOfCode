using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Collections.Concurrent;

namespace Day20
{
    class HouseFinder : GenericParallelTaskRunnerBase<int, int>
    {
        private const int NUM_THREADS = 8;

        private int _currentHouseNum = 1;
        private int _targetPresentCount;
        private List<int> _housesWithEnoughPresents = new List<int>();

        public int Winner { get => _housesWithEnoughPresents.Min(); }

        public HouseFinder(int target) : base(NUM_THREADS) {
            _targetPresentCount = target;
        }

        protected override int CreateTaskParameter()
        {
            return _currentHouseNum++;
        }

        protected override void OnTaskFinished(int houseNum, int numPresents)
        {
            if(numPresents >= _targetPresentCount)
            {
                _housesWithEnoughPresents.Add(houseNum);
                RunComplete = true;
            }
        }

        private ConcurrentDictionary<int, int> _elfUsage = new ConcurrentDictionary<int, int>();

        protected override int Worker(int houseNum)
        {
            int numPresents = 0;

            for (int elfNum = 1; elfNum <= houseNum; elfNum++)
            {
                if (houseNum % elfNum == 0)
                {
                    if (_elfUsage.ContainsKey(elfNum - 1) && _elfUsage[elfNum - 1] == 50)
                        continue;

                    _elfUsage[elfNum - 1] = _elfUsage.ContainsKey(elfNum - 1) ? _elfUsage[elfNum - 1] + 1 : 1;

                    numPresents += 11 * elfNum;
                }
            }

            return numPresents;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = 33100000;
            var finder = new HouseFinder(input);
            finder.RunToEnd();

            Console.WriteLine($"The house is number {finder.Winner}"); //Time 4:24
        }
    }
}
