using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("Redes_Neuronales")]
	public class RedNeuronal
	{
		[Key]
		public int ID { get; set; }
		public int ProyectoID { get; set; }
		public int Epocas {  get; set; }
		public string Arquitectura { get; set; }
		public double Alfa {  get; set; }
		public double ErrorMin {  get; set; }
		[ForeignKey("ProyectoID")]
		public Proyecto Proyecto { get; set; }
	}
}