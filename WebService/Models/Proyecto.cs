using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("Proyectos")]
	public class Proyecto
	{
		[Key]
		public int ID { get; set; }
		[Required]
		public int UsuarioID { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaModificacion { get; set; }

		[Required]
		[ForeignKey("UsuarioID")]
		public Usuario Usuarios { get; set; }
	}
}