using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("Proyectos_Categorias")]
	public class ProyectosCategorias
	{
		[Key]
		public int ProyectoID { get; set; }

		[Key]
		public int CategoriaID { get; set; }

		[ForeignKey("ProyectoID")]
		public virtual Proyecto Proyectos { get; set; }

		[ForeignKey("CategoriaID")]
		public virtual Categoria Categorias { get; set; }
	}
}