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

		[HttpPut]
		[ActionName("Actualizar")]
		public void Put()
		{
			db.Set<Pesos>().RemoveRange(db.Pesos);
		}

	}
}
