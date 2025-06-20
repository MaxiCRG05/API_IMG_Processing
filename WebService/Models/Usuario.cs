using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Usuario
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(Rol))]
		public int Id_rol { get; set; }

		public string Nombre { get; set; }

		public string Correo { get; set; }

		public string Contrasena { get; set; }

		public virtual ICollection<UsuariosProyectos> UsuariosProyectos { get; set; }
		public virtual ICollection<Rol> Rol { get; set; }
	}
}