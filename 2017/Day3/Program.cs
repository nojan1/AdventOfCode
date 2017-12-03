using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class MemoryCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"{X};{Y} - {Value}";
        }
    }

    class MemoryGrid
    {
        const string SAVEFILE_FILENAME = "grid.json";

        enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        class SaveFileFormat
        {
            public int High { get; set; }
            public List<MemoryCell> Cells { get; set; }
        }

        public List<MemoryCell> Cells { get; private set; } = new List<MemoryCell>();

        public MemoryGrid(int highest)
        {
            if (!LoadFile(highest))
            {
                Allocate(highest);
                SaveFile();
            }
        }

        private void SaveFile()
        {
            File.WriteAllText(SAVEFILE_FILENAME,
                              JsonConvert.SerializeObject(new SaveFileFormat
                              {
                                  Cells = Cells,
                                  High = Cells.Max(c => c.Value)
                              }));
        }

        private bool LoadFile(int requiredHigh)
        {
            if (!File.Exists(SAVEFILE_FILENAME))
                return false;

            var contents = JsonConvert.DeserializeObject<SaveFileFormat>(File.ReadAllText(SAVEFILE_FILENAME));
            if(contents.High >= requiredHigh)
            {
                Cells = contents.Cells;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Export()
        {
            var sb = new StringBuilder();
            int minRow = Cells.Min(x => x.Y);
            int maxRow = Cells.Max(x => x.Y);
            int maxValueLength = Cells.Max(x => x.Value).ToString().Length;

            for (int y = minRow; y <= maxRow; y++)
            {
                sb.AppendLine(string.Join("\t", Cells.Where(c => c.Y == y).OrderBy(c => c.X).Select(c => c.Value.ToString().PadLeft(maxValueLength))));
            }

            return sb.ToString();
        }

        private void Allocate(int highest)
        {
            var currentDirection = Direction.Right;
            var lastCell = new MemoryCell
            {
                Value = 1,
                X = 0,
                Y = 0
            };

            Cells.Add(lastCell);

            for (int i = 2; i <= highest; i++)
            {
                var nextCord = GetNextCordFromDirection(lastCell.X, lastCell.Y, currentDirection);
                var newCell = new MemoryCell
                {
                    Value = i,
                    X = nextCord.x,
                    Y = nextCord.y
                };
                Cells.Add(newCell);

                lastCell = newCell;

                var leftTurnDirection = TurnLeft(currentDirection);
                var leftTurnCords = GetNextCordFromDirection(lastCell.X, lastCell.Y, leftTurnDirection);
                if (!Cells.Any(c => c.X == leftTurnCords.x && c.Y == leftTurnCords.y))
                {
                    currentDirection = leftTurnDirection;
                }
            }
        }

        private Direction TurnLeft(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Right;
                default:
                    return Direction.Down;
            }
        }

        private (int x, int y) GetNextCordFromDirection(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (x, y - 1);
                case Direction.Right:
                    return (x + 1, y);
                case Direction.Down:
                    return (x, y + 1);
                default:
                    return (x - 1, y);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = 265149;

            var grid = new MemoryGrid(input);

            var valueCell = grid.Cells.First(x => x.Value == input);
            var targetCell = grid.Cells.First(x => x.Value == 1);

            var idealPath = GetIdealPath(grid, valueCell, targetCell).ToList();
        }

        static IEnumerable<(int x, int y)> GetIdealPath(MemoryGrid grid, MemoryCell valueCell, MemoryCell targetCell)
        {
            if (valueCell.X == targetCell.X)
                throw new NotImplementedException();

            if (valueCell.Y == targetCell.Y)
                throw new NotImplementedException();

            var k = ((double)valueCell.X - (double)targetCell.X) / ((double)valueCell.Y - (double)targetCell.Y);
            var xChange = targetCell.X < valueCell.X ? -1 : 1;

            for(var x = valueCell.X; x != targetCell.X; x+= xChange)
            {
                var y = k * x + valueCell.Y;

                yield return (x, (int)y);
            }
        }
    }
}