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
    //[Authorize]
    public class IncomingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities OrestDb = new orestEntities();

        // GET: Incomings
        public ActionResult Index()
        {
            //var incomings = db.Incomings.Include(i => i.Bank).Include(i => i.Counterparty).Include(i => i.IncomingCategory).Include(i => i.WayOfPayment);
            //return View(incomings.ToList());
            return View(db.Incomings.ToList());
        }

        // GET: Incomings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incoming incoming = db.Incomings.Find(id);
            if (incoming == null)
            {
                return HttpNotFound();
            }
            return View(incoming);
        }

        // GET: Incomings/Create
        public ActionResult Create()
        {
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName");
            ViewBag.CounterpartyId = new SelectList(db.Counterparties, "Id", "Name");
            ViewBag.IncomingCategoryId = new SelectList(db.IncomingCategorys, "Id", "IncomingCategoryName");
            ViewBag.WayOfPaymentId = new SelectList(db.WayOfPayments, "Id", "WayOfPaymentName");
            ViewBag.IncomingTypeId = new SelectList(db.IncomingTypes, "Id", "TypeName");
            return View();
        }

        // POST: Incomings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncomingData,BankId,CounterpartyId,IncomingSum,IncomingCategoryId,IncomingTypeId,InvoiceNumber,InvoiceData,WayOfPaymentId")] Incoming incoming, int? CounterpartyTypeDb)
        {
            //ModelState.Remove("CounterpartyId");
            //if (M)
            //{
            //    ViewBag.ErrorMessage = "Поле 'Плательщик' должно быть установлено";
            //}
            if (ModelState.IsValid)
            {
                if(CounterpartyTypeDb == 0) // dbOrest
                {
                    var orestClient = OrestDb.klt.Find(incoming.CounterpartyId);

                    Counterparty tempClient = new Counterparty();
                    tempClient.Name = orestClient.name;
                    tempClient.AccountNumber = orestClient.chet;
                    tempClient.ActualAddress = orestClient.adft;
                    tempClient.BankMFO = orestClient.mfob;
                    tempClient.BankName = orestClient.bank;
                    tempClient.CodVATPayer = orestClient.knds;
                    tempClient.VATCertificateNumber = orestClient.snds;
                    tempClient.Comment = orestClient.comt;
                    tempClient.ContactPerson = orestClient.cont;
                    tempClient.Discount = orestClient.per;
                    tempClient.EDRPO = orestClient.okpo;
                    tempClient.FullName = orestClient.full;
                    tempClient.IdOrest = orestClient.id;
                    tempClient.VATPayer = !Convert.ToBoolean(orestClient.nds);

                    db.Counterparties.Add(tempClient);

                    incoming.Counterparty = tempClient;
                  
                }
                db.Incomings.Add(incoming);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", incoming.Bank);
            ViewBag.CounterpartyId = new SelectList(db.Counterparties, "Id", "Name", incoming.CounterpartyId);
            ViewBag.IncomingCategoryId = new SelectList(db.IncomingCategorys, "Id", "IncomingCategoryName", incoming.IncomingCategoryId);
            ViewBag.WayOfPaymentId = new SelectList(db.WayOfPayments, "Id", "WayOfPaymentName");
            ViewBag.IncomingTypeId = new SelectList(db.IncomingTypes, "Id", "TypeName");
            return View(incoming);
        }
        public void GetCounterpartyName (string CounterpartyNameVal)
        {
            ViewBag.CounterpartyName = CounterpartyNameVal;
            TempData["CounterpartyName"] = CounterpartyNameVal;
        }
        // GET: Incomings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditIncomingModel CurrentEditIncomingModel = new EditIncomingModel(db.Incomings.Find(id));

            if (CurrentEditIncomingModel.EditedIncoming == null)
            {
                return HttpNotFound();
            }
            return View(CurrentEditIncomingModel);
        }

        // POST: Incomings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditIncomingModel incoming, int? CounterpartyTypeDb)
        {
            if (ModelState.IsValid)
            {
                if (CounterpartyTypeDb == 0) // dbOrest
                {
                    var orestClient = OrestDb.klt.Find(incoming.EditedIncoming.CounterpartyId);

                    Counterparty tempClient = new Counterparty();
                    tempClient.Name = orestClient.name;
                    tempClient.AccountNumber = orestClient.chet;
                    tempClient.ActualAddress = orestClient.adft;
                    tempClient.BankMFO = orestClient.mfob;
                    tempClient.BankName = orestClient.bank;
                    tempClient.CodVATPayer = orestClient.knds;
                    tempClient.VATCertificateNumber = orestClient.snds;
                    tempClient.Comment = orestClient.comt;
                    tempClient.ContactPerson = orestClient.cont;
                    tempClient.Discount = orestClient.per;
                    tempClient.EDRPO = orestClient.okpo;
                    tempClient.FullName = orestClient.full;
                    tempClient.IdOrest = orestClient.id;
                    tempClient.VATPayer = !Convert.ToBoolean(orestClient.nds);

                    db.Counterparties.Add(tempClient);

                    incoming.EditedIncoming.Counterparty = tempClient;

                    db.Entry(incoming.EditedIncoming).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    incoming.EditedIncoming.Counterparty = db.Counterparties.Find(incoming.EditedIncoming.CounterpartyId);
                    db.Entry(incoming.EditedIncoming).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //incoming.EditedIncoming.Counterparty = db.Counterparties.Find(incoming.EditedIncoming.CounterpartyId);
                //if (incoming.EditedIncoming.Counterparty == null)
                //{
                //    //var id = incoming.EditedIncoming.CounterpartyId;
                //    var orestClient = OrestDb.klt.Find(incoming.EditedIncoming.CounterpartyId);

                //    Counterparty tempClient = new Counterparty();
                //    tempClient.Name = orestClient.name;
                //    tempClient.AccountNumber = orestClient.chet;
                //    tempClient.ActualAddress = orestClient.adft;
                //    tempClient.BankMFO = orestClient.mfob;
                //    tempClient.BankName = orestClient.bank;
                //    tempClient.CodVATPayer = orestClient.knds;
                //    tempClient.Comment = orestClient.comt;
                //    tempClient.ContactPerson = orestClient.cont;
                //    tempClient.Discount = orestClient.per;
                //    tempClient.EDRPO = orestClient.okpo;
                //    tempClient.FullName = orestClient.full;
                //    tempClient.IdOrest = orestClient.id;

                //    db.Counterparties.Add(tempClient);

                //    incoming.EditedIncoming.Counterparty = tempClient;
                //    db.Entry(incoming.EditedIncoming).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");

                //}
                //else
                //{
                //    db.Entry(incoming.EditedIncoming).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}       
            }
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", incoming.EditedIncoming.Bank);
            ViewBag.CounterpartyId = new SelectList(db.Counterparties, "Id", "Name", incoming.EditedIncoming.CounterpartyId);
            ViewBag.IncomingCategoryId = new SelectList(db.IncomingCategorys, "Id", "IncomingCategoryName", incoming.EditedIncoming.IncomingCategoryId);
            ViewBag.WayOfPaymentId = new SelectList(db.WayOfPayments, "Id", "WayOfPaymentName", incoming.EditedIncoming.WayOfPaymentId);
            ViewBag.IncomingTypeId = new SelectList(db.IncomingTypes, "Id", "TypeName", incoming.EditedIncoming.IncomingTypeId);
            ViewBag.WayOfPaymentId = new SelectList(db.WayOfPayments, "Id", "WayOfPaymentName", incoming.EditedIncoming.WayOfPaymentId);
            return View(incoming);
        }

        // GET: Incomings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incoming incoming = db.Incomings.Find(id);
            if (incoming == null)
            {
                return HttpNotFound();
            }
            return View(incoming);
        }

        // POST: Incomings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incoming incoming = db.Incomings.Find(id);
            db.Incomings.Remove(incoming);
            db.SaveChanges();
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

        public ActionResult OrderBy(int triger, string orderBy)
        {
            if (triger == 0)
            {
                ViewBag.triger = 1;
                if (orderBy == "IncomingData")
                    return View("Index", db.Incomings.ToList().OrderBy(i => i.IncomingData));
                else if (orderBy == "Counterparty")
                    return View("Index", db.Incomings.ToList().OrderBy(i => i.Counterparty.Name));
                else if (orderBy == "InvoiceData")
                    return View("Index", db.Incomings.ToList().OrderBy(i => i.InvoiceData));
                //else if (orderBy == "BankName")
                //    return View("Index", db.Incomings.ToList().Where(i => i.Bank != null).OrderBy(i => i.Bank.BankName));
                else if (orderBy == "IncomingCategoryName")
                    return View("Index", db.Incomings.ToList().OrderBy(i => i.IncomingCategory.IncomingCategoryName));
                else if (orderBy == "WayOfPaymentName")
                    return View("Index", db.Incomings.ToList().OrderBy(i => i.WayOfPayment.WayOfPaymentName));
                else
                    return View("Index");
            }
            else
            {
                ViewBag.triger = 0;
                if (orderBy == "IncomingData")
                    return View("Index", db.Incomings.ToList().OrderByDescending(i => i.IncomingData));
                else if (orderBy == "Counterparty")
                    return View("Index", db.Incomings.ToList().OrderByDescending(i => i.Counterparty.Name));
                else if (orderBy == "InvoiceData")
                    return View("Index", db.Incomings.ToList().OrderByDescending(i => i.InvoiceData));
                else if (orderBy == "IncomingCategoryName")
                    return View("Index", db.Incomings.ToList().OrderByDescending(i => i.IncomingCategory.IncomingCategoryName));
                else if (orderBy == "WayOfPaymentName")
                    return View("Index", db.Incomings.ToList().OrderByDescending(i => i.WayOfPayment.WayOfPaymentName));
                else
                    return View("Index");
            }

        }

        public ActionResult GroupBy(string orderBy)
        {
                return View("Index", db.Incomings.GroupBy(i => i.Bank).SelectMany(r => r).ToList());
             

        }
      
    }
}
