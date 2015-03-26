using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Reflection;

namespace jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath
{
    public class EmbeddedResourceVirtualFile : VirtualFile
    {
        protected string EmbeddedResourceName { get; private set; }

        public EmbeddedResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            EmbeddedResourceName = EmbeddedResourceVirtualPathUtility.GetEmbeddedResourceNameFromVirtualPath(virtualPath);
        }

        public override System.IO.Stream Open()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResourceName);
        }
    }
}
