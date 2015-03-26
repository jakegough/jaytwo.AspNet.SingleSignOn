using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace jaytwo.AspNet.SingleSignOn.Security
{
    public class AuthenticationProviderUserInfo
    {
        public string UserName { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}
