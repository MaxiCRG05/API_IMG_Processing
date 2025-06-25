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
            var umbrales = db.Umbrales.Include(u => u.RedesNeuronales);
            return View(await umbrales.ToListAsync());
        }

        // GET: Umbrales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            return View(umbrales);
        }

        // GET: Umbrales/Create
        public ActionResult Create()
        {
            ViewBag.RedNeuronalID = new SelectList(db.RedNeuronales, "ID", "Arquitectura");
            return View();
        }

        // POST: Umbrales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RedNeuronalID,Umbral")] Umbrales umbrales)
        {
            if (ModelState.IsValid)
            {
                db.Umbrales.Add(umbrales);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RedNeuronalID = new SelectList(db.RedNeuronales, "ID", "Arquitectura", umbrales.RedNeuronalID);
            return View(umbrales);
        }

        // GET: Umbrales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedNeuronales, "ID", "Arquitectura", umbrales.RedNeuronalID);
            return View(umbrales);
        }

        // POST: Umbrales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RedNeuronalID,Umbral")] Umbrales umbrales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umbrales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedNeuronales, "ID", "Arquitectura", umbrales.RedNeuronalID);
            return View(umbrales);
        }

        // GET: Umbrales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            return View(umbrales);
        }

        // POST: Umbrales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Umbrales umbrales = await db.Umbrales.FindAsync(id);
            db.Umbrales.Remove(umbrales);
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
