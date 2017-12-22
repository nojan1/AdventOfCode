using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GridItem<T>
    {
        public (int Y, int X) Position { get; set; }
        public T Value { get; set; }
    }

    public class DynamicGrid<T> : IEnumerable<GridItem<T>>
    {
        private Dictionary<(int Y, int X), T> _matrix = new Dictionary<(int Y, int X), T>();

        private int _width = -1;
        public int Width => _width != -1 ? _width : (_width = (_matrix.Any() ? _matrix.Max(i => i.Key.X) + 1 : 0));

        private int _height = -1;
        public int Height => _height != -1 ? _height : (_height = (_matrix.Any() ? _matrix.Max(i => i.Key.Y) + 1 : 0));

        public T this[int Y, int X]
        {
            get
            {
                if (!_matrix.TryGetValue((Y, X), out T value))
                    return default(T);

                return value;
            }
            set
            {
                if (X > Width - 1)
                    _width = -1;

                if (Y > Height - 1)
                    _height = -1;

                _matrix[(Y, X)] = value;
            }
        }

        public DynamicGrid()
        {
        }

        public DynamicGrid(IEnumerable<IEnumerable<T>> values)
        {
            int y = 0;
            int x = 0;

            foreach (var row in values)
            {
                foreach (var column in row)
                {
                    _matrix[(y, x)] = column;
                    x++;
                }
                y++;
                x = 0;
            }

            _height = -1;
            _width = -1;
        }

        public DynamicGrid(IEnumerable<IEnumerable<DynamicGrid<T>>> grids)
        {
            int y = 0;
            int x = 0;

            foreach (var row in grids)
            {
                var rowGrids = row.ToList();

                var numRows = rowGrids.Max(g => g.Height);
                for (int rowNum = 0; rowNum < numRows; rowNum++)
                {
                    x = 0;

                    foreach (var grid in rowGrids)
                    {
                        if (rowNum >= grid.Height)
                            continue;

                        for (int colNum = 0; colNum < grid.Width; colNum++)
                        {
                            this[y, x++] = grid[rowNum, colNum];
                        }
                    }

                    y++;
                }
            }

            _height = -1;
            _width = -1;
        }

        public DynamicGrid<T> SubGrid(int startY, int startX, int width, int height)
        {
            var returnMatrix = new DynamicGrid<T>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    returnMatrix[y, x] = this[y + startY, x + startX];
                }
            }

            return returnMatrix;
        }

        public DynamicGrid<T> Flip(bool vertical)
        {
            var returnValue = new DynamicGrid<T>();

            if (vertical)
            {
                for (int y = 0; y < Height; y++)
                {
                    var sourceRow = Height - y - 1;
                    for (int x = 0; x < Width; x++)
                    {
                        returnValue[y, x] = this[sourceRow, x];
                    }
                }
            }
            else
            {
                for (int x = 0; x < Width; x++)
                {
                    var sourceColumn = Width - x - 1;

                    for (int y = 0; y < Height; y++)
                    {
                        returnValue[y, x] = this[y, sourceColumn];
                    }
                }
            }

            return returnValue;
        }

        public DynamicGrid<T> Rotate()
        {
            var returnValue = new DynamicGrid<T>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int newY = x;
                    int newX = Width - y - 1;

                    returnValue[newY, newX] = this[y, x];
                }
            }

            return returnValue;
        }

        public static bool operator ==(DynamicGrid<T> left, DynamicGrid<T> right)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(left, null))
            {
                if (Object.ReferenceEquals(right, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(DynamicGrid<T> left, DynamicGrid<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var other = obj as DynamicGrid<T>;
            if (other == null)
                return false;

            foreach (var val in this)
            {
                if (!val.Value.Equals(other[val.Position.Y, val.Position.X]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }

        public IEnumerator<GridItem<T>> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return new GridItem<T>
                    {
                        Position = (y, x),
                        Value = _matrix[(y, x)]
                    };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
