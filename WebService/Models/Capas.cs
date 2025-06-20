using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Capas
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey(nameof(Neurona))]
		public int Id_Neuronas { get; set; }
		
		public int Num_Neuronas { get; set; }

		public string Tipo { get; set; }

		public virtual ICollection<Neuronas> Neurona { get; set; }
	}
}