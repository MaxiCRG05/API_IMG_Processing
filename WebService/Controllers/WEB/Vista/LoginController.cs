using System;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;
using System.Web.UI.WebControls.WebParts;

namespace WebService.Controllers.WEB
{
	public class LoginController : Controller
	{
		// GET: Login
		public ActionResult Index()
		{
			if (TempData["ErrorType"] != null)
				ViewBag.ErrorType = TempData["ErrorType"];
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
			ViewBag.Success = null;
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
						ViewBag.ErrorType = "CorreoExistente";
						return View(user);
					}

					user.Contraseña = MetodosProcesamiento.Encriptar(user.Contraseña);

					db.Usuarios.Add(user);
					db.SaveChanges();

					IniciarSesion(user);
					ViewBag.Success = true;
					return View();
				}
			}
			ViewBag.ErrorType = "ErrorGeneral";
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
				var expirados = db.Usuarios.Where(u => u.ExpiracionToken < DateTime.Now).ToList();

				foreach (var exp in expirados)
				{
					exp.TokenRecuperacion = null;
					exp.ExpiracionToken = null;
				}
				db.SaveChanges();

				var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);

				if (usuario != null)
				{
					usuario.TokenRecuperacion = MetodosProcesamiento.GenerarToken();
					usuario.ExpiracionToken = DateTime.Now.AddHours(1);
					db.SaveChanges();

					EnviarCorreoRecuperacion(usuario.Nombre, usuario.Correo, usuario.TokenRecuperacion);
					
				}
				else
				{
					ViewBag.ErrorType = "UsuarioInexistente";
				}

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
					ViewBag.ErrorType = "Error";
					return View(model);
				}
				
				if (model.NuevaContraseña != model.ConfirmarContraseña)
				{
					ModelState.AddModelError("ConfirmarContraseña", "Las contraseñas no coinciden");
					ViewBag.ErrorType = "Diferentes";
					return View(model);
				}

				usuario.Contraseña = MetodosProcesamiento.Encriptar(model.NuevaContraseña);
				usuario.TokenRecuperacion = null;
				usuario.ExpiracionToken = null;
				db.SaveChanges();

				TempData["ErrorType"] = "ExitoContraseña";
				return RedirectToAction("Index");
			}
		}

		private void EnviarCorreoRecuperacion(string usuario, string correo, string token)
		{
			try
			{
				var resetUrl = Url.Action("Restablecer", "Login", new { token }, Request.Url.Scheme);
				var email = new MimeMessage();

				email.From.Add(new MailboxAddress("Visión Artificial", ""));
				email.To.Add(new MailboxAddress(usuario, correo));
				email.Subject = "Restablecimiento de contraseña";
				email.Body = new TextPart()
				{
					Text = $@"
					 <h1>Restablecimiento de contraseña</h1>
					 <p>Hola {usuario},</p>
					 <p>Para restablecer tu contraseña, haz clic en el siguiente enlace:</p>
					 <p><a href='{resetUrl}'>Restablecer contraseña</a></p>
					 <p>Si no solicitaste esto, ignora este correo.</p>"
				};

				using (var smtp = new SmtpClient())
				{
					smtp.Connect("smtp.gmail.com", 465, false);
					smtp.Authenticate("", "");
					smtp.Send(email);
					smtp.Disconnect(true);
				}
				ViewBag.ErrorType = "Enviado";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError($"Error enviando correo: {ex.Message}");
				ViewBag.ErrorType = "Error";
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