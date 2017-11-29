using Day12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputBuffer = new List<int>();
            var cpu = new BunnyPU(File.ReadAllLines("assembunny.txt").Select(t => t.Trim()).ToArray());

            cpu.OnOutput += x =>
            {
                outputBuffer.Insert(0, x);
            };

            //int a = 7897, pointer = 0;
            //while (true)
            //{
            //    outputBuffer.Clear();
            //    pointer = 0;
            //    cpu.GetRegister("a").Value = a;

            //    while (outputBuffer.Count < 20)
            //    {
            //        pointer = cpu.RunSingleInstruction(pointer);
            //        if (pointer == -1)
            //            break;
            //    }

            //    if (BufferContainsRepeatingPattern(outputBuffer))
            //        break;

            //    a++;
            //}

            bool valueFound = true;
            Parallel.For(7897, int.MaxValue, a =>
            {
                int pointer = 0;

            });

            Console.WriteLine($"The magic value is {a}");
        }

        private static bool BufferContainsRepeatingPattern(List<int> outputBuffer)
        {
            for (int i = 0; i < outputBuffer.Count; i++)
            {
                if (i + 4 > outputBuffer.Count - 1)
                {
                    return false;
                }

                if (outputBuffer[i + 0] != 0 ||
                   outputBuffer[i + 1] != 1 ||
                   outputBuffer[i + 2] != 0 ||
                   outputBuffer[i + 3] != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
