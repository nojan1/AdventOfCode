using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day20
{
    class Particle
    {
        public int Number { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }

        public float Distance { get; private set; }

        public void Update()
        {
            Velocity = Velocity + Acceleration;
            Position = Position + Velocity;

            var abs = Vector3.Abs(Position);
            Distance = abs.X + abs.Y + abs.Z;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var part = DayPart.Two;

            int particleNum = 0;
            var particles = File.ReadAllLines("input.txt")
                .Select(l =>
                {
                    var vectors = Regex.Matches(l, @"\<(.*?)\>");
                    return new Particle
                    {
                        Number = particleNum++,
                        Position = BuildVectorFromString(vectors[0].Groups[1].Value),
                        Velocity = BuildVectorFromString(vectors[1].Groups[1].Value),
                        Acceleration = BuildVectorFromString(vectors[2].Groups[1].Value)
                    };
                })
                .ToList();

            Particle closestParticle = null;

            for (int i = 0; i < (part == DayPart.One ? 400 : 40); i++)
            {
                particles.ForEach(p => p.Update());

                if (part == DayPart.Two)
                {
                    foreach(var particle in particles.ToList())
                    {
                        var colliding = particles.Where(p => p.Position == particle.Position).ToList();
                        if (colliding.Count > 1)
                        {
                            colliding.ForEach(c => particles.Remove(c));
                        }
                    }
                }

                var closestDistance = particles.Min(p => p.Distance);
                var newClosestParticle = particles.First(p => p.Distance == closestDistance);

                if (closestParticle == null || newClosestParticle.Distance < closestParticle.Distance)
                {
                    closestParticle = newClosestParticle;

                    Console.Clear();
                    Console.WriteLine($"Particle {closestParticle.Number} with a distance of {closestParticle.Distance}");
                }
            }

            Console.WriteLine($"There are {particles.Count} particles left");
        }

        private static Vector3 BuildVectorFromString(string valueString)
        {
            var values = valueString.Trim().Split(',');
            return new Vector3(float.Parse(values[0].Trim()), float.Parse(values[1].Trim()), float.Parse(values[2].Trim()));
        }
    }
}
