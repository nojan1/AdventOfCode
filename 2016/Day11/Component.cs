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
            return other.Element == Element;
        }

        public override int GetHashCode()
        {
            return Element.GetHashCode();
        }
    }

    public class RTG : Component { }
    public class Microship : Component { }
}
