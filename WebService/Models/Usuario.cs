using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
	[Table("Usuarios")]
	public class Usuario
	{
		[Key]
		public int ID { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio")]
		public string Nombre { get; set; }

		[Required(ErrorMessage = "El correo es obligatorio")]
		[EmailAddress(ErrorMessage = "Formato de correo inválido")]
		public string Correo { get; set; }

		[Required(ErrorMessage = "La contraseña es obligatoria")]
		[MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
		public string Contraseña { get; set; }

		public string Rol { get; set; }
		public string TokenRecuperacion { get; set; }
		public DateTime? ExpiracionToken { get; set; }
	}
}