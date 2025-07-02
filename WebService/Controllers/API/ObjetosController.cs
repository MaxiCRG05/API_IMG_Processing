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
        public IQueryable<Objeto> GetObjetos()
        {
            return db.Objetos;
        }

        // GET: api/Objetos/5
        [ResponseType(typeof(Objeto))]
        public async Task<IHttpActionResult> GetObjeto(int id)
        {
            Objeto objeto = await db.Objetos.FindAsync(id);
            if (objeto == null)
            {
                return NotFound();
            }

            return Ok(objeto);
        }

        // PUT: api/Objetos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObjeto(int id, Objeto objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != objeto.ID)
            {
                return BadRequest();
            }

            db.Entry(objeto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjetoExists(id))
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
        [ResponseType(typeof(Objeto))]
        public async Task<IHttpActionResult> PostObjeto(Objeto objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Objetos.Add(objeto);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = objeto.ID }, objeto);
        }

        // DELETE: api/Objetos/5
        [ResponseType(typeof(Objeto))]
        public async Task<IHttpActionResult> DeleteObjeto(int id)
        {
            Objeto objeto = await db.Objetos.FindAsync(id);
            if (objeto == null)
            {
                return NotFound();
            }

            db.Objetos.Remove(objeto);
            await db.SaveChangesAsync();

            return Ok(objeto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObjetoExists(int id)
        {
            return db.Objetos.Count(e => e.ID == id) > 0;
        }
    }
}