using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
		readonly Context db = new Context();

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

			ViewBag.Proyectos = proyectos;
			ViewBag.Categorias = db.Categorias.ToList();

			return View(proyectos);
		}

		[AutorizarRol("Usuario")]
		public ActionResult Proyecto(int? proyectoID = null)
		{
			if (proyectoID == null)
			{
				return RedirectToAction("Index", "Cliente");
			}

			if (Session["UsuarioID"] == null)
			{
				return RedirectToAction("Index", "Login");
			}

			int usuarioId = (int)Session["UsuarioID"];
			var proyecto = db.Proyectos
				.FirstOrDefault(p => p.ID == proyectoID && p.UsuarioID == usuarioId);

			if (proyecto == null)
			{
				return RedirectToAction("Index", "Cliente");
			}

			var categoriasProyecto = db.ProyectosCategorias
				.Where(pc => pc.ProyectoID == proyectoID)
				.Select(pc => pc.CategoriaID)
				.ToList();

			var objetos = db.Objetos
				.Where(o => categoriasProyecto.Contains(o.CategoriasID))
				.ToList();

			ViewBag.Objetos = objetos;

			return View(proyecto);
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
			ViewBag.ProyectoID = proyecto.ID;

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

		[HttpPost]
		[AutorizarRol("Usuario")]
		public JsonResult CrearProyecto(string nombreProyecto, List<int> categoriaIds)
		{
			try
			{
				if (Session["UsuarioID"] == null)
				{
					return Json(new { success = false, message = "Sesión expirada" });
				}

				if (string.IsNullOrEmpty(nombreProyecto) || nombreProyecto.Length < 3)
				{
					return Json(new { success = false, message = "El nombre debe tener al menos 3 caracteres" });
				}

				foreach (var catId in categoriaIds)
				{
					if (!db.Categorias.Any(c => c.ID == catId))
					{
						return Json(new { success = false, message = $"La categoría {catId} no existe" });
					}
				}

				int usuarioId = (int)Session["UsuarioID"];
				var usuario = db.Usuarios.Find(usuarioId);

				if (usuario == null)
				{
					return Json(new { success = false, message = "Usuario no encontrado" });
				}

				var proyecto = new Proyecto
				{
					UsuarioID = usuarioId,
					Nombre = nombreProyecto,
					FechaCreacion = DateTime.Now,
					FechaModificacion = DateTime.Now,
					Usuarios = usuario 
				};

				db.Proyectos.Add(proyecto);
				db.SaveChanges();

				var categoriasProyecto = categoriaIds.Select(catId => new ProyectosCategorias
				{
					ProyectoID = proyecto.ID,
					CategoriaID = catId
				}).ToList();

				db.ProyectosCategorias.AddRange(categoriasProyecto);
				db.SaveChanges();

				return Json(new { success = true, id = proyecto.ID, nombre = proyecto.Nombre });
			}
			catch (DbEntityValidationException ex)
			{
				var errorMessages = ex.EntityValidationErrors
					.SelectMany(x => x.ValidationErrors)
					.Select(x => x.ErrorMessage);

				var fullErrorMessage = string.Join("; ", errorMessages);
				return Json(new { success = false, message = "Error de validación: " + fullErrorMessage });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error al crear el proyecto: " + ex.Message });
			}
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