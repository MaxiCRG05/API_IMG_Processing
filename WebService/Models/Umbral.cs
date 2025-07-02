using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("Umbrales")]
	public class Umbral
	{
		[Key]
		public int ID { get; set; }
		public int RedNeuronalID { get; set; }
		public double Umbrales { get; set; }
		[ForeignKey("RedNeuronalID")]
		public RedNeuronal RedNeuronal { get; set; }
	}
}