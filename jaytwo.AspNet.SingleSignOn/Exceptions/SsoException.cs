using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace jaytwo.AspNet.SingleSignOn.Exceptions
{
    [Serializable]
    public class SsoException : Exception
    {
        public SsoException()
            : base()
        {
        }

        public SsoException(string message)
            : base(message)
        {
        }

        public SsoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SsoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
