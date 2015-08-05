using System.Web.Mvc;
using System.Web.Routing;

namespace Mhotivo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AcademicYear_ChangeTeacher",
                url: "AcademicYear/ChangeTeacher/{id}/{teacherId}",
                defaults: new { controller = "AcademicYear", action = "ChangeTeacher" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}