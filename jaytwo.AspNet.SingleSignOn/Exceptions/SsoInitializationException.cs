using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace jaytwo.AspNet.SingleSignOn.Exceptions
{
    [Serializable]
    public class SsoInitializationException : SsoException
    {
        public SsoInitializationException()
            : base()
        {
        }

        public SsoInitializationException(string message)
            : base(message)
        {
        }

        public SsoInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SsoInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
