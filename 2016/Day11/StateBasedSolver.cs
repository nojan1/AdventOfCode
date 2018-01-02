using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class StateBasedSolver
    {
        public int Solve(FloorState initialFloors)
        {
            var gScore = new Dictionary<string, int>();
            var closedSet = new List<string>();
            var openSet = new List<FloorState> { initialFloors };

            gScore[initialFloors.Serialized] = 0;

            while (openSet.Any())
            {
                var current = openSet.First();

                if(current.AllComponentsOnTopFloor())
                {
                    return gScore[current.Serialized];
                }

                openSet.Remove(current);
                closedSet.Add(current.Serialized);

                var moves = GenerateMoves(current);
                foreach (var move in moves)
                {
                    var newState = current.Clone();
                    move.Do(newState);

                    if (newState.WillResultInCatastrophicMeltdown())
                        continue;

                    var newStateSerialized = newState.Serialized;

                    if (closedSet.Contains(newStateSerialized))
                        continue;

                    if (!openSet.Select(x => x.Serialized).Contains(newStateSerialized))
                    {
                        openSet.Add(newState);
                    }

                    var newScore = gScore[current.Serialized] + 1;
                    gScore[newStateSerialized] = newScore;
                }

                if(closedSet.Count % 100 == 0 || openSet.Count % 100 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Open set: {openSet.Count}");
                    Console.WriteLine($"Closed set: {closedSet.Count}");
                    Console.WriteLine($"Max steps: {gScore.Values.Max()}");
                }
            }

            return -1;
        }

        private List<Move> GenerateMoves(FloorState floors)
        {
            var moves = new List<Move>();

            if (floors.ElevatorPosition < 3)
            {
                //Single component up
                moves.AddRange(floors.Floors[floors.ElevatorPosition].Components.Select(c => new Move { Components = new List<Component> { c }, ToFloor = floors.ElevatorPosition + 1 }));

                //Dual component up
                moves.AddRange(new Combinations<Component>(floors.Floors[floors.ElevatorPosition].Components, 2).Select(c => new Move { Components = c, ToFloor = floors.ElevatorPosition + 1 }));
            }

            if (floors.ElevatorPosition > 0 && floors.Floors[floors.ElevatorPosition - 1].Components.Any())
            {
                //Single component down
                moves.AddRange(floors.Floors[floors.ElevatorPosition].Components.Select(c => new Move { Components = new List<Component> { c }, ToFloor = floors.ElevatorPosition - 1 }));

                //Dual component down
                moves.AddRange(new Combinations<Component>(floors.Floors[floors.ElevatorPosition].Components, 2).Select(c => new Move { Components = c, ToFloor = floors.ElevatorPosition - 1 }));
            }

            return moves;
        }
    }
}
