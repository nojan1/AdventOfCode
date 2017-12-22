using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    public enum InfectionGrade
    {
        Clean = 0,
        Weakened = 1,
        Infected = 2,
        Flagged = 3
    }

    abstract class VirusCarrierBase
    {
        protected Dictionary<(int y, int x), InfectionGrade> _infectedNodes = new Dictionary<(int y, int x), InfectionGrade>();
        protected (int y, int x) _center;

        private int _numBursts;

        public VirusCarrierBase(string[] initialState, int numBursts)
        {
            for (int y = 0; y < initialState.Length; y++)
            {
                for (int x = 0; x < initialState[y].Length; x++)
                {
                    if (initialState[y][x] == '#')
                        _infectedNodes[(y, x)] = InfectionGrade.Infected;
                }
            }

            _center = (initialState.Length / 2, initialState[0].Length / 2);
            _numBursts = numBursts;
        }

        public int Infect()
        {
            int numInfections = 0;
            (int y, int x) position = _center;
            (int yDir, int xDir) direction = (-1, 0);

            for (int burst = 0; burst < _numBursts; burst++)
            {
                var node = _infectedNodes.ContainsKey(position) ? _infectedNodes[position] : InfectionGrade.Clean;
                OnBurst(node, ref direction, ref position, ref numInfections);

                position = (position.y + direction.yDir, position.x + direction.xDir);
            }

            return numInfections;
        }

        protected abstract void OnBurst(InfectionGrade node, ref (int yDir, int xDir) direction, ref (int y, int x) position, ref int numInfections);

        protected (int yDir, int xDir) Turn((int yDir, int xDir) current, int turnDirection)
        {
            var directions = new List<(int yDir, int xDir)>() { (-1, 0), (0, 1), (1, 0), (0, -1) };

            var index = directions.IndexOf(current) + turnDirection;

            while (index < 0)
                index+= directions.Count;

            return directions[index % directions.Count];
        }
    }

    class VirusCarrierOne : VirusCarrierBase
    {
        public VirusCarrierOne(string[] initialState) : base(initialState, 10000) { }

        protected override void OnBurst(InfectionGrade node, ref (int yDir, int xDir) direction, ref (int y, int x) position, ref int numInfections)
        {
            if (node == InfectionGrade.Infected)
            {
                _infectedNodes[position] = InfectionGrade.Clean;
                direction = Turn(direction, 1);
            }
            else
            {
                numInfections++;
                _infectedNodes[position] = InfectionGrade.Infected;
                direction = Turn(direction, -1);
            } 
        }
    }

    class VirusCarrierTwo : VirusCarrierBase
    {
        public VirusCarrierTwo(string[] initialState) : base(initialState, 10000000) { }

        protected override void OnBurst(InfectionGrade node, ref (int yDir, int xDir) direction, ref (int y, int x) position, ref int numInfections)
        {
            switch (node)
            {
                case InfectionGrade.Clean:
                    direction = Turn(direction, -1);
                    break;
                case InfectionGrade.Infected:
                    direction = Turn(direction, 1);
                    break;
                case InfectionGrade.Flagged:
                    direction = Turn(direction, 2);
                    break;
            }

            var infectionInt = (int)node + 1;
            _infectedNodes[position] = (InfectionGrade)(infectionInt > 3 ? 0 : infectionInt);

            if (_infectedNodes[position] == InfectionGrade.Infected)
                numInfections++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var part1 = new VirusCarrierOne(lines);
            var numInfections1 = part1.Infect();
            Console.WriteLine($"Part1: A total of {numInfections1} bursts caused infections");

            var part2 = new VirusCarrierTwo(lines);
            var numInfections2 = part2.Infect();
            Console.WriteLine($"Part2: A total of {numInfections2} bursts caused infections");
        }
    }
}
