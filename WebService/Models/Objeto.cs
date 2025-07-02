using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
	[Table("Objetos")]
	public class Objeto
	{
		[Key]
		public int ID { get; set; }
		public int CategoriasID { get; set; }
		public string Nombre { get; set; }
		public byte[] Imagen { get; set; }

		[ForeignKey("CategoriasID")]
		public Categoria Categorias { get; set; }
	}
}