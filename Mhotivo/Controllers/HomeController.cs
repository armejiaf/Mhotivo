using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;

namespace Mhotivo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly ViewMessageLogic _viewMessageLogic;

        public HomeController(ISecurityService securityService)
        {
            _securityService = securityService;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        [AuthorizeNewUser]
        public ActionResult Index()
        {
            ViewBag.Message = "Modifique esta plantilla para poner en marcha su aplicación ASP.NET MVC.";
            _securityService.GetUserLoggedPeoples();
            _viewMessageLogic.SetViewMessageIfExist();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Página de descripción de la aplicación.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Página de contacto.";
            return View();
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }

        //[HttpGet]
        //[ChildActionOnly]
        public ActionResult GetUserLoggedName()
        {
            var userName = _securityService.GetUserLoggedName();
            return Content(userName);
        }
    }
}