using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebService.Controllers.WEB
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registro()
        { 
            return View();
        }
        
        public ActionResult Recuperar()
        { 
            return View();
        }
    }
}