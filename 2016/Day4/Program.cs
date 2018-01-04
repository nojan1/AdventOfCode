using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day4
{

    class RoomSpecification
    {
        public int SectorID { get; private set; }
        public string Checksum { get; private set; }
        public bool IsReal { get; private set; }
        public string UnEncryptedName { get; set; }

        public RoomSpecification(string encryptedString)
        {
            var allParts = encryptedString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            var nameParts = allParts.Take(allParts.Length - 1);
            var lastPartComponents = Regex.Match(allParts.Last(), @"^(\d*)\[(\w*)\]$");

            SectorID = Convert.ToInt32(lastPartComponents.Groups[1].Value);
            Checksum = lastPartComponents.Groups[2].Value;

            var calculatedChecksum = CalculateChecksum(nameParts);
            IsReal = Checksum == calculatedChecksum;

            UnEncryptedName = DecryptName(nameParts, SectorID);
        }

        private string DecryptName(IEnumerable<string> nameParts, int sectorID)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            var decryptedParts = nameParts.Select(part =>
            {
                var decrypted = new StringBuilder();
                foreach (var letter in part)
                {
                    var index = (alphabet.IndexOf(letter) + SectorID) % alphabet.Length;
                    decrypted.Append(alphabet[index]);
                }

                return decrypted.ToString();
            });

            return String.Join(" ", decryptedParts);
        }

        private string CalculateChecksum(IEnumerable<string> parts)
        {
            var letterHaystack = string.Join("", parts);

            var countLetter = new Func<char, int>(c =>
            {
                int count = 0;
                foreach(var x in letterHaystack)
                {
                    if (x == c)
                        count++;
                }

                return count;
            });

            var checkSumBuilder = new StringBuilder();
            var letterCounts = letterHaystack.Distinct().GroupBy(x => countLetter(x), x => x.ToString()).ToDictionary(x => x.Key, x => x);

            foreach(var count in letterCounts.Keys.OrderByDescending(x => x))
            {
                checkSumBuilder.Append(string.Join("", letterCounts[count].OrderBy(x => x)));
            }

            return checkSumBuilder.ToString().Substring(0, 5);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var rooms = File.ReadAllLines("input.txt").Select(l => new RoomSpecification(l));

            var sum = rooms.Where(r => r.IsReal).Sum(r => r.SectorID);
            var northPoleRoom = rooms.FirstOrDefault(r => r.UnEncryptedName.Contains("pole"));

            Console.WriteLine($"The sector id sum is: {sum}");

            if(northPoleRoom != null)
            {
                Console.WriteLine($"North pole objects is in sector id {northPoleRoom.SectorID}");
            }
        }
    }
}
