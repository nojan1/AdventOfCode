using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string oldPassword = "vzbxkghb";
            var password1 = GetNextPassword(oldPassword);
            var password2 = GetNextPassword(password1);

            Console.WriteLine($"New password is: {password2}");
        }

        static string GetNextPassword(string password)
        {
            var stringCounter = new StringCounter(password);

            do
            {
                stringCounter.Increment();
            } while (!PasswordValidator.IsValid(stringCounter.Value));

            return stringCounter.Value;
        }
    }
}
