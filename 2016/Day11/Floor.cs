using System;
using System.Collections.Generic;
using System.Linq;

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
}
