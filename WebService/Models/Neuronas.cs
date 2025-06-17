using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Neuronas
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Umbral))]
		public int Id_Umbrales { get; set; }

		[ForeignKey(nameof(Peso))]
		public int Id_Pesos { get; set; }

		public virtual Umbrales Umbral { get; set; }

		public virtual Pesos Peso { get; set; }
	}
}