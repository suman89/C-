using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FrescoPlayTest.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));
        }

        private ILog _logger
        {
            get
            {
                return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            Exception exception = Server.GetLastError();
            Response.Clear();

            _logger.ErrorFormat("Error occured. Inner Exception : {0}, Message : {1}, Stack Trace : {2}", exception.InnerException, exception.Message, exception.StackTrace);
            HttpException httpException = exception as HttpException;
            

            // clear error on server
            Server.ClearError();
            //Response.Redirect(string.Format("~/Error/{0}/?message={1}", action, exception.Message));
            Response.RedirectToRoute(new RouteValueDictionary { { "controller", "Error" }, { "action", "General" } });
        }
    }
}
