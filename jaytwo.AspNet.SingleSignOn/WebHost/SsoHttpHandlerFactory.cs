using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using jaytwo.AspNet.SingleSignOn.WebHost.Handlers;
using System.Web.Compilation;
using jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath;
using System.IO;
using System.Web.UI;
using jaytwo.AspNet.SingleSignOn.Exceptions;
using jaytwo.AspNet.SingleSignOn.Utilities;

namespace jaytwo.AspNet.SingleSignOn.WebHost
{
    public class SsoHttpHandlerFactory : IHttpHandlerFactory
    {
        public SsoHttpHandlerFactory()
            : this(SsoAppHost.Instance)
        {
        }

        public SsoHttpHandlerFactory(SsoAppHost appHost)
        {
            AppHost = appHost;
        }

        public SsoAppHost AppHost { get; private set; }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (AppHost == null)
            {
                throw new SsoHandlerFactoryException("SsoHttpHandlerFactory.AppHost not set");
            }

            IHttpHandler result = null;

            if (AreUrlsEqual(url, AppHost.SignOnHandlerPath))
            {
                result = new AppHostSignOnHandler(AppHost);
            }
            else if (AreUrlsEqual(url, AppHost.SignOnReturnHandlerPath))
            {
                result = new AppHostSignOnReturnHandler(AppHost);
            }
            else if (AreUrlsEqual(url, AppHost.SignOutHandlerPath))
            {
                result = new AppHostSignOutHandler(AppHost);
            }
            else if (AreUrlsEqual(url, AppHost.SignOutSuccessPagePath))
            {
                result = CreateHttpHandlerFromEmbeddedResource("jaytwo.AspNet.SingleSignOn.WebHost.Content.SignedOut.aspx");
            }
            else if (AreUrlsEqual(url, AppHost.UnauthorizedErrorPagePath))
            {
                result = CreateHttpHandlerFromEmbeddedResource("jaytwo.AspNet.SingleSignOn.WebHost.Content.Unauthorized.aspx");
            }
            else if (UrlStartsWith(url, AppHost.EmbeddedContentPath))
            {
                var embeddedResourceName = GetContentEmbeddedResourceKey(url, AppHost.EmbeddedContentPath);
                result = CreateHttpHandlerFromEmbeddedResource(embeddedResourceName);
            }

            if (result == null)
            {
                throw new HttpException(httpCode: 404, message: null, innerException: null);
            }

            return result;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        private static bool AreUrlsEqual(string requestedUrl, string compareUrl)
        {
            var relativeRequestedUrl = AspNetUtility.GetAppRelativePathWithoutTilde(requestedUrl);
            var relativeCompareUrl = AspNetUtility.GetAppRelativePathWithoutTilde(compareUrl);
            return string.Equals(relativeRequestedUrl, relativeCompareUrl, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool UrlStartsWith(string requestedUrl, string compareUrl)
        {
            var relativeRequestedUrl = AspNetUtility.GetAppRelativePathWithoutTilde(requestedUrl);
            var relativeCompareUrl = AspNetUtility.GetAppRelativePathWithoutTilde(compareUrl);
            return relativeRequestedUrl.StartsWith(relativeCompareUrl, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetContentEmbeddedResourceKey(string requestedUrl, string embeddedContentPath)
        {
            const string contentEmbeddedResourceKeyPrefix = "jaytwo.AspNet.SingleSignOn.WebHost.Content";

            var relativeRequestedUrl = AspNetUtility.GetAppRelativePathWithoutTilde(requestedUrl);
            var relativeEmbeddedContentPath = AspNetUtility.GetAppRelativePathWithoutTilde(embeddedContentPath);

            if (relativeRequestedUrl.StartsWith(relativeEmbeddedContentPath, StringComparison.InvariantCultureIgnoreCase))
            {
                var foo = relativeRequestedUrl.Substring(relativeEmbeddedContentPath.Length);
                var bar = foo.Replace('/', '.');
                var result = contentEmbeddedResourceKeyPrefix + bar;
                return result;
            }
            else
            {
                var message = string.Format("{0} is not in {1}", requestedUrl, embeddedContentPath);
                throw new SsoHandlerFactoryException(message);
            }
        }

        private static IHttpHandler CreateHttpHandlerFromEmbeddedResource(string embeddedResourceName)
        {
            var foundEmbeddedResource = EmbeddedResourceVirtualPathUtility.FindEmbeddedResource(embeddedResourceName, StringComparison.InvariantCultureIgnoreCase);

            if (foundEmbeddedResource == null)
            {
                var message = string.Format("Embedded Resource not found: {0}", embeddedResourceName);
                throw new HttpException(httpCode: 404, message: message, innerException: null);
            }

            return new EmbeddedResourceHttpHandler(foundEmbeddedResource);
        }
    }
}
