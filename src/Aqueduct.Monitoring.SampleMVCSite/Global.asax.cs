using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Aqueduct.Monitoring.MVC;
using Aqueduct.Monitoring;
using Aqueduct.Web;
using Aqueduct.Web.Configuration;
using Aqueduct.Diagnostics;
using Aqueduct.Monitoring.Subscribers;

namespace Aqueduct.Monitoring.SampleMVCSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            MVCNotificationProcessor.Initialise(GlobalFilters.Filters);
            LoggingSubscriber.Subscribe();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            WebApplicationInitialiser.Setup(this, initActionList =>
            {
                initActionList.Add(new DefaultConfigInitialiser());
                initActionList.AddOnFirstRequestAction(InitialiseLog4Net);
            });
        }

        private static void InitialiseLog4Net(HttpApplication application)
        {
            string machineName = application.Server.MachineName;
            if (!string.IsNullOrEmpty(machineName))
            {
                AppLogger.AddGlobalCustomParameter("server", machineName);
            }
            AppLogger.InitialiseFromConfig("DEFAULT");
            AppLogger.LogDebugMessage("log4Net has been initialised.");
        }

        protected void Application_End()
        {
            MVCNotificationProcessor.Shutdown();
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            MVCNotificationProcessor.HandleError(Server.GetLastError());
        }

        
    }
}