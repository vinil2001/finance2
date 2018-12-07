using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Finance.Models;

namespace Finance.Controllers
{
    public class RemovedCounterpartiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities OrestDb = new orestEntities();

        // GET: RemovedCounterparties
        public ActionResult Index()
        {
            return View(db.RemovedCounterpartys.ToList());
        }

        // GET: RemovedCounterparties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemovedCounterparty removedCounterparty = db.RemovedCounterpartys.Find(id);
            if (removedCounterparty == null)
            {
                return HttpNotFound();
            }
            removedCounterparty.RemovedKlt = OrestDb.klt.Find(removedCounterparty.OrestCounterpartyId);
            return View(removedCounterparty);
        }

        public ActionResult RestoreFromArchive(int id)
        {
            var restoredCompany =  db.RemovedCounterpartys.Find(id);
            if(restoredCompany != null)
            {
                db.RemovedCounterpartys.Remove(restoredCompany);
                db.SaveChanges();
            }
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
