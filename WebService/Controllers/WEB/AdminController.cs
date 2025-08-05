using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;

namespace WebService.Controllers.WEB
{
    public class AdminController : Controller
	{
		private Context db = new Context();

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
			var categorias = db.Categorias.ToList();
			ViewBag.Categorias = categorias;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Admin")]
		public ActionResult Agregar(
	string NombreObjeto,
	int CategoriaID,
	HttpPostedFileBase imagen,
	HttpPostedFileBase archivoHu)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(NombreObjeto))
				{
					ModelState.AddModelError("", "El nombre del objeto es obligatorio");
					ViewBag.Categorias = db.Categorias.ToList();
					return View();
				}

				if (imagen == null || imagen.ContentLength == 0)
				{
					ModelState.AddModelError("", "Debe subir una imagen del objeto");
					ViewBag.Categorias = db.Categorias.ToList();
					return View();
				}

				// Crear nuevo objeto
				var nuevoObjeto = new Objeto
				{
					Nombre = NombreObjeto,
					CategoriasID = CategoriaID,
					Imagen = ConvertToBytes(imagen)
				};

				db.Objetos.Add(nuevoObjeto);
				db.SaveChanges();

				// Procesar invariantes Hu si se subieron
				if (archivoHu != null && archivoHu.ContentLength > 0)
				{
					GuardarInvariantesHu(nuevoObjeto.ID, archivoHu);
				}

				TempData["SuccessMessage"] = "Objeto agregado exitosamente!";
				return RedirectToAction("Agregar");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Error: " + ex.Message);
				ViewBag.Categorias = db.Categorias.ToList();
				return View();
			}
		}

		private byte[] ConvertToBytes(HttpPostedFileBase file)
		{
			using (BinaryReader reader = new BinaryReader(file.InputStream))
			{
				return reader.ReadBytes(file.ContentLength);
			}
		}

		private void GuardarInvariantesHu(int objetoId, HttpPostedFileBase archivoHu)
		{
			using (var reader = new StreamReader(archivoHu.InputStream))
			{
				string content = reader.ReadToEnd();
				var valores = content.Split(',').Select(double.Parse).ToArray();

				if (valores.Length != 7) return;

				var invariantes = new InvariantesHu
				{
					ObjetoID = objetoId,
					Hu1 = valores[0],
					Hu2 = valores[1],
					Hu3 = valores[2],
					Hu4 = valores[3],
					Hu5 = valores[4],
					Hu6 = valores[5],
					Hu7 = valores[6]
				};

				db.InvariantesHu.Add(invariantes);
				db.SaveChanges();
			}
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

		[HttpPost]
		[AutorizarRol("Admin")]
		public JsonResult AgregarCategoria(string nombreCategoria)
		{
			try
			{
				if (db.Categorias.Any(c => c.Nombre == nombreCategoria))
				{
					return Json(new
					{
						success = false,
						message = "¡La categoría ya existe!"
					});
				}

				var nuevaCategoria = new Categoria { Nombre = nombreCategoria };
				db.Categorias.Add(nuevaCategoria);
				db.SaveChanges();

				return Json(new
				{
					success = true,
					id = nuevaCategoria.ID,
					nombre = nuevaCategoria.Nombre
				});
			}
			catch (System.Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}


		}
		protected override void Dispose(bool disposing)
		{
			if (disposing) db.Dispose();
			base.Dispose(disposing);
		}
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
			var usuario = httpContext.Session["Rol"].ToString();
			return usuario != null && _rolesPermitidos.Contains(usuario);
		}
	}
}