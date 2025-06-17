using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebService.Models
{
	public class Objetos
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(InvariantesHu))]
		public int Id_InvariantesHu { get; set; }

		public string NombreObjeto { get; set; }

		public virtual InvariantesHu InvariantesHu { get; set; }

	}
}