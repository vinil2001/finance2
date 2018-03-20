using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Finance.Models;

namespace Finance.Controllers
{
    public class IncomingCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IncomingCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.IncomingCategorys.ToListAsync());
        }

        // GET: IncomingCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCategory incomingCategory = await db.IncomingCategorys.FindAsync(id);
            if (incomingCategory == null)
            {
                return HttpNotFound();
            }
            return View(incomingCategory);
        }

        // GET: IncomingCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncomingCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IncomingCategoryName")] IncomingCategory incomingCategory)
        {
            if (ModelState.IsValid)
            {
                db.IncomingCategorys.Add(incomingCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(incomingCategory);
        }

        // GET: IncomingCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCategory incomingCategory = await db.IncomingCategorys.FindAsync(id);
            if (incomingCategory == null)
            {
                return HttpNotFound();
            }
            return View(incomingCategory);
        }

        // POST: IncomingCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IncomingCategoryName")] IncomingCategory incomingCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incomingCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(incomingCategory);
        }

        // GET: IncomingCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCategory incomingCategory = await db.IncomingCategorys.FindAsync(id);
            if (incomingCategory == null)
            {
                return HttpNotFound();
            }
            return View(incomingCategory);
        }

        // POST: IncomingCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            IncomingCategory incomingCategory = await db.IncomingCategorys.FindAsync(id);
            db.IncomingCategorys.Remove(incomingCategory);
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
