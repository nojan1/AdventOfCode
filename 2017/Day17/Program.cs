using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static int A(int input)
        {
            var currentPosition = 0;
            var buffer = new CircularList<int>();
            buffer.Add(0);

            for (int i = 1; i <= 2017; i++)
            {
                currentPosition = (currentPosition + input + 1) % buffer.Count;
                buffer.Insert(currentPosition, i);
            }

            return buffer[buffer.IndexOf(2017) + 1];
        }

        static int B(int input)
        {
            var length = 1;
            var index = 0;
            var value = -1;

            for(int i = 1; i <= 50000000;i++)
            {
                index = (index + input + 1) % length;

                if (index == 0)
                    value = i;

                length++;
            }

            return value;
        }

        static void Main(string[] args)
        {
            var input = 369;

            var valueA = A(input);
            Console.WriteLine($"The value is {valueA}");

            var valueB = B(input);
            Console.WriteLine($"And for B the value is {valueB}");
        }
    }
}
