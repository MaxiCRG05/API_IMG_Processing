using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Proyecto
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Nombre { get; set; }

		public virtual ICollection<UsuariosProyectos> UsuariosProyectos { get; set; }
		public virtual ICollection<ProyectosObjetos> ProyectosObjetos { get; set; }
	}
}