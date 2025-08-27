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
        public IQueryable<Proyecto> GetProyectos()
        {
            return db.Proyectos;
        }

        // GET: api/Proyectos/5
        [ResponseType(typeof(Proyecto))]
        public async Task<IHttpActionResult> GetProyecto(int id)
        {
            Proyecto proyecto = await db.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }

            return Ok(proyecto);
        }

        // PUT: api/Proyectos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProyecto(int id, Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proyecto.ID)
            {
                return BadRequest();
            }

            db.Entry(proyecto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectoExists(id))
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
        [ResponseType(typeof(Proyecto))]
        public async Task<IHttpActionResult> PostProyecto(Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Proyectos.Add(proyecto);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = proyecto.ID }, proyecto);
        }

        // DELETE: api/Proyectos/5
        [ResponseType(typeof(Proyecto))]
        public async Task<IHttpActionResult> DeleteProyecto(int id)
        {
            Proyecto proyecto = await db.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }

            db.Proyectos.Remove(proyecto);
            await db.SaveChangesAsync();

            return Ok(proyecto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProyectoExists(int id)
        {
            return db.Proyectos.Count(e => e.ID == id) > 0;
        }
    }
}