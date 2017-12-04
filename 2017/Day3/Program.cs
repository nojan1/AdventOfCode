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
        protected virtual string SAVEFILE_FILENAME => "grid.json";

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
            if (contents.High >= requiredHigh)
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

            int i = 2;
            int value = 0;
            while(value <= highest)
            {
                var nextCord = GetNextCordFromDirection(lastCell.X, lastCell.Y, currentDirection);
                value = CalculateValue(i, nextCord.x, nextCord.y);

                var newCell = new MemoryCell
                {
                    Value = value,
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

                i++;
            }
        }

        protected virtual int CalculateValue(int i, int x, int y)
        {
            return i;
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

    class MemoryGrid2 : MemoryGrid
    {
        protected override string SAVEFILE_FILENAME => "grid2.json";

        public MemoryGrid2(int highest) : base(highest)
        {
        }

        protected override int CalculateValue(int i, int x, int y)
        {
            var topLeft = Cells.FirstOrDefault(c => c.X == x - 1 && c.Y == y - 1)?.Value ?? 0;
            var top = Cells.FirstOrDefault(c => c.X == x && c.Y == y - 1)?.Value ?? 0;
            var topRight = Cells.FirstOrDefault(c => c.X == x + 1 && c.Y == y - 1)?.Value ?? 0;
            var right = Cells.FirstOrDefault(c => c.X == x + 1 && c.Y == y)?.Value ?? 0;
            var bottomRight = Cells.FirstOrDefault(c => c.X == x + 1 && c.Y == y + 1)?.Value ?? 0;
            var bottom = Cells.FirstOrDefault(c => c.X == x && c.Y == y + 1)?.Value ?? 0;
            var bottomLeft = Cells.FirstOrDefault(c => c.X == x - 1 && c.Y == y + 1)?.Value ?? 0;
            var left = Cells.FirstOrDefault(c => c.X == x - 1 && c.Y == y)?.Value ?? 0;

            return topLeft + top + topRight + right + bottomRight + bottom + bottomLeft + left;
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

            var numSteps = CalculateNumSteps(valueCell, targetCell);

            Console.WriteLine($"Öhm {numSteps} maybe?");

            var grid2 = new MemoryGrid2(input);
            var number = grid2.Cells.Max(c => c.Value);

            Console.WriteLine($"The number is {number}");
        }

        static int CalculateNumSteps(MemoryCell valueCell, MemoryCell targetCell)
        {
            int numSteps = 0;
            var currentPosition = (X: valueCell.X, Y: valueCell.Y);

            while (currentPosition.X != targetCell.X || currentPosition.Y != targetCell.Y)
            {
                if (currentPosition.X > targetCell.X && currentPosition.Y > targetCell.Y)
                {
                    currentPosition = (currentPosition.X - 1, currentPosition.Y - 1);
                    numSteps += 2;
                }
                else if (currentPosition.X > targetCell.X && currentPosition.Y < targetCell.Y)
                {
                    currentPosition = (currentPosition.X - 1, currentPosition.Y + 1);
                    numSteps += 2;
                }
                else if (currentPosition.X < targetCell.X && currentPosition.Y > targetCell.Y)
                {
                    currentPosition = (currentPosition.X + 1, currentPosition.Y - 1);
                    numSteps += 2;
                }
                else if (currentPosition.X < targetCell.X && currentPosition.Y < targetCell.Y)
                {
                    currentPosition = (currentPosition.X + 1, currentPosition.Y + 1);
                    numSteps += 2;
                }
                else if (currentPosition.X > targetCell.X)
                {
                    currentPosition = (currentPosition.X - 1, currentPosition.Y);
                    numSteps++;
                }
                else if (currentPosition.X < targetCell.X)
                {
                    currentPosition = (currentPosition.X + 1, currentPosition.Y);
                    numSteps++;
                }
                else if (currentPosition.Y > targetCell.Y)
                {
                    currentPosition = (currentPosition.X, currentPosition.Y - 1);
                    numSteps++;
                }
                else if (currentPosition.Y < targetCell.Y)
                {
                    currentPosition = (currentPosition.X, currentPosition.Y + 1);
                    numSteps++;
                }
            }

            return numSteps;
        }
    }
}