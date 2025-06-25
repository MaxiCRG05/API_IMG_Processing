using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebService.Data;
using WebService.Models;

namespace WebService.Controllers.API
{
    public class PesosController : ApiController
    {
        private Context db = new Context();

		// GET: api/Pesos/
		public IQueryable<Pesos> GetPesos()
        {
            return db.Pesos;
        }

        // GET: api/Pesos/5
        [ResponseType(typeof(Pesos))]
        public async Task<IHttpActionResult> GetPesos(int id)
        {
            Pesos pesos = await db.Pesos.FindAsync(id);
            if (pesos == null)
            {
                return NotFound();
            }

            return Ok(pesos);
        }

        // PUT: api/Pesos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPesos(int id, Pesos pesos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pesos.ID)
            {
                return BadRequest();
            }

            db.Entry(pesos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PesosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pesos
        [ResponseType(typeof(Pesos))]
        public async Task<IHttpActionResult> PostPesos(Pesos pesos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pesos.Add(pesos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pesos.ID }, pesos);
        }

        // DELETE: api/Pesos/5
        [ResponseType(typeof(Pesos))]
        public async Task<IHttpActionResult> DeletePesos(int id)
        {
            Pesos pesos = await db.Pesos.FindAsync(id);
            if (pesos == null)
            {
                return NotFound();
            }

            db.Pesos.Remove(pesos);
            await db.SaveChangesAsync();

            return Ok(pesos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PesosExists(int id)
        {
            return db.Pesos.Count(e => e.ID == id) > 0;
        }
    }
}