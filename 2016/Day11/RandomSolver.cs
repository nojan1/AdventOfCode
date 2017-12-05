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

        public RandomSolver(FloorCollection floors, int allowedMax, IProgress<Solution> progress) : base(NUM_THREADS, progress)
        {
            BestResult = new Solution(floors) { NumSteps = allowedMax };

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
            int lastFloor = -1;
            var floors = _floors.Clone();
            var solution = new Solution(floors);
            Move selectedMove = null;

            while (!floors.AllComponentsOnTopFloor())
            {
                var availableMoves = GenerateMoves(floors, currentFloor);
                var legalMoves = FilterBadMoves(availableMoves, floors, currentFloor, lastFloor, selectedMove);

                if (!legalMoves.Any() || solution.NumSteps > BestResult.NumSteps)
                {
                    currentFloor = 0;
                    floors = _floors.Clone();
                    solution = new Solution(floors);
                    lastFloor = -1;
                    continue;
                }

                selectedMove = legalMoves[rnd.Next(0, legalMoves.Count)];
                selectedMove.Do(floors, currentFloor);

                solution.NumSteps++;
                solution.FloorStates.Add(floors.ToString());

                lastFloor = currentFloor;
                currentFloor = selectedMove.ToFloor;
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

        private List<Move> FilterBadMoves(List<Move> moves, FloorCollection floors, int currentFloor, int lastFloor, Move lastMove)
        {
            foreach(var move in moves.ToList())
            {
                //if(lastFloor != -1 && 
                //   move.ToFloor == lastFloor &&
                //   lastMove.Components.Count == move.Components.Count &&
                //   lastMove.Components.All(c => move.Components.Any(c2 => c.Equals(c2) /*c.GetType() == c2.GetType() && c.Element == c2.Element*/)))
                //{
                //    moves.Remove(move);
                //    continue;
                //}

                var floorsCopy = floors.Clone();
                move.Do(floorsCopy, currentFloor);

                if (floorsCopy.WillResultInCatastrophicMeltdown())
                {
                    moves.Remove(move);
                }
            }

            return moves;
        }
    }
}
