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

        // GET: api/Pesos
        public IQueryable<Peso> GetPesos()
        {
            return db.Pesos;
        }

        // GET: api/Pesos/5
        [ResponseType(typeof(Peso))]
        public async Task<IHttpActionResult> GetPeso(int id)
        {
            Peso peso = await db.Pesos.FindAsync(id);
            if (peso == null)
            {
                return NotFound();
            }

            return Ok(peso);
        }

        // PUT: api/Pesos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPeso(int id, Peso peso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != peso.ID)
            {
                return BadRequest();
            }

            db.Entry(peso).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PesoExists(id))
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
        [ResponseType(typeof(Peso))]
        public async Task<IHttpActionResult> PostPeso(Peso peso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pesos.Add(peso);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = peso.ID }, peso);
        }

        // DELETE: api/Pesos/5
        [ResponseType(typeof(Peso))]
        public async Task<IHttpActionResult> DeletePeso(int id)
        {
            Peso peso = await db.Pesos.FindAsync(id);
            if (peso == null)
            {
                return NotFound();
            }

            db.Pesos.Remove(peso);
            await db.SaveChangesAsync();

            return Ok(peso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PesoExists(int id)
        {
            return db.Pesos.Count(e => e.ID == id) > 0;
        }
    }
}