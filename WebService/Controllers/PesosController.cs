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

namespace WebService.Controllers
{
    public class PesosController : Controller
    {
        private Context db = new Context();

        // GET: Pesos
        public async Task<ActionResult> Index()
        {
            return View(await db.Pesos.ToListAsync());
        }

        // GET: Pesos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pesos pesos = await db.Pesos.FindAsync(id);
            if (pesos == null)
            {
                return HttpNotFound();
            }
            return View(pesos);
        }

        // GET: Pesos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Peso")] Pesos pesos)
        {
            if (ModelState.IsValid)
            {
                db.Pesos.Add(pesos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pesos);
        }

        // GET: Pesos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pesos pesos = await db.Pesos.FindAsync(id);
            if (pesos == null)
            {
                return HttpNotFound();
            }
            return View(pesos);
        }

        // POST: Pesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Peso")] Pesos pesos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pesos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pesos);
        }

        // GET: Pesos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pesos pesos = await db.Pesos.FindAsync(id);
            if (pesos == null)
            {
                return HttpNotFound();
            }
            return View(pesos);
        }

        // POST: Pesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pesos pesos = await db.Pesos.FindAsync(id);
            db.Pesos.Remove(pesos);
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
