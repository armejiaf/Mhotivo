using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Mhotivo.Implement.Context;
using Mhotivo.Binder;
using System;

namespace Mhotivo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();
            var binder = new DateTimeModelBinder("MM/dd/yyyy");
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MhotivoContext, Implement.Migrations.Configuration>());
            using (var context = new MhotivoContext())
            {
                context.Database.Initialize(true);
            } 
        }       
    }
}