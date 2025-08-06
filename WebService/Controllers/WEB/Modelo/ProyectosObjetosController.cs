using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebService.Data;
using WebService.Models;

namespace WebService.Controllers.WEB
{
    public class ProyectosObjetosController : Controller
    {
        private Context db = new Context();

        // GET: ProyectosObjetos
        public async Task<ActionResult> Index()
        {
            var proyectosObjetos = db.ProyectosObjetos.Include(p => p.Objeto).Include(p => p.Proyecto);
            return View(await proyectosObjetos.ToListAsync());
        }

        // GET: ProyectosObjetos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            if (proyectoObjeto == null)
            {
                return HttpNotFound();
            }
            return View(proyectoObjeto);
        }

        // GET: ProyectosObjetos/Create
        public ActionResult Create()
        {
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre");
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre");
            return View();
        }

        // POST: ProyectosObjetos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProyectoID,ObjetoID")] ProyectoObjeto proyectoObjeto)
        {
            if (ModelState.IsValid)
            {
                db.ProyectosObjetos.Add(proyectoObjeto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", proyectoObjeto.ObjetoID);
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", proyectoObjeto.ProyectoID);
            return View(proyectoObjeto);
        }

        // GET: ProyectosObjetos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            if (proyectoObjeto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", proyectoObjeto.ObjetoID);
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", proyectoObjeto.ProyectoID);
            return View(proyectoObjeto);
        }

        // POST: ProyectosObjetos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProyectoID,ObjetoID")] ProyectoObjeto proyectoObjeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectoObjeto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", proyectoObjeto.ObjetoID);
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", proyectoObjeto.ProyectoID);
            return View(proyectoObjeto);
        }

        // GET: ProyectosObjetos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            if (proyectoObjeto == null)
            {
                return HttpNotFound();
            }
            return View(proyectoObjeto);
        }

        // POST: ProyectosObjetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProyectoObjeto proyectoObjeto = await db.ProyectosObjetos.FindAsync(id);
            db.ProyectosObjetos.Remove(proyectoObjeto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
