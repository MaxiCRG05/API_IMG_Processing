using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebService.Controllers.WEB
{
    public class IAController : Controller
    {
        // GET: IA
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crear()
        {
			return View();
		}
    }
}