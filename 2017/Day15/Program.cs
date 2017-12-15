using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class GeneratorEnumerator : IEnumerator<ulong>
    {

        private ulong _startingValue;
        private ulong _currentValue;
        private ulong _factor;
        private ulong _dividerRequirment;

        public ulong Current => _currentValue;

        object IEnumerator.Current => _currentValue;

        public GeneratorEnumerator(ulong startingValue, int factor, int dividerRequirment)
        {
            _startingValue = startingValue;
            _currentValue = _startingValue;
            _factor = (ulong)factor;
            _dividerRequirment = (ulong)dividerRequirment;
        }

        public void Dispose()
        {
            //Not needed
        }

        public bool MoveNext()
        {
            do {
                _currentValue = (_currentValue * _factor) % 2147483647;
            } while (_currentValue % _dividerRequirment != 0);

            return true;
        }

        public void Reset()
        {
            _currentValue = _startingValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Generator A starts with 289
             * Generator B starts with 629
             */

            var dayPart = DayPart.Two;

            var generatorA = new GeneratorEnumerator(289, 16807, dayPart == DayPart.One ? 1 : 4);
            var generatorB = new GeneratorEnumerator(629, 48271, dayPart == DayPart.One ? 1 : 8);

            int score = 0;

            ConsoleHelper.DisableOutput();

            for (int i = 0; i < (dayPart == DayPart.One ? 40000000 : 5000000); i++)
            {
                generatorA.MoveNext(); generatorB.MoveNext();

                Console.Write($"{generatorA.Current,10} {generatorB.Current,10}");

                if (IsBitMatch(generatorA.Current, generatorB.Current)) { 
                    score++;

                    Console.WriteLine(" <=");
                }
                else
                {
                    Console.WriteLine();
                }
                
            }

            ConsoleHelper.EnableOutput();

            Console.WriteLine($"Judges gives a score of {score}");
        }

        private static bool IsBitMatch(ulong a, ulong b)
        {
            ulong modifier = 0b1111111111111111;
            return (a & modifier) == (b & modifier);
        }
    }
}
