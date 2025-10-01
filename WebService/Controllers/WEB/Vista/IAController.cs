using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;

namespace WebService.Controllers.WEB
{
    public class IAController : Controller
    {
		readonly Context db = new Context();

		public ActionResult Crear(int? proyectoID = null)
		{
			if (proyectoID == null)
				return RedirectToAction("Index", "Cliente");

			if (Session["UsuarioID"] == null)
				return RedirectToAction("Index", "Login");

			int usuarioId = (int)Session["UsuarioID"];

			var redExistente = db.RedesNeuronales
				.FirstOrDefault(r => r.ProyectoID == proyectoID);

			if (redExistente != null)
			{
				ViewBag.RedExistente = redExistente;
				ViewBag.EsEdicion = true;

				var arquitectura = redExistente.Arquitectura.Split(',');
				if (arquitectura.Length >= 3)
				{
					ViewBag.TotalCapas = arquitectura[0];
					ViewBag.NeuronasEntrada = arquitectura[1];
					ViewBag.NeuronasSalida = arquitectura[arquitectura.Length - 1];

					var capasOcultas = new List<string>();
					for (int i = 2; i < arquitectura.Length - 1; i++)
					{
						capasOcultas.Add(arquitectura[i]);
					}
					ViewBag.CapasOcultas = capasOcultas;
				}
			}
			else
			{
				ViewBag.EsEdicion = false;
			}

			ViewBag.ProyectoID = proyectoID;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Usuario", "Admin")]
		public ActionResult Crear(int proyectoID, int totalCapas, int numNeuronasCapaEntrada,
						   int numNeuronasCapaSalida, double alfa, double errorMinimo,
						   int epocas, List<int> numNeuronasNCapas)
		{
			try
			{
				if (Session["UsuarioID"] == null)
					return RedirectToAction("Index", "Login");

				int usuarioId = (int)Session["UsuarioID"];
				var proyecto = db.Proyectos.FirstOrDefault(p => p.ID == proyectoID && p.UsuarioID == usuarioId);

				if (proyecto == null)
				{
					ModelState.AddModelError("", "Proyecto no encontrado o no tiene permisos");
					ViewBag.ProyectoID = proyectoID;
					return View();
				}

				// Validaciones
				if (totalCapas < 3 || totalCapas > 10)
				{
					ModelState.AddModelError("totalCapas", "El número total de capas debe estar entre 3 y 10");
				}

				if (numNeuronasCapaEntrada < 1 || numNeuronasCapaEntrada > 7)
				{
					ModelState.AddModelError("numNeuronasCapaEntrada", "Las neuronas de entrada deben estar entre 1 y 7");
				}

				if (numNeuronasCapaSalida < 1)
				{
					ModelState.AddModelError("numNeuronasCapaSalida", "Debe haber al menos 1 neurona en la capa de salida");
				}

				if (epocas < 10000)
				{
					ModelState.AddModelError("epocas", "Las épocas deben ser al menos 10000");
				}

				if (numNeuronasNCapas == null || numNeuronasNCapas.Count != totalCapas - 2)
				{
					ModelState.AddModelError("", "El número de capas ocultas no coincide con el total de capas");
				}

				if (!ModelState.IsValid)
				{
					ViewBag.ProyectoID = proyectoID;
					return View();
				}

				string arquitectura = totalCapas.ToString() + "," + numNeuronasCapaEntrada.ToString();

				foreach (var neuronas in numNeuronasNCapas)
				{
					arquitectura += "," + neuronas;
				}

				arquitectura += "," + numNeuronasCapaSalida;

				var redExistente = db.RedesNeuronales
					.FirstOrDefault(r => r.ProyectoID == proyectoID);

				if (redExistente != null)
				{
					redExistente.Epocas = epocas;
					redExistente.Arquitectura = arquitectura;
					redExistente.Alfa = alfa;
					redExistente.ErrorMin = errorMinimo;

					db.Entry(redExistente).State = System.Data.Entity.EntityState.Modified;
					db.SaveChanges();

					TempData["RedCreadaExitosamente"] = true;
					TempData["MensajeExito"] = "Red neuronal modificada exitosamente";
				}
				else
				{
					var redNeuronal = new RedNeuronal
					{
						ProyectoID = proyectoID,
						Epocas = epocas,
						Arquitectura = arquitectura,
						Alfa = alfa,
						ErrorMin = errorMinimo
					};

					db.RedesNeuronales.Add(redNeuronal);
					db.SaveChanges();

					TempData["RedCreadaExitosamente"] = true;
					TempData["MensajeExito"] = "Red neuronal creada exitosamente";
				}

				return RedirectToAction("Proyecto", "Cliente", new { proyectoID = proyectoID });
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error al crear/modificar red neuronal: " + ex.Message);
				System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

				ModelState.AddModelError("", "Error al crear/modificar la red neuronal: " + ex.Message);
				ViewBag.ProyectoID = proyectoID;
				return View();
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AutorizarRol("Usuario", "Admin")]
		public JsonResult Entrenar(int proyectoId, List<int> objetos)
		{
			try
			{
				if (Session["UsuarioID"] == null)
					return Json(new { success = false, message = "Sesión expirada" });

				return Json(new { success = true, message = "Entrenamiento iniciado" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error: " + ex.Message });
			}
		}
	}
}