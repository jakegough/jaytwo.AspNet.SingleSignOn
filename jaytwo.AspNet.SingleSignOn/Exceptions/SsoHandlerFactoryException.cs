using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace jaytwo.AspNet.SingleSignOn.Exceptions
{
    [Serializable]
    public class SsoHandlerFactoryException : SsoException
    {
        public SsoHandlerFactoryException()
            : base()
        {
        }

        public SsoHandlerFactoryException(string message)
            : base(message)
        {
        }

        public SsoHandlerFactoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SsoHandlerFactoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
