using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Pesos
	{
		[Key]
		public int Id { get; set; }

		public double Peso { get; set; }	
	}
}