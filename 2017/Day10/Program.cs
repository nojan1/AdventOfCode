using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class KnotHasher
    {
        internal static byte[] CalculateSparseHash(IEnumerable<byte> lengths, int numRounds)
        {
            var numbers = new CircularList<byte>(Enumerable.Range(0, 256).Select(x => (byte)x));
            int currentPosition = 0;
            int skipSize = 0;

            for (int r = 0; r < numRounds; r++)
            {
                foreach (var length in lengths)
                {
                    var workingCopy = new List<byte>();
                    for (byte i = 0; i < length; i++)
                    {
                        workingCopy.Add(numbers[currentPosition + i]);
                    }

                    workingCopy.Reverse();

                    numbers.ReplaceRange(workingCopy, currentPosition);

                    currentPosition += length + skipSize;
                    skipSize++;
                }
            }

            return numbers.ToArray();
        }

        public static string CalculateDenseHash(string value)
        {
            var inputBytes = value.Select(x => (byte)x).ToList();
            inputBytes.AddRange(new byte[] { 17, 31, 73, 47, 23 });

            return CalculateDenseHash(inputBytes);
        }

        public static string CalculateDenseHash(IEnumerable<byte> lengths)
        {
            var sparseHash = CalculateSparseHash(lengths, 64);
            var blocks = Enumerable.Range(0, 16).Select(i => sparseHash.Skip(i * 16).Take(16).ToArray()).ToArray();
            var blockValues = blocks.Select(x => x.Aggregate((tot, self) => (byte)(tot ^ self))).ToArray();

            return string.Concat(blockValues.Select(x => x.ToString("x2")));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = "187,254,0,81,169,219,1,190,19,102,255,56,46,32,2,216";

            var lengthInts = input.Split(',').Select(i => Convert.ToByte(i)).ToArray();
            var part1SparseHash = KnotHasher.CalculateSparseHash(lengthInts, 1);
            var part1Answer = (int)part1SparseHash[0] * (int)part1SparseHash[1];

            Console.WriteLine($"The answer to part1 is {part1Answer}");

            var part2DenseHash = KnotHasher.CalculateDenseHash(input);

            Console.WriteLine($"Part 2 dense hash is: '{part2DenseHash}'");
        }
    }
}
