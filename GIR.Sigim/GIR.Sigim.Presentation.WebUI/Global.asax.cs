using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Presentation.WebUI.IoC;

namespace GIR.Sigim.Presentation.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialise();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            AuthenticationServiceFactory.Create().PostAuthenticateRequest();
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (!context.User.Identity.IsAuthenticated && context.Request.IsAjaxRequest())
            {
                context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    }
}