using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ListExtensions
    {
        public static void ReplaceRange<T>(this IList<T> list, IList<T> other, int offset)
        {
            for(int i = 0; i < other.Count; i++)
            {
                list[i + offset] = other[i];
            }
        }
    }
}
