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
    public class InvariantesHuController : ApiController
    {
        private Context db = new Context();

        // GET: api/InvariantesHus
        public IQueryable<InvariantesHu> GetInvariantesHu()
        {
            return db.InvariantesHu;
        }

        // GET: api/InvariantesHus/5
        [ResponseType(typeof(InvariantesHu))]
        public async Task<IHttpActionResult> GetInvariantesHu(int id)
        {
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            if (invariantesHu == null)
            {
                return NotFound();
            }

            return Ok(invariantesHu);
        }

        // PUT: api/InvariantesHus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInvariantesHu(int id, InvariantesHu invariantesHu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invariantesHu.ID)
            {
                return BadRequest();
            }

            db.Entry(invariantesHu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvariantesHuExists(id))
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

        // POST: api/InvariantesHus
        [ResponseType(typeof(InvariantesHu))]
        public async Task<IHttpActionResult> PostInvariantesHu(InvariantesHu invariantesHu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InvariantesHu.Add(invariantesHu);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = invariantesHu.ID }, invariantesHu);
        }

        // DELETE: api/InvariantesHus/5
        [ResponseType(typeof(InvariantesHu))]
        public async Task<IHttpActionResult> DeleteInvariantesHu(int id)
        {
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            if (invariantesHu == null)
            {
                return NotFound();
            }

            db.InvariantesHu.Remove(invariantesHu);
            await db.SaveChangesAsync();

            return Ok(invariantesHu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvariantesHuExists(int id)
        {
            return db.InvariantesHu.Count(e => e.ID == id) > 0;
        }
    }
}