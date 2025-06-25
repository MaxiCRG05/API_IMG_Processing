using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebService.Data;
using WebService.Models;

namespace WebService.Controllers.API
{
    public class PMLController : ApiController
    {
        private Context db = new Context();

        [HttpGet]
		[ActionName("Pesos")]
		public List<Pesos> Pesos()
        {
			return db.Pesos.ToList();
		}

		[HttpGet]
		[ActionName("Umbrales")]
		public List<Umbrales> Umbrales()
		{
			return db.Umbrales.ToList();
		}

		[HttpGet]
		[ActionName("RedesNeuronales")]
		public List<RedesNeuronales> RedesNeuronales()
		{
			return db.RedNeuronales.ToList();
		}
		
		[HttpGet]
		[ActionName("InvariantesHu")]
		public List<InvariantesHu> InvariantesHu()
		{
			return db.InvariantesHu.ToList();
		}

		[HttpGet]
		[ActionName("Objetos")]
		public List<Objetos> Objetos()
		{
			return db.Objetos.ToList();
		}

		[HttpGet]
		[ActionName("Proyectos")]
		public List<Proyectos> Proyectos()
		{
			return db.Proyectos.ToList();
		}

		[HttpGet]
		[ActionName("Usuarios")]
		public List<Usuarios> Usuarios()
		{
			return db.Usuario.ToList();
		}

		[HttpPut]
		[ActionName("Actualizar")]
		public void Put()	
		{
			db.Set<Pesos>().RemoveRange(db.Pesos);
		}

	}
}
