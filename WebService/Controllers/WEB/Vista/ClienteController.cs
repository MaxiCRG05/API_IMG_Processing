using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;

namespace WebService.Controllers.WEB
{
    public class ClienteController : Controller
    {
		Context db = new Context();

		// GET: Cliente
		[AutorizarRol("Usuario")]
		public ActionResult Index()
		{
			if (Session["Rol"].ToString() == "Admin")
				return RedirectToAction("Index", "Admin");

			if (Session["UsuarioID"] == null || Session["Nombre"] == null)
				return RedirectToAction("Index", "Login");

			int usuarioId = (int)Session["UsuarioID"];

			var proyectos = db.Proyectos
				.Where(p => p.UsuarioID == usuarioId)
				.OrderByDescending(p => p.FechaModificacion)
				.ToList();

			ViewBag.NombreUsuario = Session["Nombre"].ToString().ToUpper();

			return View(proyectos);
		}

		[AutorizarRol("Usuario")]
        public ActionResult Proyecto()
        {
			return View();
		}

		[HttpPost]
		[AutorizarRol("Usuario")]
		[ValidateAntiForgeryToken]
		public ActionResult Proyecto(Proyecto proyecto)
		{
			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			if (ModelState.IsValid)
			{
				proyecto.UsuarioID = (int)Session["UsuarioID"];
				proyecto.FechaCreacion = DateTime.Now;
				proyecto.FechaModificacion = DateTime.Now;

				db.Proyectos.Add(proyecto);
				db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(proyecto);
		}

		// GET: Cliente/EditarProyecto/5
		[AutorizarRol("Usuario")]
		public ActionResult EditarProyecto(int id)
		{
			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			int usuarioId = (int)Session["UsuarioID"];
			var proyecto = db.Proyectos.FirstOrDefault(p => p.ID == id && p.UsuarioID == usuarioId);

			if (proyecto == null)
			{
				return HttpNotFound();
			}

			return View(proyecto);
		}

		[HttpPost]
		[AutorizarRol("Usuario")]
		[ValidateAntiForgeryToken]
		public ActionResult EditarProyecto(Proyecto proyecto)
		{
			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			if (ModelState.IsValid)
			{
				var proyectoExistente = db.Proyectos.Find(proyecto.ID);
				if (proyectoExistente != null && proyectoExistente.UsuarioID == (int)Session["UsuarioID"])
				{
					proyectoExistente.Nombre = proyecto.Nombre;
					proyectoExistente.FechaModificacion = DateTime.Now;

					db.Entry(proyectoExistente).State = EntityState.Modified;
					db.SaveChanges();

					return RedirectToAction("Index");
				}
			}

			return View(proyecto);
		}

		[AutorizarRol("Usuario")]
		public ActionResult CrearProyecto()
		{
			ViewBag.Categorias = db.Categorias.ToList();
			return View();
		}

		[HttpPost]
		[AutorizarRol("Usuario")]
		[ValidateAntiForgeryToken]
		public ActionResult CrearProyecto(Proyecto proyecto, int[] categoriasSeleccionadas)
		{
			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			if (ModelState.IsValid)
			{
				proyecto.UsuarioID = (int)Session["UsuarioID"];
				proyecto.FechaCreacion = DateTime.Now;
				proyecto.FechaModificacion = DateTime.Now;

				if (categoriasSeleccionadas != null)
				{
					foreach (var categoriaId in categoriasSeleccionadas)
					{
						var categoria = db.Categorias.Find(categoriaId);
						if (categoria != null)
						{
							proyecto.Categorias.Add(categoria);
						}
					}
				}

				db.Proyectos.Add(proyecto);
				db.SaveChanges();

				TempData["Alerta"] = "Proyecto creado exitosamente!";
				return RedirectToAction("Index");
			}

			ViewBag.Categorias = db.Categorias.ToList();
			return View(proyecto);
		}

		[HttpPost]
		[AutorizarRol("Usuario")]
		[ValidateAntiForgeryToken]
		public ActionResult EliminarProyecto(int id)
		{
			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			var proyecto = db.Proyectos.Find(id);
			if (proyecto != null && proyecto.UsuarioID == (int)Session["UsuarioID"])
			{
				db.Proyectos.Remove(proyecto);
				db.SaveChanges();
			}

			return RedirectToAction("Index");
		}
	}
}