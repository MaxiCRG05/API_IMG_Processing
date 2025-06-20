using FirebaseAdmin.Auth;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebService.Controllers
{
    public class AuthController : ApiController
    {
		[HttpPost]
		[ActionName("Login")]
		public async Task<IHttpActionResult> Login([FromBody] LoginModel model)
		{
			try
			{
				var auth = FirebaseAuth.DefaultInstance;
				var user = await auth.GetUserByEmailAsync(model.Correo);

				string customToken = await auth.CreateCustomTokenAsync(user.Uid);

				return Ok(new { Token = customToken, Uid = user.Uid });
			}
			catch (FirebaseAuthException ex)
			{
				Console.WriteLine($"Error durante el inicio de sesión: {ex.Message}");
				return Unauthorized();
			}
		}
	}

	public class LoginModel
	{
		public string Nombre { get; set; }
		public string Correo { get; set; }
		public string Contraseña { get; set; }
	}
}
