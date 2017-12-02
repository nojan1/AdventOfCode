using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        static int CalculateChecksum(IEnumerable<IEnumerable<int>> spreadsheet)
        {
            int checksum = 0;

            foreach(var row in spreadsheet)
            {
                checksum += row.Max() - row.Min();
            }

            return checksum;
        }

        static int CalculateChecksum2(IEnumerable<IEnumerable<int>> spreadsheet)
        {
            int checksum = 0;

            foreach (var row in spreadsheet)
            {
                var rowArr = row.ToArray();
                var done = false;

                for(int i = 0; i < rowArr.Length; i++)
                {
                    for(int z = 0; z < rowArr.Length; z++)
                    {
                        if(i != z && rowArr[i] % rowArr[z] == 0)
                        {
                            checksum += rowArr[i] / rowArr[z];
                            done = true;
                            break;
                        }
                    }

                    if (done)
                        break;
                }
            }

            return checksum;
        }

        static void Main(string[] args)
        {
            var spreadsheet = File.ReadAllLines("input.txt")
                .Select(l => Regex.Split(l, @"\s")
                                  .Select(x => Convert.ToInt32(x))
                                  .ToList())
                .ToList();

            var checksum = CalculateChecksum2(spreadsheet);
            Console.WriteLine($"The checksum is {checksum}");
        }
    }
}
