using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class CodeGrid
    {
        private long[,] _grid;
        private int _sideSize;
        private (int row, int column) _lastGeneratedCoord;
        private int _maxRow;
        private int _maxColumn;

        private List<(int row, int column)> _coords = new List<(int row, int column)>();

        public CodeGrid(int sideSize)
        {
            _sideSize = sideSize;
            _grid = new long[sideSize, sideSize];

            _grid[1, 1] = 20151125;
            _grid[2, 1] = 31916031;
            _grid[3, 1] = 16080970;
            _grid[4, 1] = 24592653;
            _grid[5, 1] = 77061;
            _grid[6, 1] = 33071741;

            _grid[1, 2] = 18749137;
            _grid[2, 2] = 21629792;
            _grid[3, 2] = 8057251;
            _grid[4, 2] = 32451966;
            _grid[5, 2] = 17552253;

            _grid[1, 3] = 17289845;
            _grid[2, 3] = 16929656;
            _grid[3, 3] = 1601130;
            _grid[4, 3] = 21345942;

            _grid[1, 4] = 30943339;
            _grid[2, 4] = 7726640;
            _grid[3, 4] = 7981243;

            _grid[1, 5] = 10071777;
            _grid[2, 5] = 15514188;

            _grid[1, 6] = 33511524;

            _maxRow = 6;
            _maxColumn = 6;

            _lastGeneratedCoord = (1, 6);

            Fill();
            
        }

        private void Fill()
        {
            while(_lastGeneratedCoord.column < _sideSize - 1)
            {
                var nextCoord = GetNextCoord(_lastGeneratedCoord.row, _lastGeneratedCoord.column);
                var lastValue = GetValue(_lastGeneratedCoord.row, _lastGeneratedCoord.column);

                var newValue = (lastValue * 252533) % 33554393;

                _grid[nextCoord.row, nextCoord.column] = newValue;
                _lastGeneratedCoord = nextCoord;

                if (nextCoord.row > _maxRow)
                    _maxRow = nextCoord.row;

                if (nextCoord.column > _maxColumn)
                    _maxColumn = nextCoord.column;

                _coords.Add(_lastGeneratedCoord);
            }
        }

        public long GetValue(int row, int column)
        {
            return _grid[row, column];
        }

        private (int row, int column) GetNextCoord(int row, int column)
        {
            int newRow = --row;
            int newColumn = ++column;

            if(newRow < 1)
            {
                newColumn = 1;
                newRow = _maxRow + 1;
            }

            return (newRow, newColumn);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = "To continue, please consult the code grid in the manual.  Enter the code at row 2947, column 3029.";
            var targetRow = 2947;
            var targetColumn = 3020;

            var grid = new CodeGrid(6000);
            var code = grid.GetValue(targetRow, targetColumn);

            if (code >= 28030537)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"The code is: {code}");
        }
    }
}
