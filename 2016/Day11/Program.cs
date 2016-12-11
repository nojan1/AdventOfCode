using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day11
{
    abstract class Component
    {
        public string Element { get; set; }
    }

    class RTG : Component { }
    class Microship : Component { }

    class Floor
    {
        public int FloorNum { get; set; }
        public List<Component> Components { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Select(t => t.Trim()).ToArray();
            var floors = new Floor[4]
            {
                new Floor { FloorNum = 1, Components = ComponentsFromLine(lines[0]) },
                new Floor { FloorNum = 2, Components = ComponentsFromLine(lines[1]) },
                new Floor { FloorNum = 3, Components = ComponentsFromLine(lines[2]) },
                new Floor { FloorNum = 4, Components = new List<Component>() }
            };


        }

        private static List<Component> ComponentsFromLine(string line)
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
