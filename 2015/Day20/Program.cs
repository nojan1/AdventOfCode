using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Program
    {
        private static int FindHouse(int target)
        {
            int houseNum = 0;
            int numPresents = 0;

            while (numPresents < target)
            {
                houseNum++;
                numPresents = 0;

                for(int elfNum = 1; elfNum <= houseNum; elfNum++)
                {
                    if (houseNum % elfNum == 0)
                        numPresents += 10 * elfNum;
                }
            }

            return houseNum;
        }

        static void Main(string[] args)
        {
            var input = 33100000;
            var houseNum = FindHouse(input);

            Console.WriteLine($"The house is house number {houseNum}");
        }
    }
}
