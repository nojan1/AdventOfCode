using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class Code
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public ulong Value { get; set; }
    }

    class CodeEnumerator : IEnumerator<Code>
    {
        private Code _currentCode;

        public CodeEnumerator()
        {
            Reset();
        }

        public Code Current => _currentCode;

        object IEnumerator.Current => _currentCode;

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentCode == null)
            {
                _currentCode = new Code
                {
                    Row = 1,
                    Column = 1,
                    Value = 20151125
                };
            }
            else
            {
                int newColumn = ++_currentCode.Column;
                int newRow = --_currentCode.Row;

                ulong newValue = (_currentCode.Value * 252533) % 33554393;

                if (newRow < 1)
                {
                    newRow = newColumn;
                    newColumn = 1;
                }

                _currentCode = new Code
                {
                    Row = newRow,
                    Column = newColumn,
                    Value = newValue
                };
            }

            return true;
        }

        public void Reset()
        {
            _currentCode = null;
        }
    }

    class CodeEnumberable : IEnumerable<Code>
    {
        public IEnumerator<Code> GetEnumerator()
        {
            return new CodeEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CodeEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //To continue, please consult the code grid in the manual.  Enter the code at row 2947, column 3029.
            var targetRow = 2947;
            var targetColumn = 3029;

            var codeEnumerable = new CodeEnumberable();
            var code = codeEnumerable.First(x => x.Row == targetRow && x.Column == targetColumn).Value;

            Console.WriteLine($"The code is {code}");
        }
    }
}
