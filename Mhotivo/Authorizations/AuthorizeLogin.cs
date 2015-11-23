using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Implement.Repositories;

namespace Mhotivo.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeLogin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var roleRepository =
                ((RoleRepository)DependencyResolver.Current.GetService(typeof(RoleRepository)));

            var roleName = (string)HttpContext.Current.Session["loggedUserRole"];
            var role = roleRepository.Filter(r => r.Name == roleName).FirstOrDefault(r => true);

            return role != null && (role.Name.Equals("Maestro") || role.Name.Equals("Director"));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            context.Result = new RedirectResult(urlHelper.Action("Index", "Home"));
        }
    }
}