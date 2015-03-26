using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace jaytwo.AspNet.SingleSignOn.WebHost.Content
{
    public partial class Unauthorized : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string GetSignOutUrl()
        {
            return VirtualPathUtility.ToAbsolute(SsoAppHost.Instance.SignOutHandlerPath);
        }
    }
}