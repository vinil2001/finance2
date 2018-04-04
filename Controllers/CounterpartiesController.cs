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
    //[Authorize]
    public class CounterpartiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities orestDb = new orestEntities();


        // GET: Counterparties
        public ActionResult Index()
        {
            var counterparties = db.Counterparties.Include(c => c.OwneshipType);
            return View(counterparties.ToList());
        }

        // GET: Counterparties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return View(counterparty);
        }

        // GET: Counterparties/Create
        public ActionResult Create()
        {
            ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName");
            return View();
        }

        public ActionResult ImportFromOrestToFinance(int? orestCounterpartyId)
        {
            if (orestCounterpartyId != null)
            {
                klt orestEntity = orestDb.klt.Where(i => i.id == orestCounterpartyId).First();
                if (db.Counterparties.Where(c => c.Name == orestEntity.name).Count() == 0)
                {
                    Counterparty financeEntity = new Counterparty();
                    financeEntity.Name = orestEntity.name;
                    financeEntity.AccountNumber = orestEntity.chet;
                    financeEntity.ActualAddress = orestEntity.adft;
                    financeEntity.BankMFO = orestEntity.mfob;
                    financeEntity.BankName = orestEntity.bank;
                    financeEntity.CodVATPayer = orestEntity.knds;
                    financeEntity.Comment = orestEntity.comt;
                    financeEntity.ContactPerson = orestEntity.cont;
                    financeEntity.Discount = orestEntity.per;
                    financeEntity.EDRPO = orestEntity.okpo;
                    financeEntity.FullName = orestEntity.full;
                    financeEntity.IdOrest = orestEntity.id;
                    db.Counterparties.Add(financeEntity);
                    db.SaveChanges();
                    int financeEntityid = db.Counterparties.Where(c => c.Name == financeEntity.Name).First().Id;
                    return RedirectToAction("Edit", new { id = financeEntityid });
                }
                else
                {
                    ViewBag.SameNameInBothDb = "Компания с таким именем уже существует в базе Finance";
                    return View("Index", db.Counterparties.ToList());
                }

            }
            else
                return View("orestCounterpartyId == null");

        }
        public ActionResult Import()
        {
            var OrestClients = orestDb.klt.ToList();          // клиенты из бызы Orest
            var FinanceClients = db.Counterparties.ToList(); // клиенты из бызы Finance

            //OrestClients.Where - выбрать из базы Orest тех контрагентов, у которыз
            //FinanceClients.Select(b => b.IdOrest).ToList() - получить список из id контрагентов новый базы
            //.Contains(c.id) - проверить - содержи ли этот список (см. выше) id текущего контрагента из старой базы
            //Если нет, он попадёт в список NewClients (в выборка Where).

            var NewClients = OrestClients.Where(c => !FinanceClients.Select(b => b.IdOrest).ToList().Contains(c.id));

            //        db.Customers
            //.Where(c => !db.Blacklists
            //    .Select(b => b.CusId)
            //    .Contains(c.CusId)
            //);

            foreach (var item in NewClients)
            {
                Counterparty tempClient = new Counterparty();
                tempClient.Name = item.name;
                tempClient.AccountNumber = item.chet;
                tempClient.ActualAddress = item.adft;
                tempClient.BankMFO = item.mfob;
                tempClient.BankName = item.bank;
                tempClient.CodVATPayer = item.knds;
                tempClient.Comment = item.comt;
                tempClient.ContactPerson = item.cont;
                tempClient.Discount = item.per;
                tempClient.EDRPO = item.okpo;
                tempClient.FullName = item.full;
                tempClient.IdOrest = item.id;
                db.Counterparties.Add(tempClient);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Counterparties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,FullName,VATPayer,CodVATPayer,VATCertificateNumber,BankName,AccountNumber,BankMFO,EDRPO,LegalAdress,ActualAddress,PhoneNumber,ContactPerson,Comment,Discount,OwneshipTypeId")] Counterparty counterparty)
        {

            if (ModelState.IsValid)
            {
                db.Counterparties.Add(counterparty);
                db.SaveChanges();
                ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName", counterparty.OwneshipTypeId);
                return null;
            }

            ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName", counterparty.OwneshipTypeId);
            return PartialView(counterparty);
        }

        // GET: Counterparties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName", counterparty.OwneshipTypeId);
            return View(counterparty);
        }

        // POST: Counterparties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdOrest,OwneshipTypeId,Name,FullName,VATPayer,CodVATPayer,VATCertificateNumber,BankName,AccountNumber,BankMFO,EDRPO,LegalAdress,ActualAddress,PhoneNumber,ContactPerson,Comment,Discount")] Counterparty counterparty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(counterparty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + counterparty.Id);
            }
            ViewBag.OwneshipTypeId = new SelectList(db.OwnershipTypes, "Id", "OwnershipTypeName", counterparty.OwneshipTypeId);
            return View(counterparty);
        }

        // GET: Counterparties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return View(counterparty);
        }

        // POST: Counterparties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Counterparty counterparty = db.Counterparties.Find(id);
            db.Counterparties.Remove(counterparty);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CounterpartySearch(string request)
        {
            CounterpartySearchUnionModel UnionModel = new CounterpartySearchUnionModel();

            UnionModel.FinanceCounterparties = db.Counterparties.Where(c => c.Name.Contains(request)).Take(100).ToList();

            UnionModel.OrestCounterparties = orestDb.klt.Where(c => c.name.Contains(request)).Take(100).ToList();

            foreach (var i in UnionModel.OrestCounterparties.ToList())
            {
                foreach (var c in UnionModel.FinanceCounterparties.ToList())
                {
                    if (i.id == c.IdOrest)
                    {
                        UnionModel.OrestCounterparties.Remove(i);
                        break;
                    }
                }
            }


            if (UnionModel.OrestCounterparties == null)
                return HttpNotFound("База данных Орест недоступна");

            return PartialView(UnionModel);
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

           


            //          1: Employees e = new Employees();
            //          2:  
            // 3: XmlReader[] sReaders = new XmlReader[]{ XmlReader.Create(
            // 4:     Assembly.GetExecutingAssembly().GetManifestResourceStream("Employees.ssdl"))};
            // 5:  
            // 6: XmlReader[] mReaders = new XmlReader[]{ XmlReader.Create(
            // 7:     Assembly.GetExecutingAssembly().GetManifestResourceStream("Employees.msl"))};
            // 8:  
            // 9: StoreItemCollection sCollection = new StoreItemCollection(sReaders);
            //10: EdmItemCollection cCollection = e.MetadataWorkspace.GetItemCollection(
            //11:     DataSpace.CSpace) as EdmItemCollection;
            //12:  
            //13: StorageMappingItemCollection csCollection =
            //14:     new StorageMappingItemCollection(cCollection, sCollection, mReaders);
            //15:  
            //16: e.MetadataWorkspace.RegisterItemCollection(sCollection);
            //17: e.MetadataWorkspace.RegisterItemCollection(csCollection);
            //18:  
            //19: EntityContainer container =
            //20:     e.MetadataWorkspace.GetItem<EntityContainer>(
            //21:         "TestModelStoreContainer", 
            //22:         DataSpace.SSpace);            
            //23:                     
            //24: EntitySetBase set = container.BaseEntitySets["Person"];
            //25:  
            //26: typeof(EntitySetBase).GetField(
            //27:     "_schema",
            //28:     BindingFlags.NonPublic | BindingFlags.Instance).SetValue(set, "dbo");
            //29:           
            //30: var q = from p in e.Person
            //31:         select p;
            //32:  
            //33: Console.WriteLine(e.Person.ToTraceString());
            //34: Console.WriteLine(q.First().Age);

            //Db.Configuration.ProxyCreationEnabled = false;

            //var Companies = Db.Counterparties.Where(i => i.Name.StartsWith(request)).ToList();
            List<ModelUnionCompany> CompaniesNames = new List<ModelUnionCompany>();
            CompaniesNames.AddRange(from N in db.Counterparties
                                    where N.Name.ToLower().Contains(request.ToLower())
                                    select new ModelUnionCompany() { Name = N.Name, Id = N.Id, TypeDb = 1, IdOrest = N.IdOrest });

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