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

            if (part == DayPart.One)
            {
                processor.RunToEnd();

                var mulUsageCount = processor.InstructionUsageCount["mul"];
                Console.WriteLine($"The mul instruction was used {mulUsageCount} times");
            }
            else
            {
                int b, c, f, h = 0;

                b = 108400;
                c = b + 17000;

                //int d, e;
                //for (; b != c; b += 17)
                //{
                //    for (d = 2, f = 1; d != b; d++)
                //    {
                //        for (e = 2; e != b; e++)
                //        {
                //            if (d * e == b)
                //            {
                //                f = 0;
                //            }
                //        }
                //    }

                //    if (f == 0)
                //    {
                //        h++;
                //    }
                //}

                for(int i = b; i <= c; i+=17)
                {
                    f = 1;
                    for(int z = 2; z < (i / 2); z++)
                    {
                        if(i % z == 0)
                        {
                            f = 0;
                        }
                    }

                    if (f == 0)
                        h++;
                }

                Console.WriteLine($"The value of h is {h}");
            }
        }
    }
}
