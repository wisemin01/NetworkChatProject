using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MNetwork.Exceptions
{
    class InitializeException : System.Exception
    {
        public InitializeException()
        {
        }

        public InitializeException(string message) : base(message)
        {
        }

        public InitializeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InitializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
