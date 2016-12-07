using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day7
{
    class IPv7
    {
        public string[] HypernetSequences { get; private set; }
        public string RawAddress { get; private set; }

        public IPv7(string data)
        {
            RawAddress = data;

            var hypernetMatches = Regex.Matches(data, @"(\[.*?\])");
            if (hypernetMatches.Count > 0)
            {
                HypernetSequences = new string[hypernetMatches.Count];
                for (int i = 0; i < hypernetMatches.Count; i++)
                {
                    HypernetSequences[i] = hypernetMatches[i].Value;
                }
            }
        }

        public bool SupportsTLS()
        {
            return GetAbbaLikeStringsFromSubString(RawAddress, AbbaType.ABBA).Count > 0 && !HypernetSequences.Any(s => GetAbbaLikeStringsFromSubString(s, AbbaType.ABBA).Count > 0);
        }

        public bool SupportsSSL()
        {
            return Regex.Split(RawAddress, @"\[.*?\]").Any(ap => GetAbbaLikeStringsFromSubString(ap, AbbaType.ABA)
                                                        .Any(aba =>
            {
                var bab = aba[1].ToString() + aba[0].ToString() + aba[1].ToString();
                return HypernetSequences.Any(x => GetAbbaLikeStringsFromSubString(x, AbbaType.ABA).Contains(bab));
            }));
        }

        enum AbbaType
        {
            ABBA = 4,
            ABA = 3
        }
        private ICollection<string> GetAbbaLikeStringsFromSubString(string substring, AbbaType abbaType)
        {
            var returnValue = new List<string>();

            var que = new Queue<string>();
            foreach (var letter in substring)
            {
                que.Enqueue(letter.ToString());
                if (que.Count >= (int)abbaType)
                {
                    if (que.Count > (int)abbaType)
                        que.Dequeue();

                    if (abbaType == AbbaType.ABBA)
                    {
                        if (que.ElementAt(0) == que.ElementAt(3) &&
                           que.ElementAt(1) == que.ElementAt(2) &&
                           que.Distinct().Count() == 2)
                        {
                            returnValue.Add(string.Join("", que));
                        }
                    }
                    else
                    {
                        if (que.ElementAt(0) == que.ElementAt(2) &&
                           que.Distinct().Count() == 2)
                        {
                            returnValue.Add(string.Join("", que));
                        }
                    }
                }
            }

            return returnValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ips = File.ReadAllLines("input.txt").Select(t => new IPv7(t.Trim())).ToList();
            Console.WriteLine($"{ips.Count(i => i.SupportsTLS())} addresses supports TLS");
            Console.WriteLine($"{ips.Count(i => i.SupportsSSL())} addresses supports SSL");
        }
    }
}
