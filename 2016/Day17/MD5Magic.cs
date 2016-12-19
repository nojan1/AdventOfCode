using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public static class MD5Magic
    {
        private static MD5 md5;
        public static string Hash(string data)
        {
            if (md5 == null)
                md5 = MD5.Create();

            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
