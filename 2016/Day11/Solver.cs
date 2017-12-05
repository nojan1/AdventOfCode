using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class Move
    {
        public ICollection<Component> Components { get; set; }
        public int ToFloor { get; set; }

        public void Do(FloorCollection floors, int currentFloor)
        {
            foreach (var component in Components)
            {
                floors.Floors[currentFloor].Components.RemoveAll(c => c.Equals(component));
                floors.Floors[ToFloor].Components.Add(component);
            }
        }
    }

    public class Solver
    {
        int numBacktracks, numRecurseCalls;

        private ICollection<ICollection<Move>> solutions;

        public ICollection<ICollection<Move>> Solve(FloorCollection floors)
        {
            numBacktracks = numRecurseCalls = 0;
            solutions = new List<ICollection<Move>>();

            SolveRecursivly(floors.Clone(), 0);

            return solutions;
        }

        private bool SolveRecursivly(FloorCollection floors, int currentFloor, List<Move> moves = null)
        {
            List<Move> localMoves = moves;
            numRecurseCalls++;

            foreach (var move in GenerateMoves(floors, currentFloor))
            {
                if (moves == null)
                    localMoves = new List<Move>();

                if (localMoves.Any(m => m.Components.All(x => move.Components.Any(z => x.Element == z.Element)) && m.ToFloor == move.ToFloor))
                    continue;

                var floorsCopy = floors.Clone();
                move.Do(floorsCopy, currentFloor);

                var movesBackup = new List<Move>(localMoves);

                if (!floorsCopy.WillResultInCatastrophicMeltdown())
                {
                    localMoves.Add(move);

                    if (floorsCopy.AllComponentsOnTopFloor())
                    {
                        solutions.Add(localMoves);

                        localMoves.Clear();
                        localMoves.AddRange(movesBackup);
                        continue;
                        //return true;
                    }

                    if (!SolveRecursivly(floorsCopy, move.ToFloor, localMoves))
                    { 
                        numBacktracks++;

                        localMoves.Clear();
                        localMoves.AddRange(movesBackup);
                    }
                }
            }

            return false;
        }

        private ICollection<Move> GenerateMoves(FloorCollection floors, int currentFloor)
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
