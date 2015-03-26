using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using jaytwo.AspNet.SingleSignOn.Utilities;
using System.Web.Script.Serialization;
using System.Web;

namespace jaytwo.AspNet.SingleSignOn.Security
{
    public class SsoAppHostRoleProvider : RoleProvider
    {
        protected SsoAppHost AppHost { get; private set; }

        public SsoAppHostRoleProvider()
            : this(SsoAppHost.Instance)
        {
        }

        public SsoAppHostRoleProvider(SsoAppHost ssoAppHost)
        {
            AppHost = ssoAppHost;
        }

        public override string[] GetRolesForUser(string username)
        {
            if (username != HttpContext.Current.User.Identity.Name)
            {
                throw new NotSupportedException("SsoAppHostRoleProvider.GetRolesForUser only supports current FormsAuthentication user");
            }

            return SsoAppHost.Instance.GetCurrentUserRoles();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var result = false;

            var roles = GetRolesForUser(username);

            if (roles != null)
            {
                result = roles.Contains(roleName, StringComparer.InvariantCultureIgnoreCase);
            }

            return result;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
