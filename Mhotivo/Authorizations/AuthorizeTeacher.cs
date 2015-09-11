using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Controllers;

namespace Mhotivo.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeTeacher : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var role = (string)HttpContext.Current.Session["loggedUserRole"];
            return (!String.IsNullOrEmpty(role) && (role.Equals("Maestro") || role.Equals("Director")));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            context.Result = new RedirectResult(urlHelper.Action("Index", "Home"));
        }
    }
}