using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mhotivo.ParentSite.Authorization
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifyEmail : AuthorizeAttribute
    {
         protected override bool AuthorizeCore(HttpContextBase httpContext)
         {
             var role = HttpContext.Current.Session["loggedUserEmail"].ToString();

             return (!role.Equals(""));
         }

         protected override void HandleUnauthorizedRequest(AuthorizationContext context)
         {
             var urlHelper = new UrlHelper(context.RequestContext);
             context.Result = new RedirectResult(urlHelper.Action("EmailConfirmation", "Account"));
         }
    }
}