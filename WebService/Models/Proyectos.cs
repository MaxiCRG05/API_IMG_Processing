using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Proyectos
	{
		[Key]
		public int ID { get; set; }
		public int UsuariosID { get; set; }
		public string Nombre { get; set; }
		[ForeignKey("UsuariosID")]
		public Usuarios Usuario { get; set; }
	}
}