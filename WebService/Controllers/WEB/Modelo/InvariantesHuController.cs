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
    public class InvariantesHuController : Controller
    {
        private Context db = new Context();

        // GET: InvariantesHu
        public async Task<ActionResult> Index()
        {
            var invariantesHu = db.InvariantesHu.Include(i => i.Objeto);
            return View(await invariantesHu.ToListAsync());
        }

        // GET: InvariantesHu/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            if (invariantesHu == null)
            {
                return HttpNotFound();
            }
            return View(invariantesHu);
        }

        // GET: InvariantesHu/Create
        public ActionResult Create()
        {
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre");
            return View();
        }

        // POST: InvariantesHu/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ObjetoID,Hu1,Hu2,Hu3,Hu4,Hu5,Hu6,Hu7")] InvariantesHu invariantesHu)
        {
            if (ModelState.IsValid)
            {
                db.InvariantesHu.Add(invariantesHu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", invariantesHu.ObjetoID);
            return View(invariantesHu);
        }

        // GET: InvariantesHu/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            if (invariantesHu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", invariantesHu.ObjetoID);
            return View(invariantesHu);
        }

        // POST: InvariantesHu/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ObjetoID,Hu1,Hu2,Hu3,Hu4,Hu5,Hu6,Hu7")] InvariantesHu invariantesHu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invariantesHu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ObjetoID = new SelectList(db.Objetos, "ID", "Nombre", invariantesHu.ObjetoID);
            return View(invariantesHu);
        }

        // GET: InvariantesHu/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            if (invariantesHu == null)
            {
                return HttpNotFound();
            }
            return View(invariantesHu);
        }

        // POST: InvariantesHu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InvariantesHu invariantesHu = await db.InvariantesHu.FindAsync(id);
            db.InvariantesHu.Remove(invariantesHu);
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
