using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{
    public static class WireListExtensions
    {
        public static Wire GetOrCreate(this List<Wire> wires, string name)
        {
            int value;
            if (int.TryParse(name, out value))
            {
                return new Wire { FixedValue = value };
            }

            var wire = wires.FirstOrDefault(w => w.Name == name);
            if (wire == null)
            {
                wire = new Wire() { Name = name };
                wires.Add(wire);
            }

            return wire;
        }
    }

    public class Wire
    {
        public string Name { get; set; }

        public int FixedValue { get; set; } = -1;

        private Component inputComponent;
        public Component InputComponent
        {
            get { return inputComponent; }
            set
            {
                if (inputComponent != null)
                    throw new Exception("Wire can only have on input!");

                inputComponent = value;
            }
        }

        public int GetSignal()
        {
            if (FixedValue == -1)
            {
                FixedValue = InputComponent.GetOutput();
            }

            return FixedValue;
        }

    }
}
