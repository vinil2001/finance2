using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Finance.Models;

namespace Finance.Controllers
{
    [Authorize]
    public class CounterpartiesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities orestDb = new orestEntities();

        // GET: Counterparties
        public ActionResult Index()
        {
            List<RemovedCounterparty> RemovedCompanies = db.RemovedCounterpartys.ToList();

            List <klt> OrestCounterparties = orestDb.klt.Where(i=>i.idp != 0 && i.idp != 1 && i.idp != 2975 && i.name != "").OrderByDescending(a => a.id).Take(100).ToList();

            OrestCounterparties.RemoveAll(el1 => RemovedCompanies.Exists(el2 => el2.OrestCounterpartyId == el1.id));

            if (TempData["MessageAddToArchive"] != null)
                ViewBag.MessageAddToArchiveIndex = TempData["MessageAddToArchive"];
            return View(OrestCounterparties);
        }

        // GET: Counterparties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            klt counterparty = orestDb.klt.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return View(counterparty);
        }

        protected int addNewCounterpartyToOrestDb(klt counterparty)
        {
            counterparty.idp = 15;
            counterparty.sld = 0;
            counterparty.grp = 0;

            //tempClient.id = counterparty.IdOrest;

            //tempClient.nds = Convert.ToInt32(counterparty.VATPayer);

            orestDb.klt.Add(counterparty);
            orestDb.SaveChanges();
            return Convert.ToInt32(counterparty.id);
        }

        class Nds
        {
            public int Value;
            public string Text;
        }
        // GET: Counterparties/Create
        public ActionResult Create()
        {
            Nds Nds = new Nds();
            List<Nds> NdsVariantsList = new List<Nds>();
            NdsVariantsList.Add(new Nds() { Value = 0, Text = "Нет" });
            NdsVariantsList.Add(new Nds() { Value = 1, Text = "Да" });
            SelectList nds = new SelectList(NdsVariantsList, "Value", "Text");

            ViewBag.nds = nds;
            return View();
        }

        // POST: Counterparties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(klt counterparty)
        {
            //counterparty.nds = ndsVal;
            if (ModelState.IsValid)
            {
                int addedCounterparyId = addNewCounterpartyToOrestDb(counterparty);
                return Json(new { Id = addedCounterparyId, Name = counterparty.name });
            }
            return Json(counterparty);
        }

        // GET: Counterparties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Counterparty counterparty = db.Counterparties.Find(id);
            var OrestKlt = orestDb.klt.Find(id);
            if (OrestKlt == null)
            {
                return HttpNotFound();
            }

       

            //ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName", counterparty.OwneshipTypeId);

            return View(OrestKlt);
        }

        // POST: Counterparties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(klt counterparty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(counterparty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + counterparty.id);
            }
 
            return View(counterparty);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            klt counterparty = orestDb.klt.Find(id);
            return View(counterparty);
        }


        public ActionResult AddToArchive(int id)
        {
            try
            {
                RemovedCounterparty removedCounterparty = new RemovedCounterparty();
                //removedCounterparty.RemovedKlt = orestDb.klt.Find(id);
                removedCounterparty.OrestCounterpartyName = orestDb.klt.Find(id).name;
                removedCounterparty.OrestCounterpartyId = orestDb.klt.Find(id).id;
                removedCounterparty.WhoRemoveIt = User.Identity.Name;
                removedCounterparty.WhenRemoved = DateTime.Now;
                db.RemovedCounterpartys.Add(removedCounterparty);
                db.SaveChanges();
                TempData["MessageAddToArchive"] ="Компания " + orestDb.klt.Find(id).name + " успешно удалена в архив.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageAddToArchive"] = "Ошибка удаления компании " + ex.Message;
                return RedirectToAction("Index");
            }
            
        }

        // GET: Counterparties/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Counterparty counterparty = db.Counterparties.Find(id);
        //    if (counterparty == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(counterparty);
        //}

        //// POST: Counterparties/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Counterparty counterparty = db.Counterparties.Find(id);
        //    db.Counterparties.Remove(counterparty);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult CounterpartySearch(string request)
        {
            List<klt> OrestCounterparties = orestDb.klt.Where(i => i.name.Contains(request)).OrderByDescending(a => a.id).Take(100).ToList();

            return View(OrestCounterparties);
            //if (OrestCounterparties.Count() != 0)
            //    return View(OrestCounterparties);
            //else
            //    return View("Совпадений не найдено.");
        }

        public class ModelUnionCompany
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int TypeDb { get; set; }
            public long? IdOrest { get; set; }
        }

        public JsonResult SearchAutocomplete(string request)
        {

            //Db.Configuration.ProxyCreationEnabled = false;

            //var Companies = Db.Counterparties.Where(i => i.Name.StartsWith(request)).ToList();

            List<ModelUnionCompany> CompaniesNames = new List<ModelUnionCompany>();

            //Исключил запрос к db поскольку все компании беруться и добавляются только в orestDb
            //CompaniesNames.AddRange(from N in db.Counterparties
            //                        where N.Name.ToLower().Contains(request.ToLower())
            //                        select new ModelUnionCompany() { Name = N.Name, Id = N.Id, TypeDb = 1, IdOrest = N.IdOrest });
            ////Исключил запрос к db поскольку все компании беруться и добавляются только в orestDb

            //var oldCompanies = dbOrest.Database.SqlQuery<ModelUnionCompany>("SELECT id, name FROM   klt WHERE(name LIKE '%"+ request  + "%')");


            //CompaniesNames.AddRange(from N in dbOrest.klt.ToList().Where(c => !CompaniesNames.Select(b => b.IdOrest).ToList().Contains(c.id))
            //                        where N.name.Contains(request.Substring(0))
            //                        select new ModelUnionCompany() { Name = N.name, Id = N.id, TypeDb = 0 });


            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(@"SELECT id, name
            FROM   klt
            WHERE (name LIKE '%" + request + "%')");

            foreach (var i in CompaniesNames)
            {
                if (i.IdOrest != null)
                    queryBuilder.Append(" and id <> " + i.IdOrest);
            }
            var sqlRequestResult = orestDb.Database.SqlQuery<ModelUnionCompany>(queryBuilder.ToString()).ToList();
            // проверяем содержит ли данные результат SQL запроса
            if (sqlRequestResult.Count() != 0)
                CompaniesNames.AddRange(sqlRequestResult);

            //            CompaniesNames.AddRange(dbOrest.Database.SqlQuery<ModelUnionCompany>(@"SELECT id, name
            //FROM   klt
            //WHERE (name LIKE '%" + request + "%')"));

            //            var oldCompanies = dbOrest.Database.SqlQuery<ModelUnionCompany>("SELECT id, name FROM  klt WHERE(name LIKE '%" + request + "%')");

            //CompaniesNames.AddRange(from N in dbOrest.klt.ToList().Where(c => !CompaniesNames.Select(b => b.IdOrest).Contains(c.id))
            //                        where N.name.Contains(request)
            //                        select new ModelUnionCompany() { Name = N.name, Id = N.id, TypeDb = 0 });

            //var oldCompany = from N in dbOrest.klt
            //                 select new ModelUnionCompany() { Name = N.name, Id = N.id, TypeDb = 0 };


            //CompaniesNames.AddRange(from N in oldCompany.ToList()
            //                        where N.Name.ToLower().Contains(request.ToLower().Trim())
            //                        select new ModelUnionCompany() { Name = N.Name, Id = N.Id, TypeDb = 0 });

            return Json(CompaniesNames.Take(100), JsonRequestBehavior.AllowGet);
        }

        public ActionResult StepBack()
        {
            var stepBack = Request.UrlReferrer.ToString();
            return Redirect(Request.UrlReferrer.ToString());
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