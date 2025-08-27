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
            var redesNeuronales = db.RedesNeuronales.Include(r => r.Proyecto);
            return View(await redesNeuronales.ToListAsync());
        }

        // GET: RedesNeuronales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            if (redNeuronal == null)
            {
                return HttpNotFound();
            }
            return View(redNeuronal);
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
        public async Task<ActionResult> Create([Bind(Include = "ID,ProyectoID,Epocas,Arquitectura,Alfa,ErrorMin")] RedNeuronal redNeuronal)
        {
            if (ModelState.IsValid)
            {
                db.RedesNeuronales.Add(redNeuronal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redNeuronal.ProyectoID);
            return View(redNeuronal);
        }

        // GET: RedesNeuronales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            if (redNeuronal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redNeuronal.ProyectoID);
            return View(redNeuronal);
        }

        // POST: RedesNeuronales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProyectoID,Epocas,Arquitectura,Alfa,ErrorMin")] RedNeuronal redNeuronal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(redNeuronal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProyectoID = new SelectList(db.Proyectos, "ID", "Nombre", redNeuronal.ProyectoID);
            return View(redNeuronal);
        }

        // GET: RedesNeuronales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            if (redNeuronal == null)
            {
                return HttpNotFound();
            }
            return View(redNeuronal);
        }

        // POST: RedesNeuronales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RedNeuronal redNeuronal = await db.RedesNeuronales.FindAsync(id);
            db.RedesNeuronales.Remove(redNeuronal);
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
