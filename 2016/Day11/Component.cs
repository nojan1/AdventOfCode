using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public abstract class Component
    {
        public string Element { get; set; }

        public override bool Equals(object obj)
        {
            Component other = obj as Component;
            return this.GetType() == other.GetType() && other.Element == Element;
        }

        public override int GetHashCode()
        {
            return Element.GetHashCode();
        }

        public abstract Component Clone();
    }

    public class RTG : Component
    {
        public override string ToString()
        {
            return $"Generator: {Element}";
        }

        public override Component Clone()
        {
            return new RTG { Element = Element };
        }
    }
    public class Microchip : Component
    {
        public override string ToString()
        {
            return $"Microchip: {Element}";
        }

        public override Component Clone()
        {
            return new Microchip { Element = Element };
        }
    }
}
