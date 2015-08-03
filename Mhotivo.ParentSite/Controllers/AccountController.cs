using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Implement;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly IParentRepository _parentRepository;
       
        public AccountController(ISessionManagementRepository sessionManagementRepository, ISecurityRepository securityRepository, IParentRepository parentRepository)
        {
            _sessionManagementRepository = sessionManagementRepository;
            _securityRepository = securityRepository;
            _parentRepository = parentRepository;
            
        }
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }
        
            [HttpPost]
            [AllowAnonymous]
            public ActionResult LogIn(ParentLoginModel model, string returnUrl)
            {
                //System.Web.HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
                
               var parent = _parentRepository.Filter(y => y.User.Email == model.Email).FirstOrDefault();

                if (parent != null)
                {  
                    if (_sessionManagementRepository.LogIn(model.Email, model.Password))
                    {
                        return RedirectToAction("Index", "Notification");
                    }

                    ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
                    return View(model);
                }
                
                ModelState.AddModelError("", "El usuario ingresado no es un padre");
                return View(model);

                
            }

            public ActionResult LogOut()
            {
                _sessionManagementRepository.LogOut();
                return RedirectToAction("Index", "Home");
            }

            private ActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }



    }
}
