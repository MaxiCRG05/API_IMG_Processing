using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;

namespace WebService.Controllers.WEB
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(string email, string password)
		{
			using (var db = new Context())
			{
				var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == email);

				if (usuario == null)
				{
					ModelState.AddModelError("", "Usuario no encontrado");
					ViewBag.ErrorType = "UsuarioInexistente";
					return View("Index");
				}

				if (MetodosProcesamiento.VerificarContraseña(password, usuario.Contraseña))
				{
					IniciarSesion(usuario);

					if (usuario.Rol == "Admin")
					{ 
						return RedirectToAction("Index", "Admin");
					}
					else
					{
						return RedirectToAction("Index", "Cliente");
					}
				}

				ModelState.AddModelError("", "CredencialesInvalidas");
				ViewBag.ErrorType = "CredencialesInvalidas";
				return View("Index");
			}
		}

		public ActionResult Registro()
        { 
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Registro(Usuario user)
		{
            user.Rol = "Usuario";

			if (ModelState.IsValid)
			{
				using (var db = new Context())
				{
					if (db.Usuarios.Any(u => u.Correo == user.Correo))
					{
						ModelState.AddModelError("Correo", "El correo ya está registrado.");
						return View(user);
					}

                    user.Contraseña = MetodosProcesamiento.Encriptar(user.Contraseña);

					db.Usuarios.Add(user);
					db.SaveChanges();

					IniciarSesion(user);

					return RedirectToAction("Index", "Cliente");
				}
			}
			return View(user);
		}

		public ActionResult Recuperar()
        { 
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Recuperar(string correo)
		{
			using (var db = new Context())
			{
				var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);

				if (usuario != null)
				{
					usuario.TokenRecuperacion = MetodosProcesamiento.GenerarToken();
					usuario.ExpiracionToken = DateTime.Now.AddHours(1);
					db.SaveChanges();

					EnviarCorreoRecuperacion(usuario.Correo, usuario.TokenRecuperacion);
				}

				ViewBag.Mensaje = "Si el correo existe en nuestro sistema, recibirás un enlace para restablecer tu contraseña.";
				return View();
			}
		}

		public ActionResult Restablecer(string token)
		{
			using (var db = new Context())
			{
				var usuario = db.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

				if (usuario == null || !MetodosProcesamiento.TokenEsValido(usuario.ExpiracionToken))
				{
					ViewBag.Error = "El enlace es inválido o ha expirado";
					return View();
				}

				var model = new Restablecer
				{
					Token = token
				};

				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Restablecer(Restablecer model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			using (var db = new Context())
			{
				var usuario = db.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == model.Token);

				if (usuario == null || !MetodosProcesamiento.TokenEsValido(usuario.ExpiracionToken))
				{
					ViewBag.Error = "El enlace es inválido o ha expirado";
					return View(model);
				}
				
				if (model.NuevaContraseña != model.ConfirmarContraseña)
				{
					ModelState.AddModelError("ConfirmarContraseña", "Las contraseñas no coinciden");
					return View(model);
				}

				usuario.Contraseña = MetodosProcesamiento.Encriptar(model.NuevaContraseña);
				usuario.TokenRecuperacion = null;
				usuario.ExpiracionToken = null;
				db.SaveChanges();

				return RedirectToAction("Index", new { mensaje = "Contraseña actualizada correctamente" });
			}
		}

		private void EnviarCorreoRecuperacion(string correo, string token)
		{
			try
			{
				var resetUrl = Url.Action("Restablecer", "Login", new { token }, Request.Url.Scheme);
				var body = $@"<h1>Restablecimiento de contraseña</h1>
                     <p><a href='{resetUrl}'>Haz clic aquí</a> para continuar</p>
                     <p>Si no solicitaste esto, ignora este correo.</p>";

				var smtpUser = ConfigurationManager.AppSettings["SMTP_User"];
				var smtpPass = ConfigurationManager.AppSettings["SMTP_Pass"];

				using (var client = new SmtpClient())
				{
					client.Host = "smtp.office365.com";
					client.Port = 587;
					client.EnableSsl = true;
					client.UseDefaultCredentials = false;
					client.DeliveryMethod = SmtpDeliveryMethod.Network;
					client.Credentials = new NetworkCredential(smtpUser, smtpPass);
					client.Timeout = 10000;

					var mail = new MailMessage
					{
						From = new MailAddress(smtpUser, "Visión Artificial"),
						Subject = "Restablece tu contraseña",
						Body = body,
						IsBodyHtml = true
					};

					mail.To.Add(correo);
					client.Send(mail);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError($"Error enviando correo: {ex.Message}");
			}
		}

		public ActionResult ProbarCorreo(string correo)
		{
			try
			{
				var smtpUser = ConfigurationManager.AppSettings["SMTP_User"];
				var smtpPass = ConfigurationManager.AppSettings["SMTP_Pass"];

				using (var mensaje = new MailMessage())
				{
					mensaje.From = new MailAddress(smtpUser);
					mensaje.To.Add(correo);
					mensaje.Subject = "Prueba SMTP - " + DateTime.Now.ToString("HH:mm:ss");
					mensaje.Body = "<h1>¡Prueba exitosa!</h1><p>Esta es una prueba de configuración SMTP.</p>";
					mensaje.IsBodyHtml = true;

					using (var smtp = new SmtpClient("smtp.office365.com"))
					{
						smtp.Host = "smtp.office365.com";
						smtp.Port = 587;
						smtp.EnableSsl = true;
						smtp.UseDefaultCredentials = false;
						smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
						smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);

						smtp.Timeout = 15000;
						ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, errors) => true;
						smtp.Send(mensaje);
					}
				}
				return Content("¡Correo enviado correctamente!");
			}
			catch (SmtpException ex)
			{
				return Content($"Error SMTP: {ex.StatusCode}\n{ex.Message}\nDetalles: {ex.InnerException?.Message}");
			}
			catch (Exception ex)
			{
				return Content($"Error general: {ex.Message}\n{ex.StackTrace}");
			}
		}


		public ActionResult Logout()
		{
			Session.Clear();
			Session.Abandon();
			return RedirectToAction("Index", "Login");
		}

		public void IniciarSesion(Usuario usuario)
		{
			Session["UsuarioID"] = usuario.ID;
			Session["Rol"] = usuario.Rol;
			Session["Nombre"] = usuario.Nombre;
		}
	}
}