using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using jaytwo.AspNet.SingleSignOn.Security;

namespace jaytwo.AspNet.SingleSignOn.WebHost.Handlers
{
    public class AppHostSignOnReturnHandler : AppHostHttpHandler
    {
        public AppHostSignOnReturnHandler(SsoAppHost appHost)
            : base(appHost)
        {
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var userInfo = AppHost.GetUserInformationFromAuthenticationProviderReturn(context.Request);
            AppHost.SignOnUser(context, userInfo);

            var redirectUrl = AppHost.GetApplicationReturnUrl(context.Request);
            context.Response.Redirect(redirectUrl);
        }
    }
}
