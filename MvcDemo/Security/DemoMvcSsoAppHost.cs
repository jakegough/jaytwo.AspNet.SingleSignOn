using jaytwo.AspNet.FormsAuth;
using jaytwo.AspNet.SingleSignOn;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace jaytwo.AspNet.MvcDemo.Security
{
    public class DemoMvcSsoAppHost : OpenIdAppHost
    {
        public DemoMvcSsoAppHost()
            : base(discoveryUrl: "https://me.yahoo.com")
        {
        }

        public override IUserProfile GetProfileForUser(AuthenticationProviderUserInfo userInfo)
        {
            var result = new DemoMvcUserProfile();
            result.UserName = userInfo.UserName;

            // in the OpenIdAppHost.GetUserInformationFromAuthenticationProviderReturn we set the UserName to the email address
            result.EmailAddress = userInfo.UserName;
            result.SignInTimeUtc = DateTime.UtcNow;

            return result;
        }

        public override string[] GetRolesForUser(AuthenticationProviderUserInfo userInfo)
        {
            return new[] { Role.User };
        }
    }
}