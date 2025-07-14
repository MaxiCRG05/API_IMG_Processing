using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Restablecer
	{
		[Required]
		public string Token { get; set; }

		[Required(ErrorMessage = "La nueva contraseña es requerida")]
		[MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
		public string NuevaContraseña { get; set; }

		[Required(ErrorMessage = "Confirma la nueva contraseña")]
		[Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden")]
		public string ConfirmarContraseña { get; set; }
	}
}