using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class HotBreath
    {
        public string Data { get; set; }
        public string Checksum { get; set; }
    }

    class DragonDataBreather
    {
        private string _startData;
        public DragonDataBreather(string startData)
        {
            _startData = startData;
        }

        public HotBreath BreathBabyBreath(int diskSize)
        {
            var data = GenerateData(diskSize);
            data.Length = diskSize;

            var checksum = GenerateChecksum(data);

            return new HotBreath
            {
                Data = data.ToString(),
                Checksum = checksum
            };
        }

        private StringBuilder GenerateData(int targetLength)
        {
            var sb = new StringBuilder(_startData);

            while (sb.Length < targetLength)
            {
                var newData = sb.ToString()
                                .Replace('0', 'x')
                                .Replace('1', '0')
                                .Replace('x', '1')
                                .ToCharArray();

                Array.Reverse(newData);

                sb.Append('0');
                sb.Append(newData);
            }

            return sb;
        }

        private string GenerateChecksum(StringBuilder data)
        {
            var checkSumBuilder = new StringBuilder();

            for (int i = 0; i < data.Length - 1; i += 2)
            {
                checkSumBuilder.Append(data[i] == data[i + 1] ? "1" : "0");
            }

            if (checkSumBuilder.Length % 2 == 0)
            {
                var newData = new StringBuilder(checkSumBuilder.ToString());
                return GenerateChecksum(newData);
            }
            else
            {
                return checkSumBuilder.ToString();
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var data = new DragonDataBreather("10010000000110000");
            var fire = data.BreathBabyBreath(35651584);

            Console.WriteLine($"The checksum for this ball of fire is {fire.Checksum}");
        }
    }
}
