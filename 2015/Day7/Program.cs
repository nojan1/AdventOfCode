using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day7
{
    class Program
    {
        class Wire
        {
            public string Name { get; set; }

            private int signal;
            public int Signal
            {
                get { return signal; }
                set
                {
                    if (signal == value)
                        return;

                    signal = value;

                    if (!string.IsNullOrEmpty(Name))
                    {
                        SignalUpdated(signal);
                    }
                }
            }

            public event Action<int> SignalUpdated = delegate { };
        }

        class WireBundle
        {
            private List<Wire> wires = new List<Wire>();

            public Wire GetOrCreate(string name)
            {
                //Names that are just numbers are a fixed value return fake wire
                int value;
                if (int.TryParse(name, out value))
                {
                    return new Wire { Signal = value };
                }

                var wire = wires.FirstOrDefault(w => w.Name == name.ToLower());
                if (wire == null)
                {
                    wire = new Wire { Name = name.ToLower() };
                    wires.Add(wire);
                }

                return wire;
            }
        }

        #region Components

        abstract class Component
        {
            public void Setup(Wire input1, Wire input2, Wire output)
            {
                if (input1 != null)
                {
                    Input1 = input1;
                    Input1.SignalUpdated += OnSignalUpdated;
                }

                Input2 = input2;
                Output = output;

                Input2.SignalUpdated += OnSignalUpdated;
            }

            public Wire Input1 { get; private set; }
            public Wire Input2 { get; private set; }
            public Wire Output { get; private set; }

            public bool HasExecuted { get; set; }

            private void OnSignalUpdated(int newValue)
            {
                Execute();
            }

            public virtual bool CanExecute()
            {
                return Input1.Signal != 0 && Input2.Signal != 0;
            }

            public void Execute()
            {
                if (CanExecute())
                {
                    if (Input1 != null)
                        Input1.SignalUpdated -= OnSignalUpdated;

                    Input2.SignalUpdated -= OnSignalUpdated;

                    ExecuteImpl();
                    HasExecuted = true;
                }
            }

            public abstract void ExecuteImpl();
        }

        class AND : Component
        {
            public override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal & Input2.Signal;
            }
        }

        class OR : Component
        {
            public override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal | Input2.Signal;
            }
        }

        class LSHIFT : Component
        {
            public override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal << Input2.Signal;
            }
        }

        class RSHIFT : Component
        {
            public override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal >> Input2.Signal;
            }
        }

        class NOT : Component
        {
            public override bool CanExecute()
            {
                return Input2.Signal != 0;
            }

            public override void ExecuteImpl()
            {
                Output.Signal = ~Input2.Signal;
            }
        }

        #endregion

        static void Main(string[] args)
        {
            var bundle = new WireBundle();
            var components = new List<Component>();

            foreach (var command in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                var parts = command.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                var outputWire = bundle.GetOrCreate(parts[1].Trim());

                if(components.Any(c => c.Output == outputWire))
                {
                    continue;
                }

                var leftSideParts = Regex.Split(parts[0], "(AND|OR|NOT|RSHIFT|LSHIFT)");
                if (leftSideParts.Length >= 2)
                {
                    int offset = 0;
                    if (leftSideParts.Length == 3)
                    {
                        offset = 1;
                    }

                    var component = ComponentFromName(leftSideParts[offset].Trim());
                    component.Setup(offset == 1 ? bundle.GetOrCreate(leftSideParts[0].Trim()) : null,
                                    bundle.GetOrCreate(leftSideParts[offset + 1].Trim()),
                                    outputWire);

                    component.Execute();

                    components.Add(component);
                }
                else if (leftSideParts.Length == 1)
                {
                    outputWire.Signal = bundle.GetOrCreate(leftSideParts[0]).Signal;
                }else
                {
                    throw new Exception("Bad line?");
                }
            }

            while (components.Any(c => !c.HasExecuted))
            {
                foreach (var component in components.OrderBy(c => Guid.NewGuid()))
                {
                    if (!component.HasExecuted && component.CanExecute())
                    {
                        component.Execute();
                        component.HasExecuted = true;

                        break;
                    }
                }
            }

            Console.WriteLine($"Value for wire a is {bundle.GetOrCreate("a").Signal}");
        }

        static Component ComponentFromName(string name)
        {
            switch (name.ToLower())
            {
                case "and":
                    return new AND();
                case "or":
                    return new OR();
                case "not":
                    return new NOT();
                case "lshift":
                    return new LSHIFT();
                case "rshift":
                    return new RSHIFT();
                default:
                    throw new Exception("No such component " + name);
            }
        }
    }
}
