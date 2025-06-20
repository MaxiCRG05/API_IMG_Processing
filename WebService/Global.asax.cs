using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			FirebaseApp.Create(new AppOptions()
			{
				Credential = GoogleCredential.FromFile(Server.MapPath("~/firebase-config.json")),
			});

			AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
    }
}
