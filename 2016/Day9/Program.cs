using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        static int Decompress(string data)
        {
            int size = 0;

            using (var writer = new StreamWriter("output.file"))
            {
                var repeatCommand = new Regex(@"^(\((\d*)x(\d*)\)).");

                for (int i = 0; i < data.Length; i++)
                {
                    var repeatMatch = repeatCommand.Match(data.Substring(i));
                    if (repeatMatch.Success)
                    {
                        var characterCount = Convert.ToInt32(repeatMatch.Groups[2].Value);
                        var repeatCount = Convert.ToInt32(repeatMatch.Groups[3].Value);

                        var stringToRepeat = data.Substring(i + repeatMatch.Groups[1].Value.Length, characterCount);
                        for (int x = 0; x < repeatCount; x++)
                        {
                            size += stringToRepeat.Length;
                            writer.Write(stringToRepeat);
                        }

                        i += repeatMatch.Groups[1].Value.Length + characterCount - 1;
                    }
                    else
                    {
                        size++;
                        writer.Write(data.Substring(i, 1));
                    }
                }
            }

            return size;
        }

        static UInt64 EstimateSize(string data)
        {
            var repeatCommand = new Regex(@"^(\((\d*)x(\d*)\)).");
            UInt64 size = 0;
            UInt64 lastSizePrint = 0;

            while (!string.IsNullOrWhiteSpace(data))
            {
                var repeatMatch = repeatCommand.Match(data);
                if (repeatMatch.Success)
                {
                    var characterCount = Convert.ToInt32(repeatMatch.Groups[2].Value);
                    var repeatCount = Convert.ToInt32(repeatMatch.Groups[3].Value);

                    var stringToRepeat = data.Substring(repeatMatch.Groups[1].Value.Length, characterCount);
                    //var toInsert = "";
                    //for (int x = 0; x < repeatCount; x++)
                    //{
                    //    toInsert += stringToRepeat;
                    //}
                    size += EstimateSize(stringToRepeat) * (UInt64)repeatCount;

                    data = /*toInsert +*/ data.Substring(repeatMatch.Groups[1].Value.Length + stringToRepeat.Length);
                }
                else
                {
                    var index = data.IndexOf('(');
                    if(index > -1) { 
                        size += (UInt64)index;

                        data = data.Substring(index);
                    }else
                    {
                        size += (UInt64)data.Length;
                        data = "";
                    }
                    //size++;
                    //data = data.Substring(1);
                }

                if(size - lastSizePrint >= 50000000)
                {
                    lastSizePrint = size;

                    Console.Clear();
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Size: {size}, Data length: {data.Length}");
                }
            }

            return size;
        }

        static void Main(string[] args)
        {
            var compressed = File.ReadAllText("input.txt");
            //var decompressedSize = Decompress(compressed);

            //Console.WriteLine($"The decompressed length is {decompressedSize}");

            UInt64 size = EstimateSize(compressed);
            Console.WriteLine($"The size is: {size}");
        }
    }
}
