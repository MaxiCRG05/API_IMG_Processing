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
    public class RestablecerController : ApiController
    {
        private Context db = new Context();

        // GET: api/Restablecer
        public IQueryable<Restablecer> GetRestablecer()
        {
            return db.Restablecer;
        }

        // GET: api/Restablecer/5
        [ResponseType(typeof(Restablecer))]
        public async Task<IHttpActionResult> GetRestablecer(int id)
        {
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            if (restablecer == null)
            {
                return NotFound();
            }

            return Ok(restablecer);
        }

        // PUT: api/Restablecer/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRestablecer(int id, Restablecer restablecer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restablecer.Id)
            {
                return BadRequest();
            }

            db.Entry(restablecer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestablecerExists(id))
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

        // POST: api/Restablecer
        [ResponseType(typeof(Restablecer))]
        public async Task<IHttpActionResult> PostRestablecer(Restablecer restablecer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restablecer.Add(restablecer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = restablecer.Id }, restablecer);
        }

        // DELETE: api/Restablecer/5
        [ResponseType(typeof(Restablecer))]
        public async Task<IHttpActionResult> DeleteRestablecer(int id)
        {
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            if (restablecer == null)
            {
                return NotFound();
            }

            db.Restablecer.Remove(restablecer);
            await db.SaveChangesAsync();

            return Ok(restablecer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestablecerExists(int id)
        {
            return db.Restablecer.Count(e => e.Id == id) > 0;
        }
    }
}