using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebService.Controllers.WEB
{
    public class ClienteController : Controller
    {
		// GET: Cliente
		[AutorizarRol("Usuario")]
		public ActionResult Index()
        {
            return View();
        }

		[AutorizarRol("Usuario")]
        public ActionResult Proyecto()
        {
			return View();
		}

		public class AutorizarRol : AuthorizeAttribute
		{
			private readonly string[] _rolesPermitidos;

			public AutorizarRol(params string[] roles)
			{
				_rolesPermitidos = roles;
			}

			protected override bool AuthorizeCore(HttpContextBase httpContext)
			{
				var usuario = httpContext.Session["Rol"] as string;
				return usuario != null && _rolesPermitidos.Contains(usuario);
			}
		}
	}
}