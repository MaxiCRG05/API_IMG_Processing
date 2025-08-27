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
        public IQueryable<RedNeuronal> GetRedesNeuronales()
        {
            return db.RedesNeuronales;
        }

        // GET: api/RedesNeuronales/5
        [ResponseType(typeof(RedNeuronal))]
        public async Task<IHttpActionResult> GetRedNeuronal(int id)
        {
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            if (redNeuronal == null)
            {
                return NotFound();
            }

            return Ok(redNeuronal);
        }

        // PUT: api/RedesNeuronales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRedNeuronal(int id, RedNeuronal redNeuronal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != redNeuronal.ID)
            {
                return BadRequest();
            }

            db.Entry(redNeuronal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedNeuronalExists(id))
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
        [ResponseType(typeof(RedNeuronal))]
        public async Task<IHttpActionResult> PostRedNeuronal(RedNeuronal redNeuronal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RedesNeuronales.Add(redNeuronal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = redNeuronal.ID }, redNeuronal);
        }

        // DELETE: api/RedesNeuronales/5
        [ResponseType(typeof(RedNeuronal))]
        public async Task<IHttpActionResult> DeleteRedNeuronal(int id)
        {
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            if (redNeuronal == null)
            {
                return NotFound();
            }

            db.RedesNeuronales.Remove(redNeuronal);
            await db.SaveChangesAsync();

            return Ok(redNeuronal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RedNeuronalExists(int id)
        {
            return db.RedesNeuronales.Count(e => e.ID == id) > 0;
        }
    }
}