using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class RedNeuronal
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(Capa))]
		public int Id_Capas { get; set; }
		public int Num_Capas { get; set; }

		public virtual ICollection<Capas> Capa { get; set; }
	}
}