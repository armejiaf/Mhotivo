using System.Web.Mvc;
using System.Web.Routing;

namespace Mhotivo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute( // this route must be declared first, before the one below it
                 "GeneralEnrolls",
                 "Enrolls/Index/",
                 new
                 {
                     controller = "Enrolls",
                     action = "Index",
                 });

            routes.MapRoute(
                 "GeneralEnrollsFromAcademicGrades",
                 "Enrolls/Index/{gradeId}",
                 new
                 {
                     controller = "Enrolls",
                     action = "GeneralEnrollsFromAcademicGrades",
                     gradeId = UrlParameter.Optional
                 });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}