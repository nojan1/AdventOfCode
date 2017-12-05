using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day11
{
    public class Floor
    {
        public int FloorNum { get; set; }
        public List<Component> Components { get; set; }

        public Floor Clone()
        {
            return new Floor
            {
                FloorNum = FloorNum,
                Components = Components.Select<Component, Component>(c => { if (c is RTG) { return new RTG { Element = c.Element }; } else { return new Microchip { Element = c.Element }; } }).ToList()
            };
        }

        public override string ToString()
        {
            return String.Join(" ", Components.Select(c =>
            {
                if (c is RTG)
                    return $"{c.Element.ToUpper().First()}G";
                else
                    return $"{c.Element.ToUpper().First()}M";
            }));
        }
    }

    public class FloorCollection
    {
        public Floor[] Floors { get; private set; }

        public FloorCollection(string filename)
        {
            var lines = File.ReadAllLines(filename).Select(t => t.Trim()).ToArray();
            Floors = new Floor[4]
            {
                new Floor { FloorNum = 1, Components = ComponentsFromLine(lines[0]) },
                new Floor { FloorNum = 2, Components = ComponentsFromLine(lines[1]) },
                new Floor { FloorNum = 3, Components = ComponentsFromLine(lines[2]) },
                new Floor { FloorNum = 4, Components = new List<Component>() }
            };
        }

        public FloorCollection(Floor[] floors)
        {
            Floors = floors;
        }

        public bool AllComponentsOnTopFloor()
        {
            return Floors.Take(3).All(f => f.Components.Count == 0) && Floors[3].Components.Count > 0;
        }

        public bool WillResultInCatastrophicMeltdown()
        {
            foreach (var floor in Floors)
            {
                if (floor.Components.All(c => c is Microchip))
                    continue;

                foreach (var microship in floor.Components.OfType<Microchip>())
                {
                    if (!floor.Components.Any(c => c is RTG && c.Element == microship.Element))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public FloorCollection Clone()
        {
            return new FloorCollection(Floors.Select(f => f.Clone()).ToArray());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = Floors.Length - 1; i >= 0; i--)
            {
                sb.AppendLine($"F{(i + 1)}: {Floors[i]}");
            }

            return sb.ToString();
        }

        private List<Component> ComponentsFromLine(string line)
        {
            var returnValue = new List<Component>();

            var generators = Regex.Matches(line, @"(\w*) generator").Cast<Match>().Select(m => m.Groups[1].Value);
            var microships = Regex.Matches(line, @"(\w*)-compatible microchip").Cast<Match>().Select(m => m.Groups[1].Value);

            foreach (var generator in generators)
                returnValue.Add(new RTG { Element = generator });

            foreach (var microship in microships)
                returnValue.Add(new Microchip { Element = microship });

            return returnValue;
        }
    }
}
