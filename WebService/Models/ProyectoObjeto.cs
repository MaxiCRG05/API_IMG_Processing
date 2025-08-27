using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("Proyectos_Objetos")]
	public class ProyectoObjeto
	{
		[Key]
		public int ID { get; set; }

		public int ProyectoID { get; set; }
		public int ObjetoID { get; set; }

		[ForeignKey("ProyectoID")]
		public virtual Proyecto Proyecto { get; set; }

		[ForeignKey("ObjetoID")]
		public virtual Objeto Objeto { get; set; }
	}
}