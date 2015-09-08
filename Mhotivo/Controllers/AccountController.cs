using System.Diagnostics;
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
        private readonly ISessionManagementRepository _sessionManagement;
        private readonly IParentRepository _parentRepository;

        public AccountController(ISessionManagementRepository sessionManagement, IParentRepository parentRepository)
        {
            _sessionManagement = sessionManagement;
            _parentRepository = parentRepository;
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
            var traceSource = new TraceSource("AppHarborTraceSource", SourceLevels.All);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Got into Login method");
            var parent = _parentRepository.Filter(y => y.MyUser.Email == model.UserEmail).FirstOrDefault();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "filtered parents");
            if (parent == null)
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "not a parent");
                if (_sessionManagement.LogIn(model.UserEmail, model.Password, model.RememberMe))
                {
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "logged in");
                    return RedirectToLocal(returnUrl);
                }
            }
            else
            {
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "was a parent");
                ModelState.AddModelError("", "El usurio no tiene privilegios para entrar a esta pagina");
                return View(model);
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "insufficient privileges");
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