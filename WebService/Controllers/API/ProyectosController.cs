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
    public class ProyectosController : ApiController
    {
        private Context db = new Context();

        // GET: api/Proyectos
        public IQueryable<Proyectos> GetProyectos()
        {
            return db.Proyectos;
        }

        // GET: api/Proyectos/5
        [ResponseType(typeof(Proyectos))]
        public async Task<IHttpActionResult> GetProyectos(int id)
        {
            Proyectos proyectos = await db.Proyectos.FindAsync(id);
            if (proyectos == null)
            {
                return NotFound();
            }

            return Ok(proyectos);
        }

        // PUT: api/Proyectos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProyectos(int id, Proyectos proyectos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proyectos.ID)
            {
                return BadRequest();
            }

            db.Entry(proyectos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectosExists(id))
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

        // POST: api/Proyectos
        [ResponseType(typeof(Proyectos))]
        public async Task<IHttpActionResult> PostProyectos(Proyectos proyectos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Proyectos.Add(proyectos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = proyectos.ID }, proyectos);
        }

        // DELETE: api/Proyectos/5
        [ResponseType(typeof(Proyectos))]
        public async Task<IHttpActionResult> DeleteProyectos(int id)
        {
            Proyectos proyectos = await db.Proyectos.FindAsync(id);
            if (proyectos == null)
            {
                return NotFound();
            }

            db.Proyectos.Remove(proyectos);
            await db.SaveChangesAsync();

            return Ok(proyectos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProyectosExists(int id)
        {
            return db.Proyectos.Count(e => e.ID == id) > 0;
        }
    }
}