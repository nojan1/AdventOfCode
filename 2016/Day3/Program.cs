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
        public Triangle(string[] sides)
        {
            if (sides.Length != 3)
                throw new Exception("Bad input data, doesn't have 3 sides");

            int side1 = Convert.ToInt32(sides[0]);
            int side2 = Convert.ToInt32(sides[1]);
            int side3 = Convert.ToInt32(sides[2]);

            if (side3 > side2 && side3 > side1)
            {
                Hypotenuse = side3;
                Side1 = side1;
                Side2 = side2;
            }
            else if (side2 > side3 && side2 > side1)
            {
                Hypotenuse = side2;
                Side1 = side1;
                Side2 = side3;
            }
            else
            {
                Hypotenuse = side1;
                Side1 = side2;
                Side2 = side3;
            }
        }

        public int Side1 { get; private set; }
        public int Side2 { get; private set; }
        public int Hypotenuse { get; private set; }

        public bool IsPossible()
        {
            return Side1 + Side2 > Hypotenuse;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var values = File.ReadAllLines("input.txt").Select(l => l.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            var triangles = new List<Triangle>();

            for(int i = 0; i < values.Length; i += 3)
            {
                triangles.Add(new Triangle(new string[] { values[i][0], values[i + 1][0], values[i + 2][0] }));
                triangles.Add(new Triangle(new string[] { values[i][1], values[i + 1][1], values[i + 2][1] }));
                triangles.Add(new Triangle(new string[] { values[i][2], values[i + 1][2], values[i + 2][2] }));
            }

            var numPossible = triangles.Count(t => t.IsPossible());
            Console.WriteLine($"{numPossible} triangles are possible");
        }
    }
}
