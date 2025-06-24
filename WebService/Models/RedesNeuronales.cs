using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class RedesNeuronales
	{
		[Key]
		public int ID { get; set; }
		public int ProyectoID { get; set; }
		public int Epocas {  get; set; }
		public string Arquitectura { get; set; }
		public double Alfa {  get; set; }
		public double ErrorMinimo {  get; set; }
		[ForeignKey("ProyectoID")]
		public Proyectos Proyecto { get; set; }
	}
}