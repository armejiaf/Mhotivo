using System.Data.Entity;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Mhotivo.Implement.Context;
using Mhotivo.Migrations;

namespace Mhotivo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var traceSource = new TraceSource("AppHarborTraceSource", SourceLevels.All);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Running application start");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AutoMapperConfiguration.Configure();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MhotivoContext, Configuration>());
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "initialized database");
        }
    }
}