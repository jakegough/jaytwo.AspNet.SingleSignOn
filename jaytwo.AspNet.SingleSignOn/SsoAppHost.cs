using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using jaytwo.AspNet.SingleSignOn.Security;
using System.Web;
using jaytwo.AspNet.SingleSignOn.WebHost.Handlers;
using System.Web.Hosting;
using jaytwo.AspNet.SingleSignOn.WebHost.VirtualPath;
using System.Collections.Specialized;
using jaytwo.AspNet.SingleSignOn.Utilities;
using jaytwo.AspNet.SingleSignOn.WebHost;
using jaytwo.AspNet.SingleSignOn.Exceptions;
using System.Web.Security;
using System.Web.Routing;
using jaytwo.Common.Http;

namespace jaytwo.AspNet.SingleSignOn
{
	public abstract class SsoAppHost
	{
		public static readonly string SignOnHandlerResourceName = "sign_on";
		public static readonly string SignOnReturnHandlerResourceName = "sign_on_return";
		public static readonly string SignOutHandlerResourceName = "sign_out";
		public static readonly string EmbeddedContentPathResourceName = "content";
		public static readonly string DefaultSignOutSuccessPageResourceName = "sign_out_success";
		public static readonly string DefaultUnauthorizedErrorPageResourceName = "unauthorized";

		public string OverrideRootApplicationUrl { get; protected set; }
		public string AuthenticationEndpointUrl { get; private set; }
		public string SignOnHandlerPath { get; private set; }
		public string SignOnReturnHandlerPath { get; private set; }
		public string SignOutHandlerPath { get; private set; }
		public string EmbeddedContentPath { get; private set; }
		public string DefaultSignOutSuccessPagePath { get; private set; }
		public string DefaultUnauthorizedErrorPagePath { get; private set; }

		public virtual string SignOutSuccessPagePath
		{
			get
			{
				return DefaultSignOutSuccessPagePath;
			}
		}

		public virtual string UnauthorizedErrorPagePath
		{
			get
			{
				return DefaultUnauthorizedErrorPagePath;
			}
		}

		public virtual string TransformReturnHandlerUrl(string returnHandlerUrl)
		{
			return returnHandlerUrl;
		}

		protected SsoAppHost(string authenticationProviderEndpoint)
		{
			AuthenticationEndpointUrl = authenticationProviderEndpoint;
		}

		public static SsoAppHost Instance { get; private set; }

		private void InitializePaths(string workingDirectory)
		{
            var appRelativeWorkingDirectory = AspNetUtility.GetAppRelativePathWithTilde(workingDirectory);

			SignOnHandlerPath = UrlHelper.Combine(appRelativeWorkingDirectory, SignOnHandlerResourceName);
			SignOnReturnHandlerPath = UrlHelper.Combine(appRelativeWorkingDirectory, SignOnReturnHandlerResourceName);
			SignOutHandlerPath = UrlHelper.Combine(appRelativeWorkingDirectory, SignOutHandlerResourceName);
			EmbeddedContentPath = UrlHelper.Combine(appRelativeWorkingDirectory, EmbeddedContentPathResourceName);
			DefaultSignOutSuccessPagePath = UrlHelper.Combine(appRelativeWorkingDirectory, DefaultSignOutSuccessPageResourceName);
			DefaultUnauthorizedErrorPagePath = UrlHelper.Combine(appRelativeWorkingDirectory, DefaultUnauthorizedErrorPageResourceName);
		}

		private static void AddIgnoreRoute(string workingDirectory)
		{
            var ignoreRoute = AspNetUtility.GetAppRelativePathWithoutTilde(workingDirectory) + "{*pathInfo}";
			RouteTable.Routes.Ignore(ignoreRoute);
		}

		public SsoAppHost Initialize()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				throw new SsoInitializationException("SsoAppHost.Instance has already been set");
			}

			var ssoDirectory = UrlHelper.GetPathOrUrlWithoutFileNameAndQuery(FormsAuthentication.LoginUrl);
			InitializePaths(ssoDirectory);
			AddIgnoreRoute(ssoDirectory);

			HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider());

			return this;
		}

		public abstract string GetRedirectToAuthenticationProviderUrl(HttpRequestBase request, string returnHandlerUrl);

		public abstract AuthenticationProviderUserInfo GetUserInformationFromAuthenticationProviderReturn(HttpRequestBase authenticationProviderReturnRequest);

		public virtual string GetApplicationReturnUrl(HttpRequestBase authenticationProviderReturnRequest)
		{
			if (authenticationProviderReturnRequest == null)
			{
				throw new ArgumentNullException("authenticationProviderReturnRequest");
			}

			var returnUrl = authenticationProviderReturnRequest.QueryString[QueryStringParameters.ReturnUrl];

			var redirectUrl = (!string.IsNullOrWhiteSpace(returnUrl))
				? returnUrl
				: "~/";

			return redirectUrl;
		}

		public void SignOnUser(HttpContextBase context, AuthenticationProviderUserInfo userInfo)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			if (userInfo == null)
			{
				throw new ArgumentNullException("userInfo");
			}

			var userData = new Dictionary<string, object>();
			userData[TicketUserDataKeys.Roles] = GetRolesForUser(userInfo);
			userData[TicketUserDataKeys.Profile] = GetProfileForUser(userInfo);

			var userDataJson = SerializationUtility.ToJson(userData);

			FormsAuthenticationTicketUtility.SetFormsAuthenticationTicket(context, userInfo.UserName, userDataJson);
		}

		public virtual void SignOutUser(HttpContextBase context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			FormsAuthentication.SignOut();
			FormsAuthenticationTicketUtility.ClearFormsAuthenticationTicket(context);

			if (context.Session != null)
			{
				context.Session.Abandon();
			}
		}

		public virtual string[] GetRolesForUser(AuthenticationProviderUserInfo userInfo)
		{
			return new string[] { };
		}

		public virtual Type DeserializeUserProfileAsType
		{
			get
			{
				return typeof(object);
			}
		}

		public virtual object GetProfileForUser(AuthenticationProviderUserInfo userInfo)
		{
			if (userInfo == null)
			{
				throw new ArgumentNullException("userInfo");
			}

			return new { UserName = userInfo.UserName };
		}

		public string[] GetCurrentUserRoles()
		{
			var ticket = FormsAuthenticationTicketUtility.GetFormsAuthenticationTicket();

			return (ticket != null)
				? GetCurrentUserRoles(ticket)
				: null;
		}

		public static string[] GetCurrentUserRoles(FormsAuthenticationTicket ticket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("ticket");
			}

			var userData = SerializationUtility.FromJson<IDictionary<string, object>>(ticket.UserData);
			var userDataArray = (userData[TicketUserDataKeys.Roles] as object[]) ?? new object[] { };
			var result = userDataArray.Cast<string>().ToArray();
			return result;
		}

		public object GetCurrentUserProfile()
		{
			var ticket = FormsAuthenticationTicketUtility.GetFormsAuthenticationTicket();

			return (ticket != null)
				? GetCurrentUserProfile(ticket, DeserializeUserProfileAsType)
				: null;
		}

		public T GetCurrentUserProfile<T>()
		{
			return (T)GetCurrentUserProfile();
		}

		public static object GetCurrentUserProfile(FormsAuthenticationTicket ticket, Type targetType)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("ticket");
			}

			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			var userData = SerializationUtility.FromJson<IDictionary<string, object>>(ticket.UserData);
			var dictionary = userData[TicketUserDataKeys.Profile] as IDictionary<string, object>;
			var result = SerializationUtility.FromDictionary(dictionary, targetType);
			return result;
		}

		protected string GetApplicationUrl(HttpRequestBase request, string path)
		{
			string result;

			if (!string.IsNullOrEmpty(OverrideRootApplicationUrl))
			{
				var pathWithoutTilde = AspNetUtility.GetAppRelativePathWithoutTilde(path);
				result = UrlHelper.Combine(OverrideRootApplicationUrl, pathWithoutTilde);
			}
			else
			{
				result = AspNetUtility.GetApplicationUrl(request, path);
			}

			return result;
		}

		private static class TicketUserDataKeys
		{
			public static readonly string Roles = "roles";
			public static readonly string Profile = "profile";
		}
	}
}
