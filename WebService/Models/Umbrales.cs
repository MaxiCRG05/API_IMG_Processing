using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Umbrales
	{
		[Key]
		public int ID { get; set; }
		public int RedNeuronalID { get; set; }
		public double Umbral { get; set; }
		[ForeignKey("RedNeuronalID")]
		public RedesNeuronales RedesNeuronales { get; set; }
	}
}