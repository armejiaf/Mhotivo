using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISessionManagementService _sessionManagementService;
        private readonly IParentRepository _parentRepository;
        private readonly IUserRepository _userRepository;
       
        public AccountController(ISessionManagementService sessionManagementService, IParentRepository parentRepository, IUserRepository userRepository)
        {
            _sessionManagementService = sessionManagementService;
            _parentRepository = parentRepository;
            _userRepository = userRepository;
        }

        // GET: /Account/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(ParentLoginModel model, string returnUrl)
        {
            var parent = model.Email.Contains("@") ? _parentRepository.Filter(y => y.User.Email == model.Email).FirstOrDefault()
                : _parentRepository.Filter(y => y.IdNumber == model.Email).FirstOrDefault();

            if (parent != null)
            {
                if (_sessionManagementService.LogIn(model.Email, model.Password))
                {
                    if (parent.User.IsUsingDefaultPassword)
                    {
                        return RedirectToAction("ChangePassword");
                    }
                    return parent.User.Email.Equals("") ? RedirectToAction("EmailConfirmation") : RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
                return View(model);
            }
            ModelState.AddModelError("", "El usuario ingresado no es un padre");
            return View(model);
        }

        public ActionResult LogOut()
        {
            _sessionManagementService.LogOut();
            return RedirectToAction("Index", "Home");
        }

 
        public ActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmailConfirmation(EmailConfirmationModel model)
        {
            var userId = Convert.ToInt64(_sessionManagementService.GetUserLoggedId());
            var parentUser = _parentRepository.Filter(x => x.User.Id == userId).FirstOrDefault();
            
            if (parentUser != null)
            {
                var user = parentUser.User;
                user.Email = model.Email;
                _userRepository.Update(user);
                return RedirectToAction("Index", "Notification");
            }

            return RedirectToAction("EmailConfirmation");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View();
            var userId = Convert.ToInt32(_sessionManagementService.GetUserLoggedId());
            var user = _userRepository.GetById(userId);
            user.Password = model.NewPassword;
            user.HashPassword();
            user.DefaultPassword = null;
            user.IsUsingDefaultPassword = false;
            _userRepository.Update(user);
            return RedirectToAction("Index", "Home");
        }

      
    }

    
}
