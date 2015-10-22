using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private readonly ISessionManagement _sessionManagement;
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(ISessionManagementRepository sessionManagementRepository, IParentRepository parentRepository, IUserRepository userRepository)
        {
            _sessionManagementRepository = sessionManagementRepository;
            _parentRepository = parentRepository;
            _userRepository = userRepository;
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
            var parent = _parentRepository.Filter(y => y.MyUser.Email == model.UserEmail).FirstOrDefault();
            if (parent == null)
            {
                if (_sessionManagementRepository.LogIn(model.UserEmail, model.Password, model.RememberMe))
                {
                    var user = _userRepository.FirstOrDefault(x => x.Email == model.UserEmail);
                    return user.IsUsingDefaultPassword ? RedirectToAction("ChangePassword") : RedirectToLocal(returnUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "El usurio no tiene privilegios para entrar a esta pagina");
                return View(model);
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout(string returnUrl)
        {
            _sessionManagementRepository.LogOut();
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            _sessionManagementRepository.LogOut();
            return RedirectToAction("Index", "Home");
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
            user.EncryptPassword();
            user.DefaultPassword = null;
            user.IsUsingDefaultPassword = false;
            _userRepository.Update(user);
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