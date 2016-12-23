using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Program
    {
        static string Scrambler(ICollection<string> instructions, string data, bool deScramble = false)
        {
            foreach (var instruction in deScramble ? instructions.Reverse() : instructions)
            {
                var parts = instruction.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                switch (parts[0])
                {
                    case "swap":
                        var row = data.ToCharArray();

                        int from, to;
                        if (!int.TryParse(parts[2], out from))
                        {
                            from = data.IndexOf(parts[2]);
                        }

                        if (!int.TryParse(parts[5], out to))
                        {
                            to = data.IndexOf(parts[5]);
                        }

                        var backup = data[to];
                        row[to] = row[from];
                        row[from] = backup;

                        data = string.Join("", row);
                        break;
                    case "reverse":
                        int start = Convert.ToInt32(parts[2]);
                        int end = Convert.ToInt32(parts[4]);

                        var reversedPart = string.Join("", data.Substring(start, (end - start) + 1).Reverse());

                        data = data.Substring(0, start) + reversedPart + (end + 1 < data.Length ? data.Substring(end + 1) : "");

                        break;
                    case "rotate":
                        if (parts[1] == "based")
                        {
                            if (deScramble)
                            {
                                var workingData = data;

                                do
                                {
                                    workingData = Rotate(workingData, "left", 1);
                                } while (BaseRotate(workingData, parts[6]) != data);

                                data = workingData;
                            }
                            else
                            {
                                data = BaseRotate(data, parts[6]);
                            }
                        }
                        else
                        {
                            var steps = Convert.ToInt32(parts[2]);
                            data = Rotate(data, (deScramble ? (parts[1] == "left" ? "right" : "left") : parts[1]), steps);
                        }

                        break;
                    case "move":
                        from = Convert.ToInt32(parts[2]);
                        to = Convert.ToInt32(parts[5]);

                        if (deScramble)
                        {
                            var temp = from;
                            from = to;
                            to = temp;
                        }

                        var charToMove = data[from];

                        if (to > from)
                        {
                            data = data.Substring(0, from) + data.Substring(from + 1, (to - from)) + charToMove.ToString() + data.Substring(to + 1);
                        }
                        else
                        {
                            data = data.Substring(0, to) + charToMove.ToString() + data.Substring(to, (from - to)) + data.Substring(from + 1);
                        }

                        break;
                    default:
                        throw new Exception($"This instruction makes no sense: '{instruction}'");
                }
            }

            return data;
        }

        static string BaseRotate(string data, string search)
        {
            var index = data.IndexOf(search);

            data = Rotate(data, "right", 1);

            for (int i = 0; i < index; i++)
            {
                data = Rotate(data, "right", 1);
            }

            if (index >= 4)
            {
                data = Rotate(data, "right", 1);
            }

            return data;
        }

        static string Rotate(string input, string direction, int steps)
        {
            if (direction == "left")
            {
                var leftPart = input.Substring(steps);
                var rightPart = input.Substring(0, steps);

                return leftPart + rightPart;
            }
            else
            {
                var leftPart = input.Substring(input.Length - steps, steps);
                var rightPart = input.Substring(0, input.Length - steps);

                return leftPart + rightPart;
            }
        }

        static void Main(string[] args)
        {
            var unscrambled = "abcdefgh";
            var scrambled = "fbgdceah";
            var instructions = File.ReadAllLines("input.txt").Select(t => t.Trim()).ToArray();

            Console.WriteLine($"The scrambled password is '{Scrambler(instructions, unscrambled, false)}'... happy now?");
            Console.WriteLine($"The unscrambled password is '{Scrambler(instructions, scrambled, true)}'... you better be happy now!");
        }
    }
}
