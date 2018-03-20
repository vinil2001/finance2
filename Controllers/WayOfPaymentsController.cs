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
    public class WayOfPaymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WayOfPayments
        public async Task<ActionResult> Index()
        {
            return View(await db.WayOfPayments.ToListAsync());
        }

        // GET: WayOfPayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayOfPayment wayOfPayment = await db.WayOfPayments.FindAsync(id);
            if (wayOfPayment == null)
            {
                return HttpNotFound();
            }
            return View(wayOfPayment);
        }

        // GET: WayOfPayments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WayOfPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,WayOfPaymentName")] WayOfPayment wayOfPayment)
        {
            if (ModelState.IsValid)
            {
                db.WayOfPayments.Add(wayOfPayment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(wayOfPayment);
        }

        // GET: WayOfPayments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayOfPayment wayOfPayment = await db.WayOfPayments.FindAsync(id);
            if (wayOfPayment == null)
            {
                return HttpNotFound();
            }
            return View(wayOfPayment);
        }

        // POST: WayOfPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,WayOfPaymentName")] WayOfPayment wayOfPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wayOfPayment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(wayOfPayment);
        }

        // GET: WayOfPayments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayOfPayment wayOfPayment = await db.WayOfPayments.FindAsync(id);
            if (wayOfPayment == null)
            {
                return HttpNotFound();
            }
            return View(wayOfPayment);
        }

        // POST: WayOfPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WayOfPayment wayOfPayment = await db.WayOfPayments.FindAsync(id);
            db.WayOfPayments.Remove(wayOfPayment);
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
