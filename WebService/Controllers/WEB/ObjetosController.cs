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
            var objetos = db.Objetos.Include(o => o.Categorias);
            return View(await objetos.ToListAsync());
        }

        // GET: Objetos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objeto objeto = await db.Objetos.FindAsync(id);
            if (objeto == null)
            {
                return HttpNotFound();
            }
            return View(objeto);
        }

        // GET: Objetos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriasID = new SelectList(db.Categorias, "ID", "Nombre");
            return View();
        }

        // POST: Objetos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CategoriasID,Nombre,Imagen")] Objeto objeto)
        {
            if (ModelState.IsValid)
            {
                db.Objetos.Add(objeto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriasID = new SelectList(db.Categorias, "ID", "Nombre", objeto.CategoriasID);
            return View(objeto);
        }

        // GET: Objetos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objeto objeto = await db.Objetos.FindAsync(id);
            if (objeto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriasID = new SelectList(db.Categorias, "ID", "Nombre", objeto.CategoriasID);
            return View(objeto);
        }

        // POST: Objetos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CategoriasID,Nombre,Imagen")] Objeto objeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(objeto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriasID = new SelectList(db.Categorias, "ID", "Nombre", objeto.CategoriasID);
            return View(objeto);
        }

        // GET: Objetos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objeto objeto = await db.Objetos.FindAsync(id);
            if (objeto == null)
            {
                return HttpNotFound();
            }
            return View(objeto);
        }

        // POST: Objetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Objeto objeto = await db.Objetos.FindAsync(id);
            db.Objetos.Remove(objeto);
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
