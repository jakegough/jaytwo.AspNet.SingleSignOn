using jaytwo.Common.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace jaytwo.AspNet.SingleSignOn.Utilities
{
    public static class AspNetUtility
    {
        public static Uri GetApplicationUri(HttpContext context, string path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return GetApplicationUri(new HttpRequestWrapper(context.Request), path);
        }

        public static Uri GetApplicationUri(HttpRequest request, string path)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return GetApplicationUri(new HttpRequestWrapper(request), path);
        }

        public static Uri GetApplicationUri(HttpRequestBase request, string path)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string absolutePath;

            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                absolutePath = path;
            }
            else if (path.StartsWith("~/", StringComparison.Ordinal))
            {
                absolutePath = UrlHelper.Combine(request.ApplicationPath, path.Substring(2));
            }
            else
            {
                throw new ArgumentException("Path must be app relative (starts with '~/') or absolute (starts with '/').");
            }

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Url.Scheme;
            uriBuilder.Host = request.Url.Host;
            uriBuilder.Port = request.Url.Port;
            uriBuilder.Path = UrlHelper.GetPathOrUrlWithoutQuery(absolutePath);
            uriBuilder.Query = UrlHelper.GetQueryFromPathOrUrl(absolutePath);

            return uriBuilder.Uri;
        }

        public static string GetApplicationUrl(HttpContext context, string path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return GetApplicationUrl(new HttpRequestWrapper(context.Request), path);
        }

        public static string GetApplicationUrl(HttpRequest request, string path)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return GetApplicationUrl(new HttpRequestWrapper(request), path);
        }

        public static string GetApplicationUrl(HttpRequestBase request, string path)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return GetApplicationUri(request, path).AbsoluteUri;
        }

        public static Uri GetRootApplicationUri(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return GetRootApplicationUri(new HttpRequestWrapper(context.Request));
        }

        public static Uri GetRootApplicationUri(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return GetRootApplicationUri(new HttpRequestWrapper(request));
        }

        public static Uri GetRootApplicationUri(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return GetApplicationUri(request, "~/");
        }

        public static string GetRootApplicationUrl(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return GetRootApplicationUrl(new HttpRequestWrapper(context.Request));
        }

        public static string GetRootApplicationUrl(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return GetRootApplicationUrl(new HttpRequestWrapper(request));
        }

        public static string GetRootApplicationUrl(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return GetRootApplicationUri(request).AbsoluteUri;
        }

        public static string GetAppRelativePathWithTilde(string absolutePath)
        {
            return GetAppRelativePathWithTilde(HttpRuntime.AppDomainAppVirtualPath, absolutePath);
        }

        public static string GetAppRelativePathWithTilde(string appVirtualPath, string absolutePath)
        {
            if (appVirtualPath == null)
            {
                throw new ArgumentNullException("appVirtualPath");
            }

            if (absolutePath == null)
            {
                throw new ArgumentNullException("absolutePath");
            }

            var result = "~/" + GetAppRelativePathWithoutTilde(appVirtualPath, absolutePath);
            return result;
        }

        public static string GetAppRelativePathWithoutTilde(string path)
        {
            return GetAppRelativePathWithoutTilde(HttpRuntime.AppDomainAppVirtualPath, path);
        }

        public static string GetAppRelativePathWithoutTilde(string appVirtualPath, string path)
        {
            if (appVirtualPath == null)
            {
                throw new ArgumentNullException("appVirtualPath");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string absolutePath;

            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                absolutePath = path;
            }
            else if (path.StartsWith("~/", StringComparison.Ordinal))
            {
                absolutePath = UrlHelper.Combine(appVirtualPath, path.Substring(2));
            }
            else
            {
                throw new ArgumentException("Path must be app relative (starts with '~/') or absolute (starts with '/').");
            }

            appVirtualPath = appVirtualPath.TrimEnd('/') + "/";

            if (!absolutePath.StartsWith(appVirtualPath, StringComparison.OrdinalIgnoreCase))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "absolutePath '{0}' does not belng to appVirtualPath '{1}'",
                    absolutePath,
                    appVirtualPath);

                throw new ArgumentException(message);
            }

            var result = absolutePath.Substring(appVirtualPath.Length);
            return result;
        }
    }
}
