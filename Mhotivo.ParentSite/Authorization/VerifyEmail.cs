using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.ParentSite.Authorization
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifyEmail : AuthorizeAttribute
    {
         protected override bool AuthorizeCore(HttpContextBase httpContext)
         {
             var sessionManagementService = ((ISessionManagementService)DependencyResolver.Current.GetService(typeof(ISessionManagementService)));    
             var role = HttpContext.Current.Session["loggedUserEmail"].ToString();
             var user = ((IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository))).GetById(Convert.ToInt64(sessionManagementService.GetUserLoggedId()));
             return (!user.IsUsingDefaultPassword) && !role.Equals("");
         }

         protected override void HandleUnauthorizedRequest(AuthorizationContext context)
         {
             var sessionManagementService = ((ISessionManagementService)DependencyResolver.Current.GetService(typeof(ISessionManagementService)));
             var user = ((IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository))).GetById(Convert.ToInt64(sessionManagementService.GetUserLoggedId()));
             var urlHelper = new UrlHelper(context.RequestContext);
             context.Result = user.IsUsingDefaultPassword ? new RedirectResult(urlHelper.Action("ChangePassword", "Account")) : new RedirectResult(urlHelper.Action("ConfirmEmail", "Account"));
         }
    }
}