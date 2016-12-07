using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            string secretKey = "bgvyzdsv";

            int answer = 0;
            while (true)
            {
                string composite = secretKey + answer.ToString();
                var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(composite));
                string hashHex = BitConverter.ToString(hash).Replace("-", "").ToLower();

                if (hashHex.StartsWith("000000"))
                {
                    break;
                }

                answer++;
            }

            Console.WriteLine("Secret answer is {0}", answer);
            Console.Read();
        }
    }
}
