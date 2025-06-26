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
    public class RedesNeuronalesController : ApiController
    {
        private Context db = new Context();

        // GET: api/RedesNeuronales
        public IQueryable<RedesNeuronales> GetRedNeuronales()
        {
            return db.RedNeuronales;
        }

        // GET: api/RedesNeuronales/5
        [ResponseType(typeof(RedesNeuronales))]
        public async Task<IHttpActionResult> GetRedesNeuronales(int id)
        {
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            if (redesNeuronales == null)
            {
                return NotFound();
            }

            return Ok(redesNeuronales);
        }

        // PUT: api/RedesNeuronales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRedesNeuronales(int id, RedesNeuronales redesNeuronales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != redesNeuronales.ID)
            {
                return BadRequest();
            }

            db.Entry(redesNeuronales).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedesNeuronalesExists(id))
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

        // POST: api/RedesNeuronales
        [ResponseType(typeof(RedesNeuronales))]
        public async Task<IHttpActionResult> PostRedesNeuronales(RedesNeuronales redesNeuronales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RedNeuronales.Add(redesNeuronales);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = redesNeuronales.ID }, redesNeuronales);
        }

        // DELETE: api/RedesNeuronales/5
        [ResponseType(typeof(RedesNeuronales))]
        public async Task<IHttpActionResult> DeleteRedesNeuronales(int id)
        {
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            if (redesNeuronales == null)
            {
                return NotFound();
            }

            db.RedNeuronales.Remove(redesNeuronales);
            await db.SaveChangesAsync();

            return Ok(redesNeuronales);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RedesNeuronalesExists(int id)
        {
            return db.RedNeuronales.Count(e => e.ID == id) > 0;
        }
    }
}