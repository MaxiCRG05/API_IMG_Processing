using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
	[Table("Usuarios")]
	public class Usuario
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public string Nombre { get; set; }

		[Required]
		[EmailAddress]
		public string Correo { get; set; }

		[Required]
		public string Contraseña { get; set; }

		[Required]
		public string Rol { get; set; }
	}
}