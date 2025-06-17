using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Usuario
	{
		[Key]
		public int Id { get; set; }

		public string Nombre { get; set; }

		public string Correo { get; set; }

		public string Contrasena { get; set; }

		public virtual ICollection<UsuariosProyectos> UsuariosProyectos { get; set; }
	}
}