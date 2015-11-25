using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.ParentSite.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ISecurityService _securityService;

        public HomeController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserLoggedName()
        {
            var userName = _securityService.GetUserLoggedName();
            return Content(userName);
        }
    }
}
