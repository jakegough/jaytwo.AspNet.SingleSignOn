using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Caching;
using System.Collections;

namespace jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath
{
    public class EmbeddedResourceVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            return (EmbeddedResourceVirtualPathUtility.IsEmbeddedResourcePath(virtualPath) || base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (EmbeddedResourceVirtualPathUtility.IsEmbeddedResourcePath(virtualPath))
            {
                return new EmbeddedResourceVirtualFile(virtualPath);
            }
            else
            {
                return base.GetFile(virtualPath);
            }
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (EmbeddedResourceVirtualPathUtility.IsEmbeddedResourcePath(virtualPath))
            {
                return null;
            }
            else
            {
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }
    }
}
