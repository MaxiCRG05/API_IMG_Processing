using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class UsuariosProyectos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(Usuario))]
		public int Id_Usuario { get; set; }

		[ForeignKey(nameof(Proyecto))]
		public int Id_Proyecto { get; set; }

		[ForeignKey(nameof(RedNeuronal))]
		public int Id_RedNeuronal { get; set; }

		public virtual Usuario Usuario { get; set; }

		public virtual Proyecto Proyecto { get; set; }

		public virtual RedNeuronal RedNeuronal { get; set; }
	}
}