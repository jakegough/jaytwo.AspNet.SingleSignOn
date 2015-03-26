using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Compilation;
using System.Web.UI;
using jaytwo.AspNet.SingleSignOn.Utilities;

namespace jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath
{
    public class EmbeddedResourceHttpHandler : IHttpHandler
    {
        public string EmbeddedResourceName { get; private set; }

        public EmbeddedResourceHttpHandler(string embeddedResourceName)
        {
            EmbeddedResourceName = embeddedResourceName;
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var fileExtension = Path.GetExtension(EmbeddedResourceName);

            if (EmbeddedResourceVirtualPathUtility.IsHttpHandlerResource(fileExtension))
            {
                var virtualPath = EmbeddedResourceVirtualPathUtility.GetVirtualPathFromEmbeddedResource(EmbeddedResourceName);
                var handler = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(IHttpHandler));

                if (handler == null)
                {
                    throw new HttpException(httpCode: 404, message: null, innerException: null);
                }

                var handlerAsTemplateControl = (handler as TemplateControl);
                if (handlerAsTemplateControl != null)
                {
                    handlerAsTemplateControl.AppRelativeVirtualPath = context.Request.AppRelativeCurrentExecutionFilePath;
                }

                handler.ProcessRequest(context);
            }
            else
            {
                context.Response.ContentType = MimeTypeUtility.GetMimeType(fileExtension);

                using (var contentStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResourceName))
                {
                    contentStream.CopyTo(context.Response.OutputStream);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}