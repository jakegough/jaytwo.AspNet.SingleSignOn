using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath;
using System.Security.Principal;
using jaytwo.AspNet.SingleSignOn.Utilities;
using jaytwo.Common.Http;

namespace jaytwo.AspNet.SingleSignOn.WebHost.Handlers
{
    public class AppHostSignOnHandler : AppHostHttpHandler
    {
        public AppHostSignOnHandler(SsoAppHost appHost)
            : base(appHost)
        {
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var userIsAuthenticated = (context.User != null) && (context.User.Identity != null) && context.User.Identity.IsAuthenticated;
            var relativeReturnUrl = context.Request.QueryString[QueryStringParameters.ReturnUrl];

            var authenticatedButNotAuthorized = (userIsAuthenticated && !string.IsNullOrEmpty(relativeReturnUrl));

            if (authenticatedButNotAuthorized)
            {
                // because unauthorized and unauthenticated are handled the same in .net...
                //    if the user is authenticated, and yet is redirected here with a redirect url, 
                //    then we have an unauthorized issue, not an unauthenticated issue

                var redirectUrl = AppHost.UnauthorizedErrorPagePath;
                redirectUrl = UrlHelper.SetUrlQueryStringParameter(redirectUrl, QueryStringParameters.ReturnUrl, relativeReturnUrl);

                context.Response.Redirect(redirectUrl);
            }
            else
            {
                var returnHandlerUrl = AspNetUtility.GetApplicationUrl(context.Request, AppHost.SignOnReturnHandlerPath);
                returnHandlerUrl = UrlHelper.SetUrlQueryStringParameter(returnHandlerUrl, QueryStringParameters.ReturnUrl, relativeReturnUrl);
                returnHandlerUrl = AppHost.TransformReturnHandlerUrl(returnHandlerUrl);

                var redirectUrl = AppHost.GetRedirectToAuthenticationProviderUrl(context.Request, returnHandlerUrl);

                context.Response.Redirect(redirectUrl);
            }
        }
    }
}