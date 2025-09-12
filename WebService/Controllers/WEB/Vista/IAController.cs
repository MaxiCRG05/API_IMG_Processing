using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Scripts;

namespace WebService.Controllers.WEB
{
    public class IAController : Controller
    {
		[AutorizarRol("Usuario", "Admin")]
		public ActionResult Crear(int? proyectoID = null)
        {
			if (proyectoID == null)
				return RedirectToAction("Index", "Cliente");

			ViewBag.ProyectoID = proyectoID;
			return View();
		}
    }
}