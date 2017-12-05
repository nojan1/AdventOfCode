using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// When something happened that just shouldn't
    /// </summary>
    public class WtfException : Exception
    {
        public WtfException()
        {
        }

        public WtfException(string message) : base(message)
        {
        }

        public WtfException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WtfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
