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
            return View();
        }

        public ActionResult Agregar()
		{
			return View();
		}
        
        public ActionResult Modificar()
		{
			return View();
		}
	}
}