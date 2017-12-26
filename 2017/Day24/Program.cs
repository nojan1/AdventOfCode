using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Component
    {
        public int Side1 { get; set; }
        public int Side2 { get; set; }

        public Component(string line)
        {
            var parts = line.Split('/');

            Side1 = Convert.ToInt32(parts[0]);
            Side2 = Convert.ToInt32(parts[1]);
        }

        public bool HasSide(int value)
        {
            return Side1 == value || Side2 == value;
        }
    }

    class Bridge : List<Component>
    {
        public Bridge(IEnumerable<Component> collection) : base(collection)
        {
        }

        public Bridge()
        {
        }

        public int GetStrength()
        {
            return this.Aggregate(0, (left, right) => left + right.Side1 + right.Side2);
        }

        public Bridge Clone()
        {
            return new Bridge(this.ToList());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var components = File.ReadAllLines("input.txt").Select(l => new Component(l)).ToArray();
            var bridges = new List<Bridge>();
            var workingBridges = new List<Bridge>();

            components.Where(x => x.HasSide(0)).ToList().ForEach(c =>
            {
                var bridge = new Bridge();
                bridge.Add(c);

                workingBridges.Add(bridge);
            });

            while (workingBridges.Any())
            {
                var localBridges = workingBridges.ToList();
                foreach (var bridge in localBridges)
                {
                    var possibleComponents = components.Where(c => (c.HasSide(bridge.Last().Side1) || c.HasSide(bridge.Last().Side2)) && !bridge.Contains(c)).ToList();
                    foreach (var component in possibleComponents)
                    {
                        var bridgeCopy = bridge.Clone();
                        bridgeCopy.Add(component);

                        var strength = bridgeCopy.GetStrength();
                        if (strength >= localBridges.Max(b => b.GetStrength()))
                        {
                            workingBridges.Add(bridgeCopy);
                        }
                    }

                    bridges.Add(bridge);
                    workingBridges.Remove(bridge);
                }

                Console.Clear();
                Console.WriteLine($"Working bridges: {workingBridges.Count}");
                Console.WriteLine($"Bridges: {bridges.Count}");
                Console.WriteLine($"Max strength: {bridges.Max(b => b.GetStrength())}");

            }

            var maxStrength = bridges.Max(b => b.GetStrength());
            Console.WriteLine($"The maxiumum strength is {maxStrength}");
        }
    }
}
