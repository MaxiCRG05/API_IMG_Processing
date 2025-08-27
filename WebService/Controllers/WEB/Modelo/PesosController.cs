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
    public class PesosController : Controller
    {
        private Context db = new Context();

        // GET: Pesos
        public async Task<ActionResult> Index()
        {
            var pesos = db.Pesos.Include(p => p.RedNeuronal);
            return View(await pesos.ToListAsync());
        }

        // GET: Pesos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peso peso = await db.Pesos.FindAsync(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            return View(peso);
        }

        // GET: Pesos/Create
        public ActionResult Create()
        {
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura");
            return View();
        }

        // POST: Pesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RedNeuronalID,Pesos")] Peso peso)
        {
            if (ModelState.IsValid)
            {
                db.Pesos.Add(peso);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", peso.RedNeuronalID);
            return View(peso);
        }

        // GET: Pesos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peso peso = await db.Pesos.FindAsync(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", peso.RedNeuronalID);
            return View(peso);
        }

        // POST: Pesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RedNeuronalID,Pesos")] Peso peso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(peso).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RedNeuronalID = new SelectList(db.RedesNeuronales, "ID", "Arquitectura", peso.RedNeuronalID);
            return View(peso);
        }

        // GET: Pesos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peso peso = await db.Pesos.FindAsync(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            return View(peso);
        }

        // POST: Pesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Peso peso = await db.Pesos.FindAsync(id);
            db.Pesos.Remove(peso);
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
