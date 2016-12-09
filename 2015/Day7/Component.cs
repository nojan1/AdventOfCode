using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{
    public enum ComponentType
    {
        AND,
        OR,
        NOT,
        LSHIFT,
        RSHIFT,
        ASSIGN
    }

    public class Component
    {
        public ComponentType Type { get; set; }

        public Wire Input1 { get; set; }
        public Wire Input2 { get; set; }

        public int GetOutput()
        {
            switch (Type)
            {
                case ComponentType.AND:
                    return Input1.GetSignal() & Input2.GetSignal();
                case ComponentType.OR:
                    return Input1.GetSignal() | Input2.GetSignal();
                case ComponentType.NOT:
                    return ~Input2.GetSignal();
                case ComponentType.LSHIFT:
                    return Input1.GetSignal() << Input2.GetSignal();
                case ComponentType.RSHIFT:
                    return Input1.GetSignal() >> Input2.GetSignal();
                default:
                case ComponentType.ASSIGN:
                    return Input2.GetSignal();
            }
        }
    }
}
