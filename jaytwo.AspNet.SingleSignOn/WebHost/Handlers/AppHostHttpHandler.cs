using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath;
using System.Security.Principal;

namespace jaytwo.AspNet.SingleSignOn.WebHost.Handlers
{
    public abstract class AppHostHttpHandler : IHttpHandler
    {
        protected SsoAppHost AppHost { get; private set; }

        protected AppHostHttpHandler(SsoAppHost appHost)
        {
            AppHost = appHost;
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public abstract void ProcessRequest(HttpContextBase context);

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}