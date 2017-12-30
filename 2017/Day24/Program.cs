using System;
using System.Collections.Concurrent;
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
        public string Identity => $"{Side1}/{Side2}";
        public string ReverseIdentity => $"{Side2}/{Side1}";

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

        public override string ToString()
        {
            return Identity;
        }
    }

    class Bridge : List<Component>
    {
        public int OpenPort => this.Any() ? Convert.ToInt32(ComponentsString.Split('/').Last()) : 0;

        public string ComponentsString { get; set; }

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
            return new Bridge(this.ToList()) { ComponentsString = this.ComponentsString };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var components = File.ReadAllLines("input.txt").Select(l => new Component(l)).ToArray();
            var bridges = new ConcurrentBag<Bridge>();
            var workingBridges = new SynchronizedCollection<Bridge>();

            components.Where(x => x.HasSide(0)).ToList().ForEach(c =>
            {
                var bridge = new Bridge();
                bridge.Add(c);

                bridge.ComponentsString += c.Side1 == 0 ? c.Identity : c.ReverseIdentity;
                workingBridges.Add(bridge);
            });

            while (workingBridges.Any())
            {
                var localBridges = workingBridges.ToList();

                Parallel.ForEach(localBridges, bridge =>
                {
                    var possibleComponents = components.Where(c => c.HasSide(bridge.OpenPort) && !bridge.Contains(c)).ToList();
                    foreach (var component in possibleComponents)
                    {
                        var bridgeCopy = bridge.Clone();

                        bridgeCopy.Add(component);
                        bridgeCopy.ComponentsString += component.Side1 == bridge.OpenPort ? component.Identity : component.ReverseIdentity;

                        workingBridges.Add(bridgeCopy);
                    }

                    bridges.Add(bridge);
                    workingBridges.Remove(bridge);
                });

                Console.Clear();
                Console.WriteLine($"Working bridges: {workingBridges.Count}");
                Console.WriteLine($"Bridges: {bridges.Count}");
                Console.WriteLine($"Max strength: {bridges.Max(b => b.GetStrength())}");
            }

            var maxStrength = bridges.Max(b => b.GetStrength());
            Console.WriteLine($"The maxiumum strength is {maxStrength}");

            var bestBridgePart2 = bridges.OrderByDescending(b => b.Count).ThenByDescending(b => b.GetStrength()).First();
            Console.WriteLine($"The strength for the longest bridge is {bestBridgePart2.GetStrength()}");
        }
    }
}
