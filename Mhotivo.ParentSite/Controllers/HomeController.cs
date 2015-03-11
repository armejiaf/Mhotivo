using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPeopleRepository _peopleRepository;
        private IUserRepository _userRepository;

        public HomeController(ISessionManagementRepository sessionManagementRepository,IRoleRepository roleRepository, IPeopleRepository peopleRepository,IUserRepository userRepository)
        {
            _sessionManagementRepository = sessionManagementRepository;
            _roleRepository = roleRepository;
            _peopleRepository = peopleRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            return View();
            
        }

        public ActionResult Redirect()
        {
            return RedirectToAction("Index", "Notification");  
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return Redirect(returnUrl);
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(ParentLoginModel model, string returnUrl)
        {
            if (_sessionManagementRepository.LogIn(model.Email, model.Password))
            {
                var userLooged = _userRepository.Filter(usr => usr.Email.Equals(model.Email)).FirstOrDefault();

                var people = _peopleRepository.Filter(p => p.User.Id== userLooged.Id).FirstOrDefault();

                if (people!= null && people.Discriminator == "Parent")
                {
                    return RedirectToAction("Index", "Notification");  
                }
                ModelState.AddModelError("Forbbiden", "El usuario no es un padre");
                return Login(returnUrl);

            }
            ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
            return Redirect(); //RedirectToAction("Login", "Home");
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
