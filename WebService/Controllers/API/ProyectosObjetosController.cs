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
    public class ProyectosObjetosController : ApiController
    {
        private Context db = new Context();

        // GET: api/ProyectosObjetos
        public IQueryable<ProyectoObjeto> GetProyectosObjetos()
        {
            return db.ProyectosObjetos;
        }

        // GET: api/ProyectosObjetos/5
        [ResponseType(typeof(ProyectoObjeto))]
        public async Task<IHttpActionResult> GetProyectoObjeto(int id)
        {
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            if (proyectoObjeto == null)
            {
                return NotFound();
            }

            return Ok(proyectoObjeto);
        }

        // PUT: api/ProyectosObjetos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProyectoObjeto(int id, ProyectoObjeto proyectoObjeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proyectoObjeto.ID)
            {
                return BadRequest();
            }

            db.Entry(proyectoObjeto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectoObjetoExists(id))
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

        // POST: api/ProyectosObjetos
        [ResponseType(typeof(ProyectoObjeto))]
        public async Task<IHttpActionResult> PostProyectoObjeto(ProyectoObjeto proyectoObjeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProyectosObjetos.Add(proyectoObjeto);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = proyectoObjeto.ID }, proyectoObjeto);
        }

        // DELETE: api/ProyectosObjetos/5
        [ResponseType(typeof(ProyectoObjeto))]
        public async Task<IHttpActionResult> DeleteProyectoObjeto(int id)
        {
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            if (proyectoObjeto == null)
            {
                return NotFound();
            }

            db.ProyectosObjetos.Remove(proyectoObjeto);
            await db.SaveChangesAsync();

            return Ok(proyectoObjeto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProyectoObjetoExists(int id)
        {
            return db.ProyectosObjetos.Count(e => e.ID == id) > 0;
        }
    }
}