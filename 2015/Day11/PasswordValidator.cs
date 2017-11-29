using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class PasswordValidator
    {
        public static bool IsValid(string password)
        {
            if (!ContainsIncreasingStraight(password))
                return false;

            if (HasBadLetters(password))
                return false;

            if (!HasOverlappingPairs(password))
                return false;

            return true;
        }

        private static bool ContainsIncreasingStraight(string password)
        {
            for (int x = 'a'; x <= 'z'; x++)
            {
                var search = $"{(char)x}{(char)(x + 1)}{(char)(x + 2)}";
                if (password.Contains(search))
                    return true;
            }

            return false;
        }

        private static bool HasBadLetters(string password)
        {
            if (password.Contains('i') || password.Contains('o') || password.Contains('l'))
                return true;

            return false;
        }

        private static bool HasOverlappingPairs(string password)
        {
            int numPairs = 0;
            for(int x = 'a'; x <= 'z'; x++)
            {
                var search = $"{(char)x}{(char)x}";
                if (password.Contains(search))
                    numPairs++;
            }

            return numPairs >= 2;
        }
    }
}
