using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    class Program
    {
        static int Part1(string input)
        {
            int captcha = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var next = i == input.Length - 1 ? 0 : i + 1;

                if (input[i] == input[next])
                {
                    captcha += Convert.ToInt32(input[i].ToString());
                }
            }

            return captcha;
        }

        private static int Part2(string input)
        {
            int captcha = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var next = i + (input.Length / 2);
                if(next > input.Length - 1)
                {
                    next -= input.Length;
                }

                if (input[i] == input[next])
                {
                    captcha += Convert.ToInt32(input[i].ToString());
                }
            }

            return captcha;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            //var captcha1 = Part1(input);
            var captcha = Part2(input);

            Console.WriteLine($"The captcha is: {captcha}");
        }
    }
}
