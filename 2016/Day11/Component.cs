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

        public override bool Equals(object obj)
        {
            RTG other = obj as RTG;
            if (other == null)
                return false;

            return other.Element == Element;
        }

        public override int GetHashCode()
        {
            return $"{Element}G".GetHashCode();
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

        public override bool Equals(object obj)
        {
            Microchip other = obj as Microchip;
            if (other == null)
                return false;

            return other.Element == Element;
        }

        public override int GetHashCode()
        {
            return $"{Element}M".GetHashCode();
        }
    }
}
