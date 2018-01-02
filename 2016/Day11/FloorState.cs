using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day11
{

    public class FloorState
    {
        private string _serialized = string.Empty;
        public string Serialized => string.IsNullOrEmpty(_serialized) ? (_serialized = Serialize()) : _serialized;

        public int ElevatorPosition { get; set; } = 0;
        public Floor[] Floors { get; private set; }

        public FloorState(string filename)
        {
            var lines = File.ReadAllLines(filename).Select(t => t.Trim()).ToArray();
            Floors = new Floor[4]
            {
                new Floor { FloorNum = 1, Components = ComponentsFromLine(lines[0]) },
                new Floor { FloorNum = 2, Components = ComponentsFromLine(lines[1]) },
                new Floor { FloorNum = 3, Components = ComponentsFromLine(lines[2]) },
                new Floor { FloorNum = 4, Components = new List<Component>() }
            };
        }

        public FloorState(Floor[] floors)
        {
            Floors = floors;
        }

        public bool AllComponentsOnTopFloor()
        {
            return Floors.Take(3).All(f => f.Components.Count == 0) && Floors[3].Components.Count > 0;
        }

        public bool WillResultInCatastrophicMeltdown()
        {
            foreach (var floor in Floors)
            {
                if (floor.Components.All(c => c is Microchip))
                    continue;

                foreach (var microship in floor.Components.OfType<Microchip>())
                {
                    if (!floor.Components.Any(c => c is RTG && c.Element == microship.Element))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public FloorState Clone()
        {
            return new FloorState(Floors.Select(f => f.Clone()).ToArray()) { ElevatorPosition = this.ElevatorPosition };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = Floors.Length - 1; i >= 0; i--)
            {
                sb.AppendLine($"F{(i + 1)}: {Floors[i]}");
            }

            return sb.ToString();
        }

        public void ClearSerializedState()
        {
            _serialized = string.Empty;
        }

        private List<Component> ComponentsFromLine(string line)
        {
            var returnValue = new List<Component>();

            var generators = Regex.Matches(line, @"(\w*) generator").Cast<Match>().Select(m => m.Groups[1].Value);
            var microships = Regex.Matches(line, @"(\w*)-compatible microchip").Cast<Match>().Select(m => m.Groups[1].Value);

            foreach (var generator in generators)
                returnValue.Add(new RTG { Element = generator });

            foreach (var microship in microships)
                returnValue.Add(new Microchip { Element = microship });

            return returnValue;
        }

        private string Serialize()
        {
            var positions = new List<string>();
            var elements = Floors.SelectMany(f => f.Components).Select(c => c.Element).Distinct().ToList();

            foreach(var element in elements)
            {
                int generatorPosition = Floors.First(f => f.Components.Any(c => c is RTG && c.Element == element)).FloorNum;
                int chipPosition = Floors.First(f => f.Components.Any(c => c is Microchip && c.Element == element)).FloorNum;

                positions.Add($"{generatorPosition}{chipPosition}");
            }

            return $"{ElevatorPosition}|{string.Join("", positions.OrderBy(x => x))}";
        }
    }
}
