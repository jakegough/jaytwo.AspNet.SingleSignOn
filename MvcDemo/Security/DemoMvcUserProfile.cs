using jaytwo.AspNet.FormsAuth;
using jaytwo.AspNet.SingleSignOn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jaytwo.AspNet.MvcDemo.Security
{
    public class DemoMvcUserProfile : IUserProfile
    {
        public static DemoMvcUserProfile Current
        {
            get
            {
                return SsoAppHost.Instance.GetCurrentUserProfile<DemoMvcUserProfile>();
            }
        }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SignInTimeUtc { get; set; }
    }
}