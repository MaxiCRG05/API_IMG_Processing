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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Proyecto()
        {
			return View();
		}
    }
}