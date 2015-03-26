using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace jaytwo.AspNet.SingleSignOn.Utilities
{
    public static class FormsAuthenticationTicketUtility
    {
        public static void ClearFormsAuthenticationTicket(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                cookie.Value = null;
                cookie.Expires = DateTime.MinValue;
            }

            if (context.Response.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                cookie.Value = null;
                cookie.Expires = DateTime.MinValue;
            }
        }

        public static void SetFormsAuthenticationTicket(HttpContextBase context, string userName, string userData)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var ticket = GetFormsAuthenticationTicket(userName, userData);
            SetFormsAuthenticationTicket(context, ticket);
        }

        public static void SetFormsAuthenticationTicket(HttpContextBase context, FormsAuthenticationTicket ticket)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            if (context.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                context.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }

            context.Request.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));

            if (context.Response.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }

            context.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
        }

        public static FormsAuthenticationTicket GetFormsAuthenticationTicket()
        {
            return GetFormsAuthenticationTicket(new HttpContextWrapper(HttpContext.Current));
        }

        public static FormsAuthenticationTicket GetFormsAuthenticationTicket(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return GetFormsAuthenticationTicket(context.Request);
        }

        public static FormsAuthenticationTicket GetFormsAuthenticationTicket(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            FormsAuthenticationTicket result = null;

            if (request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                var encryptedTicket = request.Cookies[FormsAuthentication.FormsCookieName].Value;
                result = FormsAuthentication.Decrypt(encryptedTicket);
            }

            return result;
        }

        public static T GetFormsAuthenticationTicketUserDataAs<T>()
        {
            var ticket = GetFormsAuthenticationTicket();
            var userDataJson = ticket.UserData;
            var result = new JavaScriptSerializer().Deserialize<T>(userDataJson);
            return result;
        }

        public static FormsAuthenticationTicket GetFormsAuthenticationTicket(string userName, string userData)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 0,
                name: userName,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddYears(1),
                isPersistent: false,
                userData: userData);

            return ticket;
        }
    }
}
