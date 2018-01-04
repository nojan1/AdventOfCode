using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class Triangle
    {
        public int Side1 { get; private set; }
        public int Side2 { get; private set; }
        public int Hypotenuse { get; private set; }
        public bool IsPossible => Side1 + Side2 > Hypotenuse;

        public Triangle(string[] parts)
        {
            var sides = parts
                .Select(s => Convert.ToInt32(s))
                .OrderByDescending(x => x)
                .ToList();

            Hypotenuse = sides[0];
            Side1 = sides[1];
            Side2 = sides[2];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var part = DayPart.Two;

            ////

            var values = File.ReadAllLines("input.txt")
                .Select(l => l.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            List<Triangle> triangles;

            if(part == DayPart.One)
            {
                triangles = values
                    .Select(x => new Triangle(x))
                    .ToList();
            }
            else
            {
                
                triangles = new List<Triangle>();

                for (int i = 0; i < values.Length; i += 3)
                {
                    triangles.Add(new Triangle(new string[] { values[i][0], values[i + 1][0], values[i + 2][0] }));
                    triangles.Add(new Triangle(new string[] { values[i][1], values[i + 1][1], values[i + 2][1] }));
                    triangles.Add(new Triangle(new string[] { values[i][2], values[i + 1][2], values[i + 2][2] }));
                }
            }

            var numPossible = triangles.Count(t => t.IsPossible);
            Console.WriteLine($"{numPossible} triangles are possible");
        }
    }
}
