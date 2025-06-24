using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Objetos
	{
		[Key]
		public int ID { get; set; }
		public string Nombre { get; set; }
		public string Clasificación { get; set; }
	}
}