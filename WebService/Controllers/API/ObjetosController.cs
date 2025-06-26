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
    public class ObjetosController : ApiController
    {
        private Context db = new Context();

        // GET: api/Objetos
        public IQueryable<Objetos> GetObjetos()
        {
            return db.Objetos;
        }

        // GET: api/Objetos/5
        [ResponseType(typeof(Objetos))]
        public async Task<IHttpActionResult> GetObjetos(int id)
        {
            Objetos objetos = await db.Objetos.FindAsync(id);
            if (objetos == null)
            {
                return NotFound();
            }

            return Ok(objetos);
        }

        // PUT: api/Objetos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObjetos(int id, Objetos objetos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != objetos.ID)
            {
                return BadRequest();
            }

            db.Entry(objetos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjetosExists(id))
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

        // POST: api/Objetos
        [ResponseType(typeof(Objetos))]
        public async Task<IHttpActionResult> PostObjetos(Objetos objetos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Objetos.Add(objetos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = objetos.ID }, objetos);
        }

        // DELETE: api/Objetos/5
        [ResponseType(typeof(Objetos))]
        public async Task<IHttpActionResult> DeleteObjetos(int id)
        {
            Objetos objetos = await db.Objetos.FindAsync(id);
            if (objetos == null)
            {
                return NotFound();
            }

            db.Objetos.Remove(objetos);
            await db.SaveChangesAsync();

            return Ok(objetos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObjetosExists(int id)
        {
            return db.Objetos.Count(e => e.ID == id) > 0;
        }
    }
}