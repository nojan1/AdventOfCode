using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class BlockedRange
    {
        public UInt32 Start { get; set; }
        public UInt32 End { get; set; }
    }

    class Program
    {
        static List<BlockedRange> GetMergedBlockedRanges()
        {
            var allRanges = File.ReadAllLines("blacklist.txt").Select(t => t.Trim())
                        .Select(r =>
                        {
                            var parts = r.Split('-');
                            return new BlockedRange
                            {
                                Start = Convert.ToUInt32(parts[0].Trim()),
                                End = Convert.ToUInt32(parts[1].Trim())
                            };
                        }).OrderBy(b => b.Start)
                          .ToList();

            Func<UInt32, UInt32?> findEnd = null;
            findEnd = new Func<UInt32, UInt32?>(e =>
            {
                var ranges = allRanges.Where(r => e >= r.Start && e < r.End);
                if (!ranges.Any())
                {
                    return null;
                }

                var end = ranges.Max(r => r.End);
                return findEnd(end) ?? end;
            });

            var retval = new List<BlockedRange>();
            while (allRanges.Any())
            {
                var first = allRanges.First();

                var start = first.Start;
                var end = findEnd(first.End) ?? first.End;

                retval.Add(new BlockedRange
                {
                    Start = start,
                    End = end
                });

                allRanges.RemoveAll(r => r.End <= end);
            }

            return retval;
        }

        static void Main(string[] args)
        {
            UInt32 firstNonBlockedIp = 0;
            int allowedCount = 0;

            var startTime = DateTime.Now;

            var blocked = GetMergedBlockedRanges();

            for (long i = 0; i < UInt32.MaxValue; i++)
            {
                var range = blocked.FirstOrDefault(b => i >= b.Start && i <= b.End);
                if (range == null)
                {
                    if (firstNonBlockedIp == 0)
                        firstNonBlockedIp = (UInt32)i;

                    allowedCount++;
                }
                else
                {
                    i = range.End;
                }
            }

            var completionTime = DateTime.Now - startTime;

            Console.WriteLine($"Yo IP {firstNonBlockedIp} is safe like a ginger snaps and butter");
            Console.WriteLine($"There are {allowedCount} IP addresses allowed");
            Console.WriteLine($"Program ran in {completionTime.TotalMilliseconds} ms");
        }
    }
}
