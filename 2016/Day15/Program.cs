using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        class Disc
        {
            private int _currentPosition;
            private int _startPosition;

            public int NumPositions { get; private set; }
            public int CurrentPosition
            {
                get
                {
                    return _currentPosition;
                }
            }

            public Disc(int startPositon, int numPositions)
            {
                _startPosition = _currentPosition = startPositon;
                NumPositions = numPositions;
            }

            public void Reset(int numSteps)
            {
                _currentPosition = (_startPosition + numSteps) % NumPositions;
            }

            public void Advance()
            {
                _currentPosition++;

                if (_currentPosition > NumPositions - 1)
                {
                    _currentPosition = 0;
                }
            }
        }

        static void Main(string[] args)
        {
            /*
                Disc #1 has 13 positions; at time=0, it is at position 10.
                Disc #2 has 17 positions; at time=0, it is at position 15.
                Disc #3 has 19 positions; at time=0, it is at position 17.
                Disc #4 has 7 positions; at time=0, it is at position 1.
                Disc #5 has 5 positions; at time=0, it is at position 0.
                Disc #6 has 3 positions; at time=0, it is at position 1.
             */

            var discs = new List<Disc>
            {
                new Disc(10, 13),
                new Disc(15, 17),
                new Disc(17, 19),
                new Disc(1, 7),
                new Disc(0, 5),
                new Disc(1, 3)
            };

            discs.Add(new Disc(0, 11));

            //var discs = new List<Disc>
            //{
            //    new Disc(4, 5),
            //    new Disc(1, 2)
            //};

            var startTime = DateTime.Now;

            for (int buttonTime = 0; true; buttonTime++)
            {
                if (Simulate(discs, buttonTime))
                {
                    Console.WriteLine($"You gotta push it at timecode {buttonTime}");
                    break;
                }
            }

            var timeTaken = DateTime.Now - startTime;
            Console.WriteLine($"I ran in {timeTaken.TotalMilliseconds} ms");
        }

        private static bool Simulate(List<Disc> discs, int buttonTime)
        {
            int capsulePosition = -1;
            discs.ForEach(d => d.Reset(buttonTime));

            for (int timecode = buttonTime; true; timecode++)
            {
                if(timecode > buttonTime)
                    discs.ForEach(d => d.Advance());

                if (timecode >= buttonTime)
                {
                    capsulePosition++;
                    if (capsulePosition > 0)
                    {
                        if (capsulePosition > discs.Count)
                        {
                            return true;
                        }

                        if (discs[capsulePosition - 1].CurrentPosition != 0)
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
