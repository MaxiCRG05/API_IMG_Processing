using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Umbrales
	{
		[Key]
		public int Id { get; set; }

		public double Umbral { get; set; }

	}
}