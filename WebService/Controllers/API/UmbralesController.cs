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
    public class UmbralesController : ApiController
    {
        private Context db = new Context();

        // GET: api/Umbrales
        public IQueryable<Umbrales> GetUmbrales()
        {
            return db.Umbrales;
        }

        // GET: api/Umbrales/5
        [ResponseType(typeof(Umbrales))]
        public async Task<IHttpActionResult> GetUmbrales(int id)
        {
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            if (umbrales == null)
            {
                return NotFound();
            }

            return Ok(umbrales);
        }

        // PUT: api/Umbrales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUmbrales(int id, Umbrales umbrales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != umbrales.ID)
            {
                return BadRequest();
            }

            db.Entry(umbrales).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UmbralesExists(id))
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

        // POST: api/Umbrales
        [ResponseType(typeof(Umbrales))]
        public async Task<IHttpActionResult> PostUmbrales(Umbrales umbrales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Umbrales.Add(umbrales);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = umbrales.ID }, umbrales);
        }

        // DELETE: api/Umbrales/5
        [ResponseType(typeof(Umbrales))]
        public async Task<IHttpActionResult> DeleteUmbrales(int id)
        {
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            if (umbrales == null)
            {
                return NotFound();
            }

            db.Umbrales.Remove(umbrales);
            await db.SaveChangesAsync();

            return Ok(umbrales);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UmbralesExists(int id)
        {
            return db.Umbrales.Count(e => e.ID == id) > 0;
        }
    }
}