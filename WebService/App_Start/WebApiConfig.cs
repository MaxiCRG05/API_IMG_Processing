using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "ProcesamientoApi",
				routeTemplate: "api/Procesamiento/{action}/{id}",
				defaults: new { controller = "Procesamiento", id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "PML",
				routeTemplate: "api/PML/{action}/{id}",
				defaults: new { controller = "PML",id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
    }
}

