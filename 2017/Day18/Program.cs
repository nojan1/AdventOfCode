using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static long A(string[] instructions)
        {
            long lastValue = -1;
            bool hasRecovered = false;

            var processor = new Processor(instructions, Common.DayPart.One);
            processor.OnRecieve += x => hasRecovered = true;
            processor.OnSend += val => lastValue = val;

            int pointer = 0;
            while (!hasRecovered)
                pointer = processor.RunSingleInstruction(pointer);

            return lastValue;
        }

        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt");

            int sendCount = 0;

            int pointer1 = 0;
            var processor1 = new Processor(instructions, Common.DayPart.Two);
            processor1.GetRegister("p").Value = 0;

            int pointer2 = 0;
            var processor2 = new Processor(instructions, Common.DayPart.Two);
            processor2.GetRegister("p").Value = 1;

            processor1.OnSend += val => processor2.InputQueue.Enqueue(val);
            processor2.OnSend += val => { processor1.InputQueue.Enqueue(val); sendCount++; };

            while (ProcessorStepped(processor1, ref pointer1) || ProcessorStepped(processor2, ref pointer2)) { }

            Console.WriteLine($"Program1 send {sendCount} values");
        }

        static bool ProcessorStepped(Processor processor, ref int pointer)
        {
            var pointerBefore = pointer;
            pointer = processor.RunSingleInstruction(pointerBefore);

            return pointerBefore != pointer;
        }
    }
}
