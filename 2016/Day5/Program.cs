using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var doorId = "ojvtpuvg";

            var password = new char[] { '_', '_', '_', '_', '_', '_', '_', '_' };
            int index = 0;
            var guessBuffer = new List<string>();

            Print(password, guessBuffer);

            while (true)
            {
                var hashInput = doorId + (index++).ToString();
                var hash = CalculateHash(hashInput);

                if (hash.StartsWith("00000"))
                {
                    int position;
                    if (!int.TryParse(hash[5].ToString(), out position) || position > 7 || password[position] != '_')
                        continue;

                    var character = hash[6];

                    password[position] = character;

                    guessBuffer.Add($"Position: {position}, Character: {character}, Index: {index-1}");
                    Print(password, guessBuffer);

                    if (password.Count(c => c == '_') == 0)
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("==========================");
            Console.WriteLine($"The password is {string.Join("", password)}");
        }

        private static void Print(char[] password, List<string> guessBuffer)
        {
            Console.Clear();
            Console.WriteLine(string.Join(" ", password));
            Console.WriteLine();

            if (guessBuffer.Any())
            {
                Console.WriteLine("--------------------------------");
                guessBuffer.ForEach(s => Console.WriteLine(s));
            }
        }

        private static string CalculateHash(string hashInput)
        {
            using(var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.Default.GetBytes(hashInput));
                return string.Join("", data.Select(b => b.ToString("x2")));
            }
        }
    }
}
