using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class ProyectosObjetos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(Proyecto))]
		public int Id_Proyecto { get; set; }

		[ForeignKey(nameof(Objeto))]
		public int Id_Objetos { get; set; }

		public virtual Proyecto Proyecto { get; set; }

		public virtual Objetos Objeto { get; set; }
	}
}