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
using Microsoft.AspNet.Identity.Owin;

namespace Finance.Controllers
{
    public class PaymentStatementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentStatements
        public async Task<ActionResult> Index()
        {
            return View(await db.PaymentStatements.ToListAsync());
        }

        // GET: PaymentStatements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStatement paymentStatement = await db.PaymentStatements.FindAsync(id);
            if (paymentStatement == null)
            {
                return HttpNotFound();
            }
            return View(paymentStatement);
        }

        // GET: PaymentStatements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentStatements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,KltId,InvoiceNumber,InvoiceDate,InvoiceSumma,Comment,InvoiceChecked,PaymentApproved,PaymentDone,DocumentUrl")] PaymentStatement paymentStatement, HttpPostedFileBase UploadDocumentUrl)
        {
            if (ModelState.IsValid)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (paymentStatement.PaymentApproved == true && User.IsInRole("Sign1"))
                {
                    paymentStatement.PaymentApprovedUser = await UserManager.FindByNameAsync(User.Identity.Name);
                }
                else
                {
                    paymentStatement.PaymentApproved = false;
                }

                if (UploadDocumentUrl != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(UploadDocumentUrl.FileName);
                    // сохраняем файл в папку Files в проекте
                    UploadDocumentUrl.SaveAs(Server.MapPath("~/Files/" + fileName));
                    paymentStatement.DocumentUrl = "~/Files/" + fileName;
                }
             

                db.PaymentStatements.Add(paymentStatement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentStatement);
        }

        // GET: PaymentStatements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStatement paymentStatement = await db.PaymentStatements.FindAsync(id);
            if (paymentStatement == null)
            {
                return HttpNotFound();
            }
            return View(paymentStatement);
        }

        // POST: PaymentStatements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,KltId,InvoiceNumber,InvoiceDate,InvoiceSumma,Comment,InvoiceChecked,PaymentApproved,PaymentDone,DocumentUrl")] PaymentStatement paymentStatement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentStatement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentStatement);
        }

        // GET: PaymentStatements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStatement paymentStatement = await db.PaymentStatements.FindAsync(id);
            if (paymentStatement == null)
            {
                return HttpNotFound();
            }
            return View(paymentStatement);
        }

        // POST: PaymentStatements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentStatement paymentStatement = await db.PaymentStatements.FindAsync(id);
            db.PaymentStatements.Remove(paymentStatement);
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
