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
    public class UmbralesController : Controller
    {
        private Context db = new Context();

        // GET: Umbrales
        public async Task<ActionResult> Index()
        {
            var umbrales = db.Umbrales.Include(u => u.RedNeuronal);
            return View(await umbrales.ToListAsync());
        }

        // GET: Umbrales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbral umbral = await db.Umbrales.FindAsync(id);
            if (umbral == null)
            {
                return HttpNotFound();
            }
            return View(umbral);
        }

        // GET: Umbrales/Create
        public ActionResult Create()
        {
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura");
            return View();
        }

        // POST: Umbrales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RedNeuronalID,Umbrales")] Umbral umbral)
        {
            if (ModelState.IsValid)
            {
                db.Umbrales.Add(umbral);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", umbral.RedNeuronalID);
            return View(umbral);
        }

        // GET: Umbrales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbral umbral = await db.Umbrales.FindAsync(id);
            if (umbral == null)
            {
                return HttpNotFound();
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", umbral.RedNeuronalID);
            return View(umbral);
        }

        // POST: Umbrales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RedNeuronalID,Umbrales")] Umbral umbral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umbral).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", umbral.RedNeuronalID);
            return View(umbral);
        }

        // GET: Umbrales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbral umbral = await db.Umbrales.FindAsync(id);
            if (umbral == null)
            {
                return HttpNotFound();
            }
            return View(umbral);
        }

        // POST: Umbrales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Umbral umbral = await db.Umbrales.FindAsync(id);
            db.Umbrales.Remove(umbral);
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
