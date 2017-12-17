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
        static void Main(string[] args)
        {
            var input = 3/*369*/;

            var currentPosition = 0;
            var buffer = new CircularList<int>();
            buffer.Add(0);

            for(int i = 1; i <= 2017; i++)
            {
                currentPosition += input;
                buffer.Insert(currentPosition, i);
            }

            var value = buffer[buffer.IndexOf(2017) + 1];
            Console.WriteLine($"The value is {value}");
        }
    }
}
