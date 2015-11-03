using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IUserRepository _userRepository;
       
        public AccountController(ISessionManagementRepository sessionManagementRepository, IParentRepository parentRepository, IUserRepository userRepository)
        {
            _sessionManagementRepository = sessionManagementRepository;
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
                if (_sessionManagementRepository.LogIn(model.Email, model.Password))
                {
                    if (parent.User.IsUsingDefaultPassword)
                    {
                        return RedirectToAction("ChangePassword");
                    }
                    return parent.User.Email.Equals("") ? RedirectToAction("ConfirmEmail") : RedirectToAction("Index", "Home");
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

        [AllowAnonymous]
        public ActionResult ConfirmEmail()
        {
            return View("EmailConfirmation");
        }

        public ActionResult UpdateEmail(UpdateParentMailModel model)
        {
            var userId = Convert.ToInt64(_sessionManagementRepository.GetUserLoggedId());
            var parentUser = _parentRepository.Filter(x => x.User.Id == userId).Include(x => x.User).FirstOrDefault();
            
            if (parentUser != null)
            {
                var user = parentUser.User;
                user.Email = model.Email;
                _userRepository.Update(user);
                return RedirectToAction("Index", "Notification");
            }

            return RedirectToAction("ConfirmEmail");
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
            var userId = Convert.ToInt32(_sessionManagementRepository.GetUserLoggedId());
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
