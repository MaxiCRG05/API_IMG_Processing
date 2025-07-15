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
    public class RestablecerController : Controller
    {
        private Context db = new Context();

        // GET: Restablecer
        public async Task<ActionResult> Index()
        {
            return View(await db.Restablecer.ToListAsync());
        }

        // GET: Restablecer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            if (restablecer == null)
            {
                return HttpNotFound();
            }
            return View(restablecer);
        }

        // GET: Restablecer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restablecer/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Token,NuevaContraseña,ConfirmarContraseña")] Restablecer restablecer)
        {
            if (ModelState.IsValid)
            {
                db.Restablecer.Add(restablecer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(restablecer);
        }

        // GET: Restablecer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            if (restablecer == null)
            {
                return HttpNotFound();
            }
            return View(restablecer);
        }

        // POST: Restablecer/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Token,NuevaContraseña,ConfirmarContraseña")] Restablecer restablecer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restablecer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(restablecer);
        }

        // GET: Restablecer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            if (restablecer == null)
            {
                return HttpNotFound();
            }
            return View(restablecer);
        }

        // POST: Restablecer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Restablecer restablecer = await db.Restablecer.FindAsync(id);
            db.Restablecer.Remove(restablecer);
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
