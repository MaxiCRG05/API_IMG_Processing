using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Usuarios
	{
		[Key]
		public int ID { get; set; }
		public string Nombre { get; set; }
		public string Correo { get; set; }
		public string Contraseña { get; set; }
	}
}