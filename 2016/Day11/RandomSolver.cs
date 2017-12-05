using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Combinatorics.Collections;

namespace Day11
{
    class Solution
    {
        public int NumSteps { get; set; } = 0;
        public List<string> FloorStates { get; set; } = new List<string>();

        public Solution(FloorCollection floors)
        {
            FloorStates.Add(floors.ToString());
        }
    }

    class RandomSolver : GenericParallelTaskRunnerBaseWithProgress<int, Solution, Solution>
    {
        const int NUM_THREADS = 8;

        public Solution BestResult { get; private set; }

        private FloorCollection _floors;
        private int _seed = 0;

        public RandomSolver(FloorCollection floors, IProgress<Solution> progress) : base(NUM_THREADS, progress)
        {
            _floors = floors;
        }

        protected override int CreateTaskParameter()
        {
            if (_seed == int.MaxValue)
                RunComplete = true;

            return _seed++;
        }

        protected override void OnTaskFinished(int seed, Solution result)
        {
            if(BestResult == null || result.NumSteps < BestResult.NumSteps)
            {
                BestResult = result;
                ReportProgress(result);
            }
        }

        protected override Solution Worker(int taskId, int seed)
        {
            var rnd = new Random(seed);
            
            int currentFloor = 0;
            var floors = _floors.Clone();
            var solution = new Solution(floors);

            while (!floors.AllComponentsOnTopFloor())
            {
                var availableMoves = GenerateMoves(floors, currentFloor);
                var move = availableMoves[rnd.Next(0, availableMoves.Count)];

                move.Do(floors, currentFloor);

                if (floors.WillResultInCatastrophicMeltdown())
                {
                    currentFloor = 0;
                    floors = _floors.Clone();
                    solution = new Solution(floors);
                    continue;
                }

                solution.NumSteps++;
                solution.FloorStates.Add(floors.ToString());

                currentFloor = move.ToFloor;
            }

            return solution;
        }

        private List<Move> GenerateMoves(FloorCollection floors, int currentFloor)
        {
            var moves = new List<Move>();

            if (currentFloor < 3)
            {
                //Single component up
                moves.AddRange(floors.Floors[currentFloor].Components.Select(c => new Move { Components = new List<Component> { c }, ToFloor = currentFloor + 1 }));

                //Dual component up
                moves.AddRange(new Combinations<Component>(floors.Floors[currentFloor].Components, 2).Select(c => new Move { Components = c, ToFloor = currentFloor + 1 }));
            }

            if (currentFloor > 0)
            {
                //Single component down
                moves.AddRange(floors.Floors[currentFloor].Components.Select(c => new Move { Components = new List<Component> { c }, ToFloor = currentFloor - 1 }));

                //Dual component down
                moves.AddRange(new Combinations<Component>(floors.Floors[currentFloor].Components, 2).Select(c => new Move { Components = c, ToFloor = currentFloor - 1 }));
            }

            return moves;
        }
    }
}
