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
    public class IncomingTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IncomingTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.IncomingTypes.ToListAsync());
        }

        // GET: IncomingTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingType incomingType = await db.IncomingTypes.FindAsync(id);
            if (incomingType == null)
            {
                return HttpNotFound();
            }
            return View(incomingType);
        }

        // GET: IncomingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncomingTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TypeName")] IncomingType incomingType)
        {
            if (ModelState.IsValid)
            {
                db.IncomingTypes.Add(incomingType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(incomingType);
        }

        // GET: IncomingTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingType incomingType = await db.IncomingTypes.FindAsync(id);
            if (incomingType == null)
            {
                return HttpNotFound();
            }
            return View(incomingType);
        }

        // POST: IncomingTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TypeName")] IncomingType incomingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incomingType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(incomingType);
        }

        // GET: IncomingTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingType incomingType = await db.IncomingTypes.FindAsync(id);
            if (incomingType == null)
            {
                return HttpNotFound();
            }
            return View(incomingType);
        }

        // POST: IncomingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            IncomingType incomingType = await db.IncomingTypes.FindAsync(id);
            db.IncomingTypes.Remove(incomingType);
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
