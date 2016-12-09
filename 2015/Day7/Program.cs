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
        class TheForce
        {
            private Dictionary<string, List<Component>> dependencies = new Dictionary<string, List<Component>>();
            private List<Component> withNoDependencies = new List<Component>();

            public void Register(Component component, List<Wire> dependentWires)
            {
                if (dependentWires.All(w => w.IsFake))
                {
                    withNoDependencies.Add(component);
                }
                else
                {
                    foreach(var wire in dependentWires)
                    {
                        if (wire.IsFake)
                            continue;

                        if(!dependencies.ContainsKey(wire.Name) || dependencies[wire.Name] == null)
                        {
                            dependencies[wire.Name] = new List<Component> { component };
                            wire.SignalUpdated += Wire_SignalUpdated;
                        }
                        else
                        {
                            dependencies[wire.Name].Add(component);
                        }
                    }
                }
            }

            private void Wire_SignalUpdated(object sender, EventArgs e)
            {
                var wireName = (sender as Wire).Name;
                if (dependencies.ContainsKey(wireName))
                {
                    foreach (var component in dependencies[wireName])
                    {
                        component.Execute();
                    }
                }
            }

            public void Run()
            {
                foreach (var component in withNoDependencies)
                    component.Execute();
            }

        }

        class Wire
        {
            public string Name { get; set; }

            public bool GottenSignal { get; private set; }
            public bool IsFake { get; set; }

            private int firstValueBackup = -1;
            private int signal = 0;
            public int Signal
            {
                get { return signal; }
                set
                {
                    GottenSignal = true;

                    signal = value;

                    if (!string.IsNullOrEmpty(Name))
                    {
                        SignalUpdated(this, new EventArgs());
                    }

                    if (firstValueBackup == -1)
                        firstValueBackup = value;
                }
            }

            public event EventHandler SignalUpdated = delegate { };

            public void Reset(int signalValue = 0)
            {
                GottenSignal = false;
                signal = signalValue;

                if (IsFake && firstValueBackup != -1)
                    signal = firstValueBackup;
            }
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
                    var fakeWire = new Wire { Signal = value, IsFake = true };
                    wires.Add(fakeWire);

                    return fakeWire;
                }

                var wire = wires.FirstOrDefault(w => w.Name == name.ToLower());
                if (wire == null)
                {
                    wire = new Wire { Name = name.ToLower() };
                    wires.Add(wire);
                }

                return wire;
            }

            public void Reset()
            {
                foreach(var wire in wires)
                {
                    wire.Reset();
                }
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, wires.Select(w => $"{w.Name}: {w.Signal}"));
            }
        }

        

        abstract class Component
        {
            public void Setup(Wire input1, Wire input2, Wire output, TheForce force)
            {
                Input2 = input2;
                Output = output;

                if (input1 != null)
                {
                    Input1 = input1;

                    force.Register(this, new List<Wire> { Input1, Input2 });
                }
                else
                {
                    force.Register(this, new List<Wire> { Input2 });
                }
            }

            public Wire Input1 { get; private set; }
            public Wire Input2 { get; private set; }
            public Wire Output { get; private set; }

            protected int lastInput1Value = -2;
            protected int lastInput2Value = -2;

            public bool HasExecuted { get; private set; }

            public virtual bool CanExecute()
            {
                //return (HasExecuted || (Input1 != null && Input1.Signal != lastInput1Value) || Input2.Signal != lastInput2Value)
                //    && Input1.GottenSignal && Input2.GottenSignal;

                return !HasExecuted && Input1.GottenSignal && Input2.GottenSignal;
            }

            public void Execute()
            {
                if (CanExecute())
                {
                    ExecuteImpl();
                    HasExecuted = true;
                }
            }

            public void Reset()
            {
                lastInput1Value = lastInput2Value = -2;
                HasExecuted = false;
            }

            protected abstract void ExecuteImpl();
        }

        #region Components

        class AND : Component
        {
            protected override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal & Input2.Signal;
            }
        }

        class OR : Component
        {
            protected override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal | Input2.Signal;
            }
        }

        class LSHIFT : Component
        {
            protected override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal << Input2.Signal;
            }
        }

        class RSHIFT : Component
        {
            protected override void ExecuteImpl()
            {
                Output.Signal = Input1.Signal >> Input2.Signal;
            }
        }

        class NOT : Component
        {
            public override bool CanExecute()
            {
                return (!HasExecuted || Input2.Signal != lastInput2Value) && Input2.GottenSignal;
            }

            protected override void ExecuteImpl()
            {
                Output.Signal = ~Input2.Signal;
            }
        }

        class ASSIGN : Component
        {
            protected override void ExecuteImpl()
            {
                Output.Signal = Input2.Signal;
            }

            public override bool CanExecute()
            {
                return (!HasExecuted || Input2.Signal != lastInput2Value) && Input2.GottenSignal;
            }
        }

        #endregion

        static void Main(string[] args)
        {
            var force = new TheForce();
            var bundle = new WireBundle();
            var components = new List<Component>();

            foreach (var command in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                var parts = command.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                var outputWire = bundle.GetOrCreate(parts[1].Trim());

                if (components.Any(c => c.Output == outputWire))
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
                                    outputWire,
                                    force);

                    components.Add(component);
                }
                else if (leftSideParts.Length == 1)
                {
                    var component = new ASSIGN();
                    component.Setup(null,
                                    bundle.GetOrCreate(leftSideParts[0].Trim()),
                                    outputWire, 
                                    force);

                    components.Add(component);
                }
                else
                {
                    throw new Exception("Bad line?");
                }

            }

            force.Run();

            var wireASignal = bundle.GetOrCreate("a").Signal;

            bundle.Reset();
            foreach (var component in components)
                component.Reset();

            //var overridingComponent = new ASSIGN();
            //overridingComponent.Setup(null,
            //                          bundle.GetOrCreate(wireASignal.ToString()),
            //                          bundle.GetOrCreate("b"),
            //                          force);

            bundle.GetOrCreate("b").Reset(wireASignal);

            force.Run();

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
