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
    public class ObjetosController : Controller
    {
        private Context db = new Context();

        // GET: Objetos
        public async Task<ActionResult> Index()
        {
            return View(await db.Objetos.ToListAsync());
        }

        // GET: Objetos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetos objetos = await db.Objetos.FindAsync(id);
            if (objetos == null)
            {
                return HttpNotFound();
            }
            return View(objetos);
        }

        // GET: Objetos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Objetos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Nombre,Clasificación")] Objetos objetos)
        {
            if (ModelState.IsValid)
            {
                db.Objetos.Add(objetos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(objetos);
        }

        // GET: Objetos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetos objetos = await db.Objetos.FindAsync(id);
            if (objetos == null)
            {
                return HttpNotFound();
            }
            return View(objetos);
        }

        // POST: Objetos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Nombre,Clasificación")] Objetos objetos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(objetos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(objetos);
        }

        // GET: Objetos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetos objetos = await db.Objetos.FindAsync(id);
            if (objetos == null)
            {
                return HttpNotFound();
            }
            return View(objetos);
        }

        // POST: Objetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Objetos objetos = await db.Objetos.FindAsync(id);
            db.Objetos.Remove(objetos);
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
