﻿using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private readonly ISessionManagement _sessionManagement;
        private readonly ISessionManagementRepository _sessionManagement;

        public AccountController(ISessionManagementRepository sessionManagement)
        {
            _sessionManagement = sessionManagement;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {


            if (_sessionManagement.LogIn(model.UserEmail, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout(string returnUrl)
        {
            _sessionManagement.LogOut();

            return RedirectToAction("Index", "Home");
        }


        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            _sessionManagement.LogOut();

            return RedirectToAction("Index", "Home");
        }

        #region Aplicaciones auxiliares

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}