using PhotoGallery;
using PhotoGallery.AppCommonCore;
using PhotoGallery.Common;
using PhotoGallery.IoC;
using PhotoGallery.Logging;
using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MvcApplication), "Start")]
[assembly: ApplicationShutdownMethod(typeof(MvcApplication), "End")]

namespace PhotoGallery
{
    public class MvcApplication : HttpApplication
    {
        private static ICustomLogger _logger;

        protected void Application_Start()
        {
            InitCustomStuff();

            _logger.DebugInfo("Register Areas");
            AreaRegistration.RegisterAllAreas();
            _logger.DebugInfo("Register Global Filters");
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            _logger.DebugInfo("Register Routes");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            _logger.DebugInfo("Register Bundles");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void Start()
        {
            IoCInitializer iocInit = new IoCInitializer();
            iocInit.InitIoc();  
        }

        private static void End()
        {
            IoCInitializer.StructureMapDependencyScope.Dispose();
        }

        private void InitCustomStuff()
        {
            ConfigurationElements.InitConfig();

            AutoMapperInitiazlier autoMapperInit = new AutoMapperInitiazlier();
            autoMapperInit.InitMapper();

            _logger = IoCContainer.GetInstace<ICustomLogger>();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, CustomContextMigrationConfig>());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            _logger.Exception(exception,"Unexpected error occured");

            Server.ClearError();
            
            Response.Redirect("/Home/Error");
        }
    }
}
