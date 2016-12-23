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
                Components = Components.Select<Component, Component>(c => { if (c is RTG) { return new RTG { Element = c.Element }; } else { return new Microship { Element = c.Element }; } }).ToList()
            };
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
                foreach (Microship microship in floor.Components.Where(c => c is Microship))
                {
                    if (floor.Components.Any(c => c is RTG) && floor.Components.All(c => c is RTG && c.Element != microship.Element))
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

        private List<Component> ComponentsFromLine(string line)
        {
            var returnValue = new List<Component>();

            var generators = Regex.Matches(line, @"(\w*) generator").Cast<Match>().Select(m => m.Groups[1].Value);
            var microships = Regex.Matches(line, @"(\w*)-compatible microchip").Cast<Match>().Select(m => m.Groups[1].Value);

            foreach (var generator in generators)
                returnValue.Add(new RTG { Element = generator });

            foreach (var microship in microships)
                returnValue.Add(new Microship { Element = microship });

            return returnValue;
        }
    }
}
