using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day14
{
    class OneTimePad
    {
        public string Value { get; set; }
        public int Index { get; set; }
    }

    class Program
    {
        const bool USEKEYSTRETCHING = true;

        static string Hash(string data)
        {
            var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(data));
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

        static List<OneTimePad> CalculateOneTimePads(string salt, int count)
        {
            var retval = new List<OneTimePad>();
            var hashBuffer = new List<OneTimePad>();
            var regex = new Regex(@"(.)\1\1");

            var index = 1000;
            hashBuffer.AddRange(Enumerable.Range(0, index).Select(i => CreatePossibleOneTimePad(salt, i, USEKEYSTRETCHING)));

            while (retval.Count < count)
            {
                hashBuffer.Add(CreatePossibleOneTimePad(salt, index++, USEKEYSTRETCHING));

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
            var pads = CalculateOneTimePads("ngcjuoqr", 64);
            Console.WriteLine($"Pad value 64 was found on index {pads[63].Index}");
        }
    }
}
