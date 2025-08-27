using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	[Table("InvariantesHu")]
	public class InvariantesHu
	{
		[Key]
		public int ID { get; set; }
		public int ObjetoID { get; set; }
		public double Hu1 { get; set; }
		public double Hu2 { get; set; }
		public double Hu3 { get; set; }
		public double Hu4 { get; set; }
		public double Hu5 { get; set; }
		public double Hu6 { get; set; }
		public double Hu7 { get; set; }
		[ForeignKey("ObjetoID")]
		public Objeto Objeto { get; set; }
	}
}