using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class StringCounter
    {
        private List<char> chars;

        public string Value { get => string.Concat(chars); }

        public StringCounter(string baseString)
        {
            chars = baseString.ToList();
        }

        public void Increment()
        {
            for(int i = chars.Count - 1; IncrementPosition(i) && i > 0; i--) { }
        }

        private bool IncrementPosition(int position)
        {
            if(chars[position] == 'z'){
                chars[position] = 'a';

                return true;
            }
            else
            {
                chars[position] = (char)(chars[position] + 1);

                return false;
            }
        }
    }
}
