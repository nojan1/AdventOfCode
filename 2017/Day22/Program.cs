using Common;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
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
        public Dictionary<(int y, int x), InfectionGrade> InfectedNodes { get; private set; } = new Dictionary<(int y, int x), InfectionGrade>();
        protected (int y, int x) _center;

        private int _numBursts;

        public VirusCarrierBase(string[] initialState, int numBursts)
        {
            for (int y = 0; y < initialState.Length; y++)
            {
                for (int x = 0; x < initialState[y].Length; x++)
                {
                    if (initialState[y][x] == '#')
                        InfectedNodes[(y, x)] = InfectionGrade.Infected;
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
                var node = InfectedNodes.ContainsKey(position) ? InfectedNodes[position] : InfectionGrade.Clean;
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
                InfectedNodes[position] = InfectionGrade.Clean;
                direction = Turn(direction, 1);
            }
            else
            {
                numInfections++;
                InfectedNodes[position] = InfectionGrade.Infected;
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
            InfectedNodes[position] = (InfectionGrade)(infectionInt > 3 ? 0 : infectionInt);

            if (InfectedNodes[position] == InfectionGrade.Infected)
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


            var startX = part2.InfectedNodes.Min(x => x.Key.x);
            var startY = part2.InfectedNodes.Min(x => x.Key.y);
            var width = part2.InfectedNodes.Max(x => x.Key.x);
            var height = part2.InfectedNodes.Max(x => x.Key.y);

            var image = new Image<Rgba32>((width + Math.Abs(startX)) * 4, (height + Math.Abs(startY)) * 4);
            image.Mutate(op =>
            {
                var colors = new Rgba32[] { Rgba32.Black, Rgba32.Orange, Rgba32.Red, Rgba32.Yellow };

                for(int y = startY; y < height; y+=4)
                {
                    for(int x = startX; x < width; x+=4)
                    {
                        var state = part2.InfectedNodes.ContainsKey((y, x)) ? part2.InfectedNodes[(y, x)] : InfectionGrade.Clean;

                        int imageY = y + Math.Abs(startY);
                        int imageX = x + Math.Abs(startX);

                        op.FillPolygon(colors[(int)state], new PointF[] { new PointF(imageX, imageY),
                                                                          new PointF(imageX + 4, imageY),
                                                                          new PointF(imageX + 4, imageY + 4),
                                                                          new PointF(imageX, imageY + 4)
                                                                         });
                    }
                }
            });

            image.Save("part2.png");
        }
    }
}
