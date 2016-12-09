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
        static void Main(string[] args)
        {
            var wires = new List<Wire>();

            foreach (var command in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                var parts = command.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                var outputWire = wires.GetOrCreate(parts[1].Trim());

                var leftSideParts = Regex.Split(parts[0], "(AND|OR|NOT|RSHIFT|LSHIFT)");
                if (leftSideParts.Length >= 2)
                {
                    int offset = 0;
                    if (leftSideParts.Length == 3)
                    {
                        offset = 1;
                    }

                    var component = new Component
                    {
                        Type = (ComponentType)Enum.Parse(typeof(ComponentType), leftSideParts[offset].Trim(), true),
                        Input1 = offset == 1 ? wires.GetOrCreate(leftSideParts[0].Trim()) : null,
                        Input2 = wires.GetOrCreate(leftSideParts[offset + 1].Trim())
                    };

                    outputWire.InputComponent = component;
                }
                else if (leftSideParts.Length == 1)
                {
                    var component = new Component
                    {
                        Type = ComponentType.ASSIGN,
                        Input1 = null,
                        Input2 = wires.GetOrCreate(leftSideParts[0].Trim())
                    };

                    outputWire.InputComponent = component;
                }
                else
                {
                    throw new Exception("Bad line?");
                }

            }

            var wireA = wires.GetOrCreate("a");
            var value = wireA.GetSignal();

            wires.ForEach(w =>
            {
                w.FixedValue = -1;
            });

            wires.GetOrCreate("b").FixedValue = value;

            value = wireA.GetSignal();

            Console.WriteLine($"Value for wire a is {wireA.GetSignal()}");
        }
    }
}
