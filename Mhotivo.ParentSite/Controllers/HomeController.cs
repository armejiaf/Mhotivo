using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly IParentRepository _parentRepository;

        public HomeController(ISessionManagementRepository sessionManagementRepository, IParentRepository parentRepository)
        {
            _sessionManagementRepository = sessionManagementRepository;
          
            _parentRepository = parentRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

            
        // GET: /Account/Login

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(string email,string password)
        {
            if (_sessionManagementRepository.LogIn(email, password))
            {
                var parent = _parentRepository.Filter(y => y.User.Email == email);

                if (parent.Any())
                {
                    Session["Email"] = email;
                    return RedirectToAction("Index", "Notification");
                }
                
                    ModelState.AddModelError("Forbbiden", "El usuario no es un padre");
                    return Index();
                
            }
            
                ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
                return Index();    
            
        }

    }
}
