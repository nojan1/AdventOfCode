using Common;
using Day18;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var part = DayPart.Two;
            var instructions = File.ReadAllLines("input.txt");

            var processor = new Processor(instructions, DayPart.One);

            if(part == DayPart.One)
            {
                processor.RunToEnd();

                var mulUsageCount = processor.InstructionUsageCount["mul"];
                Console.WriteLine($"The mul instruction was used {mulUsageCount} times");
            }
            else
            {
                int pointer = 0;
                long hValue = -1;

                processor.GetRegister("a").Value = 1;

                while(pointer != -1)
                {
                    pointer = processor.RunSingleInstruction(pointer);

                    if(hValue != processor.GetRegister("h").Value)
                    {
                        hValue = processor.GetRegister("h").Value;
                        Console.WriteLine($"Register H: {hValue}");
                    }
                }
            }
        }
    }
}
