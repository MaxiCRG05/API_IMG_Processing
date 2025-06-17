using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class InvariantesHu
	{
		[Key]
		public int Id { get; set; }
		public double Hu1 { get; set; }
		public double Hu2 { get; set; }
		public double Hu3 { get; set; }
		public double Hu4 { get; set; }
		public double Hu5 { get; set; }
		public double Hu6 { get; set; }
		public double Hu7 { get; set; }
	}
}