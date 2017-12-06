using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day6
{
    static class BlockArrayExtension
    {
        public static int[] ToIntArray(this Block[] arr)
        {
            return arr.OrderBy(b => b.Index).Select(b => b.Value).ToArray();
        }
    }

    struct Block
    {
        public int Value { get; set; }
        public int Index { get; set; }
    }
    
    class Program
    {
        static int FindDuplicate(List<int[]> previous, int[] now)
        {
            for(int i = 0; i < previous.Count - 1; i++)
            {
                if (now.SequenceEqual(previous[i]))
                    return i;
            }

            return -1;
        }

        static void Main(string[] args)
        {
            string input = "0	5	10	0	11	14	13	4	11	8	8	7	1	4	12	11";

            int duplicateIndex;
            int numCycles = 0;
            var previousBanks = new List<int[]>();
            var banks = Regex.Split(input, @"\s").Select((x,i) => new Block { Index = i, Value = Convert.ToInt32(x) }).ToArray();

            previousBanks.Add(banks.ToIntArray());

            while ((duplicateIndex = FindDuplicate(previousBanks, banks.ToIntArray())) == -1)
            {
                var index = banks.OrderByDescending(b => b.Value).ThenBy(b => b.Index).First().Index;

                var toDistribute = banks[index].Value;
                banks[index].Value = 0;
                
                while(toDistribute > 0)
                {
                    if (++index > banks.Length - 1)
                        index = 0;

                    banks[index].Value++;
                    toDistribute--;
                }

                numCycles++;
                previousBanks.Add(banks.ToIntArray());
            }

            Console.WriteLine($"Number of cycles {numCycles}");

            int loopLength = numCycles - duplicateIndex;
            Console.WriteLine($"...and the loop is {loopLength} cycles long");
        }
    }
}
