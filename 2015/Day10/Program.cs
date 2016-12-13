using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static string LookAndSay(string value)
        {
            var sb = new StringBuilder();

            var digitGroups = Regex.Matches(value, @"([1]+|[2]+|[3]+|[4]+|[5]+|[6]+|[7]+|[8]+|[9]+|[0]+)");
            foreach(Match digitGroup in digitGroups)
            {
                sb.Append(digitGroup.Groups[1].Value.Length.ToString() + digitGroup.Groups[1].Value[0]);
            }

            return sb.ToString();
        }

        static void Main(string[] args)
        {
            var data = "1321131112";

            for(int i = 0; i < 50; i++)
            {
                data = LookAndSay(data);
            }

            Console.WriteLine($"The answer length is {data.Length}");
        }
    }
}
