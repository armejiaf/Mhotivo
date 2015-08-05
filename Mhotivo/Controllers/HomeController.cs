using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;

namespace Mhotivo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionManagementRepository _sessionManagement;
        private readonly ISecurityRepository _securityRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public HomeController(ISessionManagementRepository sessionManagement, ISecurityRepository securityRepository)
        {
            _sessionManagement = sessionManagement;
            _securityRepository = securityRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modifique esta plantilla para poner en marcha su aplicación ASP.NET MVC.";
            _securityRepository.GetUserLoggedPeoples();
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

        [HttpGet]
        [ChildActionOnly]
        public ActionResult GetUserLoggedName()
        {
            var userName = _securityRepository.GetUserLoggedName();
            return Content(userName);
        }
    }
}