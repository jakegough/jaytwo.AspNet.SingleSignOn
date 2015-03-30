using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace jaytwo.AspNet.SingleSignOn.WebHost.Handlers
{
    public class AppHostSignOutHandler : AppHostHttpHandler
    {
        public AppHostSignOutHandler(SsoAppHost appHost)
            : base(appHost)
        {
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            AppHost.SignOutUser();
            context.Response.Redirect(AppHost.SignOutSuccessPagePath);
        }
    }
}
