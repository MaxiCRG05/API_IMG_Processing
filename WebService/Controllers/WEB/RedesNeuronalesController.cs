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
    public class RedesNeuronalesController : Controller
    {
        private Context db = new Context();

        // GET: RedesNeuronales
        public async Task<ActionResult> Index()
        {
            var redNeuronales = db.RedNeuronales.Include(r => r.Proyecto);
            return View(await redNeuronales.ToListAsync());
        }

        // GET: RedesNeuronales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            if (redesNeuronales == null)
            {
                return HttpNotFound();
            }
            return View(redesNeuronales);
        }

        // GET: RedesNeuronales/Create
        public ActionResult Create()
        {
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre");
            return View();
        }

        // POST: RedesNeuronales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProyectoID,Epocas,Arquitectura,Alfa,ErrorMinimo")] RedesNeuronales redesNeuronales)
        {
            if (ModelState.IsValid)
            {
                db.RedNeuronales.Add(redesNeuronales);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redesNeuronales.ProyectoID);
            return View(redesNeuronales);
        }

        // GET: RedesNeuronales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            if (redesNeuronales == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redesNeuronales.ProyectoID);
            return View(redesNeuronales);
        }

        // POST: RedesNeuronales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProyectoID,Epocas,Arquitectura,Alfa,ErrorMinimo")] RedesNeuronales redesNeuronales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(redesNeuronales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redesNeuronales.ProyectoID);
            return View(redesNeuronales);
        }

        // GET: RedesNeuronales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            if (redesNeuronales == null)
            {
                return HttpNotFound();
            }
            return View(redesNeuronales);
        }

        // POST: RedesNeuronales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RedesNeuronales redesNeuronales = await db.RedNeuronales.FindAsync(id);
            db.RedNeuronales.Remove(redesNeuronales);
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
