using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Reflection;
using jaytwo.AspNet.SingleSignOn.Utilities;

namespace jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath
{
    public static class EmbeddedResourceVirtualPathUtility
    {
        const string appRelativePathPrefix = "~/__embedded_resource.";

        // http://www.codeproject.com/Articles/15494/Load-WebForms-and-UserControls-from-Embedded-Resou				

        public static bool IsHttpHandlerResource(string fileExtension)
        {
            //http://forums.asp.net/t/1832098.aspx?Difference+between+asmx+aspx+and+ashx	
            if (!string.IsNullOrWhiteSpace(fileExtension))
            {
                switch (fileExtension.ToLowerInvariant())
                {
                    // case ".asax": // protected?
                    case ".aspx":
                    case ".ashx":
                    case ".asmx":
                    case ".ascx":
                        return true;
                }
            }

            return false;
        }

        public static string FindEmbeddedResource(string embeddedResourceName, StringComparison comparisonType)
        {
            var manifestResourceNames = typeof(EmbeddedResourceVirtualPathUtility).Assembly.GetManifestResourceNames();

            return manifestResourceNames.FirstOrDefault(x => x == embeddedResourceName)
                ?? manifestResourceNames.FirstOrDefault(x => x.Equals(embeddedResourceName, comparisonType));
        }

        public static string GetVirtualPathFromEmbeddedResource(string embeddedResourceName)
        {
            return appRelativePathPrefix + embeddedResourceName;
        }

        public static string GetEmbeddedResourceNameFromVirtualPath(string virtualPath)
        {
            if (!IsEmbeddedResourcePath(virtualPath))
            {
                throw new ArgumentException("virtualPath is not an embedded resource path");
            }

            var appRelativePath = VirtualPathUtility.ToAppRelative(virtualPath);
            var result = appRelativePath.Substring(appRelativePathPrefix.Length);
            return result;
        }

        public static bool IsEmbeddedResourcePath(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(appRelativePathPrefix, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
