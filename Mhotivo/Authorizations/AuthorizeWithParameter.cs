using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Util;

namespace Mhotivo.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeWithParameter : AuthorizeAttribute
    {
        private readonly List<string> _requireAtLeastOnePrivileges;

        public AuthorizeWithParameter(string privilegeString)
        {
            var privileges = privilegeString.Split(',');
            this._requireAtLeastOnePrivileges = privileges
                .Select(privilege => privilege.Trim()).ToList();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var roleRepository =
                ((IRoleRepository) DependencyResolver.Current.GetService(typeof (IRoleRepository)));

            var roleName = (string)HttpContext.Current.Session["loggedUserRole"];
            var role = roleRepository.Filter(r => r.Name == roleName).FirstOrDefault(r => true);
            var sessionManagementService = ((ISessionManagementService)DependencyResolver.Current.GetService(typeof(ISessionManagementService)));
            var user = ((IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository))).GetById(Convert.ToInt64(sessionManagementService.GetUserLoggedId()));
            return (!user.IsUsingDefaultPassword) && role != null && role.HasAnyPrivilege(_requireAtLeastOnePrivileges);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            var sessionManagementService = ((ISessionManagementService)DependencyResolver.Current.GetService(typeof(ISessionManagementService)));
            var user = ((IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository))).GetById(Convert.ToInt64(sessionManagementService.GetUserLoggedId()));
            var urlHelper = new UrlHelper(context.RequestContext);
            context.Result = user.IsUsingDefaultPassword ? new RedirectResult(urlHelper.Action("ChangePassword", "Account")) : new RedirectResult(urlHelper.Action("Index", "Home"));
        }
    }
}