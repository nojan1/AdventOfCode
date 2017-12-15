using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ConsoleHelper
    {
        private static TextWriter _outBackup;

        public static void DisableOutput()
        {
            _outBackup = Console.Out;
            Console.SetOut(TextWriter.Null);
        }

        public static void EnableOutput()
        {
            if (_outBackup == null)
                return;

            Console.SetOut(_outBackup);
        }
    }
}
