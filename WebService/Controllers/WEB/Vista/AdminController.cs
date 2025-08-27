using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;

namespace WebService.Controllers.WEB
{
	public class AdminController : Controller
	{
		private readonly Context db = new Context();

		// GET: Admin
		[AutorizarRol("Admin")]
		public ActionResult Index()
		{
			Validar();
			ResetearTempData();
			return View();
		}

		[AutorizarRol("Admin")]
		public ActionResult Consultar()
		{
			Validar();
			
			ViewBag.Categorias = ObtenerCategorias();
			ViewBag.Objetos = ObtenerObjetos();
			ViewBag.InvariantesHu = ObtenerInvariantesHu();

			return View();
		}

		[AutorizarRol("Admin")]
		public ActionResult Agregar()
		{
			Validar();
			ViewBag.Categorias = ObtenerCategorias();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Admin")]
		public ActionResult Agregar(string NombreObjeto, int CategoriaID, HttpPostedFileBase archivoImagen, HttpPostedFileBase archivoHu)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(NombreObjeto))
				{
					ModelState.AddModelError("", "El nombre del objeto es obligatorio");
					ViewBag.Categorias = ObtenerCategorias();
					TempData["Error"] = "Error";
					return View();
				}

				if (archivoImagen == null || archivoImagen.ContentLength == 0)
				{
					ModelState.AddModelError("", "Debe subir una Imagen del objeto");
					ViewBag.Categorias = ObtenerCategorias();
					TempData["Error"] = "Error";
					return View();
				}

				var nuevoObjeto = new Objeto
				{
					Nombre = NombreObjeto,
					CategoriasID = CategoriaID,
					Imagen = MetodosProcesamiento.ConvertirABytes(archivoImagen)
				};

				db.Objetos.Add(nuevoObjeto);
				db.SaveChanges();

				if (archivoHu != null && archivoHu.ContentLength > 0)
				{
					GuardarInvariantesHu(nuevoObjeto.ID, archivoHu);
				}

				TempData["Success"] = "Objeto creado correctamente";
				return RedirectToAction("Agregar");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al crear el objeto: " + ex.Message;
				ViewBag.Categorias = ObtenerCategorias();
				return View();
			}
		}

		private void GuardarInvariantesHu(int objetoId, HttpPostedFileBase archivoHu)
		{
			using (var reader = new StreamReader(archivoHu.InputStream))
			{
				string content = reader.ReadToEnd();
				var lineas = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var linea in lineas)
				{
					var valores = linea.Trim().Split(',');
					if (valores.Length != 7) continue; 

					db.InvariantesHu.Add(new InvariantesHu
					{
						ObjetoID = objetoId,
						Hu1 = double.Parse(valores[0]),
						Hu2 = double.Parse(valores[1]),
						Hu3 = double.Parse(valores[2]),
						Hu4 = double.Parse(valores[3]),
						Hu5 = double.Parse(valores[4]),
						Hu6 = double.Parse(valores[5]),
						Hu7 = double.Parse(valores[6])
					});
				}
				db.SaveChanges(); 
			}
		}

		[AutorizarRol("Admin")]
		public ActionResult Modificar(int ObjetoID = 0)
		{
			Validar();
			ViewBag.Objetos = ObtenerObjetos();
			ViewBag.Categorias = ObtenerCategorias();

			if (ObjetoID != 0)
			{
				var objeto = db.Objetos.FirstOrDefault(o => o.ID == ObjetoID);

				if (objeto != null)
				{
					ViewBag.Objeto = new Objeto
					{
						ID = objeto.ID,
						Nombre = objeto.Nombre,
						CategoriasID = objeto.CategoriasID,
						Imagen = objeto.Imagen
					};
				}
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Admin")]
		public ActionResult Modificar(int ObjetosID, string NombreObjeto, int CategoriaID, HttpPostedFileBase archivoImagen, HttpPostedFileBase archivoHu)
		{
			try
			{
				var objeto = db.Objetos.Find(ObjetosID);
				if (objeto == null)
				{
					TempData["Error"] = "Objeto no encontrado";
					return RedirectToAction("Modificar");
				}

				objeto.Nombre = NombreObjeto;
				objeto.CategoriasID = CategoriaID;

				if (archivoImagen != null && archivoImagen.ContentLength > 0)
				{
					objeto.Imagen = MetodosProcesamiento.ConvertirABytes(archivoImagen);
				}

				db.Entry(objeto).State = EntityState.Modified;
				db.SaveChanges();

				if (archivoHu != null && archivoHu.ContentLength > 0)
				{
					ActualizarInvariantesHu(objeto.ID, archivoHu);
				}

				TempData["Success"] = "Objeto modificado correctamente";
				return RedirectToAction("Modificar");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al modificar el objeto: " + ex.Message;
				return RedirectToAction("Modificar");
			}
		}

		[AutorizarRol("Admin")]
		public ActionResult Eliminar()
		{
			Validar();
			ViewBag.Objetos = ObtenerObjetos();
			ViewBag.Categorias = ObtenerCategorias();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Admin")]
		public ActionResult Eliminar(int? ObjetosID, int? CategoriasID, bool BorrarCat = false)
		{
			try
			{
				if (ObjetosID.HasValue && CategoriasID.HasValue && !BorrarCat)
				{
					var objeto = db.Objetos.Find(ObjetosID.Value);
					var categoria = db.Categorias.Find(CategoriasID.Value);

					if (objeto == null || categoria == null)
					{
						TempData["Error"] = "Objeto o categoría no encontrados";
						return RedirectToAction("Eliminar");
					}

					var hus = db.InvariantesHu.Where(h => h.ObjetoID == objeto.ID).ToList();
					db.InvariantesHu.RemoveRange(hus);

					db.Objetos.Remove(objeto);

					if (db.Objetos.Any(o => o.CategoriasID == categoria.ID && o.ID != objeto.ID))
					{
						TempData["Error"] = "La categoría tiene otros objetos asociados";
						return RedirectToAction("Eliminar");
					}

					db.Categorias.Remove(categoria);
					db.SaveChanges();

					TempData["Success"] = "Objeto y categoría eliminados correctamente";
					return RedirectToAction("Eliminar");
				}

				if (!ObjetosID.HasValue && CategoriasID.HasValue)
				{
					var categoria = db.Categorias.Find(CategoriasID.Value);
					if (categoria == null)
					{
						TempData["Error"] = "Categoría no encontrada";
						return RedirectToAction("Eliminar");
					}

					if (db.Objetos.Any(o => o.CategoriasID == categoria.ID))
					{
						TempData["Error"] = "La categoría tiene objetos asociados";
						return RedirectToAction("Eliminar");
					}

					db.Categorias.Remove(categoria);
					db.SaveChanges();

					TempData["Success"] = "Categoría eliminada correctamente";
					return RedirectToAction("Eliminar");
				}

				if (ObjetosID.HasValue && !BorrarCat)
				{
					var objeto = db.Objetos.Find(ObjetosID.Value);
					if (objeto == null)
					{
						TempData["Error"] = "Objeto no encontrado";
						return RedirectToAction("Eliminar");
					}

					var hu = db.InvariantesHu.FirstOrDefault(h => h.ObjetoID == objeto.ID);
					if (hu != null) db.InvariantesHu.Remove(hu);

					db.Objetos.Remove(objeto);
					db.SaveChanges();

					TempData["Success"] = "Objeto eliminado correctamente";
					return RedirectToAction("Eliminar");
				}

				

				TempData["Error"] = "Selección inválida";
				return RedirectToAction("Eliminar");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error: " + ex.Message;
				return RedirectToAction("Eliminar");
			}
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

		private void ActualizarInvariantesHu(int objetoId, HttpPostedFileBase archivoHu)
		{
			var existentes = db.InvariantesHu.Where(h => h.ObjetoID == objetoId).ToList();
			db.InvariantesHu.RemoveRange(existentes);

			using (var reader = new StreamReader(archivoHu.InputStream))
			{
				string content = reader.ReadToEnd();
				var lineas = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var linea in lineas)
				{
					var valores = linea.Trim().Split(',');
					if (valores.Length != 7) continue;

					db.InvariantesHu.Add(new InvariantesHu
					{
						ObjetoID = objetoId,
						Hu1 = double.Parse(valores[0]),
						Hu2 = double.Parse(valores[1]),
						Hu3 = double.Parse(valores[2]),
						Hu4 = double.Parse(valores[3]),
						Hu5 = double.Parse(valores[4]),
						Hu6 = double.Parse(valores[5]),
						Hu7 = double.Parse(valores[6])
					});
				}
				db.SaveChanges();
			}
		}

		[AutorizarRol("Admin")]
		public JsonResult ObtenerObjeto(int objetoId)
		{
			var objeto = db.Objetos.Find(objetoId);
			if (objeto == null) return Json(null);

			return Json(new
			{
				objeto.Nombre,
				objeto.CategoriasID,
				ImagenBase64 = objeto.Imagen != null ?
					Convert.ToBase64String(objeto.Imagen) : null,
				TipoImagen = "image/png" 
			}, JsonRequestBehavior.AllowGet);
		}

		private ActionResult Validar()
		{
			if (Session["UsuarioID"] == null || Session["Rol"].ToString() != "Admin")
			{
				return RedirectToAction("Index", "Login");
			}
			return null;
		}

		private void ResetearTempData()
		{
			TempData["Success"] = null;
			TempData["Error"] = null;
		}

		private List<Categoria> ObtenerCategorias()
		{
			return db.Categorias.ToList();
		}

		private List<Objeto> ObtenerObjetos()
		{
			return db.Objetos.ToList();   
		}

		private List<InvariantesHu> ObtenerInvariantesHu()
		{
			return db.InvariantesHu.ToList();
		}

		[AutorizarRol("Admin")]
		public ActionResult DescargarHu(int id)
		{
			using (var db = new Context())
			{
				var archivo = MetodosProcesamiento.CrearArchivoHu(id, db);
				if (archivo == null)
				{
					TempData["Error"] = "No se encontraron invariantes de Hu";
					return RedirectToAction("Consultar");
				}
				return archivo;
			}
		}
	}
}