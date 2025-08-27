using FirebaseAdmin.Auth;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebService.Data;
using WebService.Models;
using WebService.Scripts;

namespace WebService.Controllers
{
    public class AuthController : ApiController
	{
		private Context db = new Context();

		[HttpPost]
		[ActionName("VerificarContraseña")]
		public string VerificarContraseña()
		{
			return MetodosProcesamiento.Encriptar("hola mundo");
		}

		public class LoginModelDTO
		{
			public string Correo { get; set; }
			public string Contraseña { get; set; }
		}
	}
}
