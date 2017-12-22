using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<int> Sequence(int start, int length, int skip)
        {
            for(int i = start; i < (start + length); i += skip)
            {
                yield return i;
            }
        }
    }
}
