using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day14
{
    public class OneTimePad
    {
        public string Value { get; set; }
        public int Index { get; set; }
    }

    public class Program
    {
        const bool USEKEYSTRETCHING = true;

        static MD5 md5;
        static string Hash(string data)
        {
            if (md5 == null)
                md5 = MD5.Create();

            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        static OneTimePad CreatePossibleOneTimePad(string salt, int index, bool useKeyStretching)
        {
            var composite = salt + index.ToString();
            var hash = Hash(composite);

            if (useKeyStretching)
            {
                for (int i = 0; i < 2016; i++)
                {
                    hash = Hash(hash);
                }
            }

            return new OneTimePad
            {
                Value = hash,
                Index = index
            };
        }

        public static List<OneTimePad> CalculateOneTimePads(string salt, int count, bool useKeyStretching, int index = 1000)
        {
            var retval = new List<OneTimePad>();
            var hashBuffer = new List<OneTimePad>();
            var regex = new Regex(@"(.)\1\1");

            hashBuffer.AddRange(Enumerable.Range(index - 1000, index).Select(i => CreatePossibleOneTimePad(salt, i, useKeyStretching)));

            while (retval.Count < count)
            {
                hashBuffer.Add(CreatePossibleOneTimePad(salt, index++, useKeyStretching));

                var letterMatch = regex.Match(hashBuffer.First().Value);
                if (letterMatch.Success)
                {
                    var search = letterMatch.Groups[1].Value +
                                 letterMatch.Groups[1].Value +
                                 letterMatch.Groups[1].Value +
                                 letterMatch.Groups[1].Value +
                                 letterMatch.Groups[1].Value;

                    if (hashBuffer.Skip(1).Take(1000).Any(s => s.Value.Contains(search)))
                    {
                        //Got you
                        retval.Add(new OneTimePad
                        {
                            Value = hashBuffer.First().Value,
                            Index = hashBuffer.First().Index
                        });
                    }
                }

                hashBuffer.RemoveAt(0);
            }

            return retval;
        }

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            var pads = CalculateOneTimePads("ngcjuoqr", 64, USEKEYSTRETCHING);
            Console.WriteLine($"Pad value 64 was found on index {pads[63].Index}");

            var timeTaken = DateTime.Now - startTime;
            Console.WriteLine($"I ran in {timeTaken}");
        }
    }
}
