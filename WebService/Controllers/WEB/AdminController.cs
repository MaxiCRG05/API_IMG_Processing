using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebService.Controllers.WEB
{
    public class AdminController : Controller
    {
		// GET: Admin
		public ActionResult Index()
		{
			if (Session["UsuarioID"] == null || Session["Rol"].ToString() != "Admin")
			{
				return RedirectToAction("Index", "Login");
			}
			return View();
		}

		[AutorizarRol("Admin")]
		public ActionResult Agregar()
		{
			return View();
		}

		[AutorizarRol("Admin")]
		public ActionResult Modificar()
		{
			return View();
		}

		[AutorizarRol("Admin")]
		public ActionResult Eliminar()
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