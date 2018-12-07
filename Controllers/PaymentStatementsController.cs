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
using System.Data.Entity.Validation;

namespace Finance.Controllers
{
    [Authorize]
    public class PaymentStatementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities OrestDb = new orestEntities();

        //private ApplicationUserManager _userManager;
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        // GET: PaymentStatements
        public async Task<ActionResult> Index(List<PaymentStatement> SortedByPaymentList = null)
        {
            if (SortedByPaymentList != null)
            {
                return View(SortedByPaymentList);
            }
            else
            {
                List<PaymentStatement> lastPayments = await db.PaymentStatements.OrderByDescending(i => i.Id).Take(100).ToListAsync();
                var lastElementId = lastPayments.Last().Id;
                List<PaymentStatement> unpaidPayments = await db.PaymentStatements.Where(i => i.PaymentDone == false && i.Id < lastElementId).ToListAsync();
                
                //foreach(var i in unpaidPayments)
                //{
                //    if (lastPayments.Find(j => j.Id == i.Id) != null)
                //    {
                //        unpaidPayments.Remove(i);
                //    }
                //}


                List<PaymentStatement> paymentsPlusUnpaidPayments = new List<PaymentStatement>();
                paymentsPlusUnpaidPayments.AddRange(lastPayments);
                paymentsPlusUnpaidPayments.AddRange(unpaidPayments);
                

                return View(paymentsPlusUnpaidPayments);
                //return View(await db.PaymentStatements.OrderBy(i => i.Id).Take(100).ToListAsync());
            }

        }

        public ActionResult PaymentsByCompany(int companyId)
        {
            return View(db.PaymentStatements.Where(c => c.KltId == companyId).ToList().OrderByDescending(i => i.Id));
        }

        public ActionResult sortByPayment()
        {
            return View("Index", db.PaymentStatements.OrderBy(i => i.PaymentDone).ThenByDescending(j => j.Id).Take(100).ToList());
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
            PaymentStatement payment = new PaymentStatement();
            payment.Payments = new List<Payment>();
            payment.DocumentUrl = "Открыть";
            payment.InvoiceDate = DateTime.Now;
            payment.Currency = new Currency();
            payment.Currency.Id = 1;  //UAH
            payment.Currency.CurrencyName = "UAH";
            ViewBag.CurrenciesList = new SelectList(db.Currencies, "Id", "CurrencyName");
            return View(payment);
        }
        // POST: PaymentStatements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,KltId,InvoiceNumber,InvoiceDate,InvoiceSumma,Comment,InvoiceChecked,PaymentApproved,DocumentUrl,CurrencyId")]
        PaymentStatement paymentStatement, decimal[] Summa, string[] PaymentComment, bool[] PaymentPaymentDone, HttpPostedFileBase[] UploadDocumentUrl)
        {        
            if (paymentStatement.KltId == 0)
            {
                ModelState.AddModelError("Counterparty_Id", "Не выбран контрагент");
                if (UploadDocumentUrl != null)
                {
                    //ViewBag.FileName = System.IO.Path.GetFileName(UploadDocumentUrl.FileName);
                }
            }            
            
            if (ModelState.IsValid)
            {
                var currentUser = db.Users.Where(a => a.UserName == User.Identity.Name).First();
                if (UploadDocumentUrl != null)
                {
                    //// получаем имя файла
                    //string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(UploadDocumentUrl.FileName);
                    //// сохраняем файл в папку Files в проекте
                    //UploadDocumentUrl.SaveAs(Server.MapPath("~/Files/" + fileName));
                    //paymentStatement.DocumentUrl = "/Files/" + fileName;
                    foreach (var file in UploadDocumentUrl)
                    {
                        // получаем имя файла
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(file.FileName);
                        // сохраняем файл в папку Files в проекте
                        file.SaveAs(Server.MapPath("~/Files/" + fileName));
                        PaymentsDocument paymentDocument = new PaymentsDocument();
                        paymentDocument.DocumentUrl = "/Files/" + fileName;
                        if (paymentStatement.PaymentsDocuments == null)
                          paymentStatement.PaymentsDocuments = new List<PaymentsDocument>();
                        paymentStatement.PaymentsDocuments.Add(paymentDocument);
                    }
                }

                if (paymentStatement.PaymentApproved == true && (User.IsInRole("Sign1") || User.IsInRole("Developer")))
                {
                    paymentStatement.PaymentApprovedUser = currentUser;
                }
                else
                {
                    paymentStatement.PaymentApproved = false;
                }

                paymentStatement.whoAddThis = currentUser; //await UserManager.FindByNameAsync(User.Identity.Name); 
                paymentStatement.whoMadeLastChanges = currentUser;
                paymentStatement.WhenEdited = DateTime.Now;
                paymentStatement.Currency = db.Currencies.Find(paymentStatement.CurrencyId);

                //Payment payment = new Payment();
                //payment.InvoiceChecked = paymentStatement.InvoiceChecked;
                //payment.PaymentApproved = paymentStatement.PaymentApproved;
                //payment.PaymentPaymentDone = paymentStatement.PaymentPaymentDone;
                //payment.InvoiceCheckedUser = paymentStatement.InvoiceCheckedUser;
                //payment.PaymentApprovedUser = paymentStatement.PaymentApprovedUser;
                //payment.PaymentPaymentDoneUser = paymentStatement.PaymentPaymentDoneUser;
                //payment.Summa = paymentStatement.InvoiceSumma;
                //paymentStatement.Payments = new List<Payment>();
                //paymentStatement.Payments.Add(payment);
                //db.PaymentStatements.Add(paymentStatement);

                paymentStatement.Payments = new List<Payment>();
                int PaymentDoneIndex = 0;
                int CommentIndex = 0;
                if (Summa != null)
                {
                    foreach (var sum in Summa)
                    {
                        Payment payment = new Payment();

                        payment.Summa = sum;

                        payment.PaymentComment = PaymentComment[CommentIndex];
                        CommentIndex++;

                        payment.PaymentPaymentDoneUser = currentUser;
                        payment.PaymentWhoAddThis = currentUser;
                        payment.PaymentWhenEdited = DateTime.Now;
                        payment.PaymentwhoMadeLastChanges = currentUser;

                        payment.PaymentPaymentDone = PaymentPaymentDone[PaymentDoneIndex];
                        if (payment.PaymentPaymentDone == false)
                            PaymentDoneIndex++;
                        else
                            PaymentDoneIndex += 2;

                        paymentStatement.Payments.Add(payment);
                    }
                }
                db.PaymentStatements.Add(paymentStatement);
                db.SaveChanges();

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
            ViewBag.CurrenciesList = new SelectList(db.Currencies, "Id", "CurrencyName");
            return View(paymentStatement);
        }

        // POST: PaymentStatements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KltId,InvoiceNumber,InvoiceDate,InvoiceSumma,Comment,InvoiceChecked,PaymentApproved,DocumentUrl, CurrencyId")]
        PaymentStatement paymentStatement, decimal[] Summa, string[] PaymentComment, bool[] PaymentPaymentDone, int[] UploadDocumentId, HttpPostedFileBase[] UploadDocumentUrl, string LastUrl)
        {
           
          
            if (ModelState.IsValid)
            {
                PaymentStatement currentPaymentStatement = db.PaymentStatements.Find(paymentStatement.Id);
                currentPaymentStatement.Comment = paymentStatement.Comment;
                currentPaymentStatement.KltId = paymentStatement.KltId;
                currentPaymentStatement.InvoiceNumber = paymentStatement.InvoiceNumber;
                currentPaymentStatement.InvoiceSumma = paymentStatement.InvoiceSumma;
                currentPaymentStatement.InvoiceChecked = paymentStatement.InvoiceChecked;
                currentPaymentStatement.InvoiceDate = paymentStatement.InvoiceDate;
                currentPaymentStatement.PaymentApproved = paymentStatement.PaymentApproved;
                currentPaymentStatement.WhenEdited = DateTime.Now;
               
                // Проверяем были ли ранее загружены файлы (при Create)

                if (currentPaymentStatement.PaymentsDocuments != null && currentPaymentStatement.PaymentsDocuments.Count != 0) 
                {
                  
                    db.SaveChanges();
                }

                //2.Сохраняем ново добавленные файлы из UploadDocumentUrl
                if (UploadDocumentUrl != null)
                {
                    foreach (var file in UploadDocumentUrl)
                    {
                        // получаем имя файла
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(file.FileName);
                        // сохраняем файл в папку Files в проекте
                        file.SaveAs(Server.MapPath("~/Files/" + fileName));
                        PaymentsDocument paymentDocument = new PaymentsDocument();
                        paymentDocument.DocumentUrl = "/Files/" + fileName;
                        if (currentPaymentStatement.PaymentsDocuments == null)
                            currentPaymentStatement.PaymentsDocuments = new List<PaymentsDocument>();
                        currentPaymentStatement.PaymentsDocuments.Add(paymentDocument);
                    }

                }

                var currentUser = db.Users.Where(a => a.UserName == User.Identity.Name).First();

                /*paymentStatement.whoAddThis = currentUser;*/ //await UserManager.FindByNameAsync(User.Identity.Name); 
                currentPaymentStatement.whoMadeLastChanges = currentUser;
                currentPaymentStatement.WhenEdited = DateTime.Now;
                currentPaymentStatement.Currency = db.Currencies.Find(paymentStatement.CurrencyId);

                currentPaymentStatement.Payments = db.Payments.Where(a => a.PaymentStatementId == paymentStatement.Id).ToList();
               
                db.Payments.RemoveRange(currentPaymentStatement.Payments);
                db.SaveChanges();


                int PaymentDoneIndex = 0;
                int CommentIndex = 0;
                if (currentPaymentStatement.Payments == null)
                    currentPaymentStatement.Payments = new List<Payment>();
                //db.Entry(paymentStatement).State = EntityState.Modified;
                //db.SaveChanges();

                if (Summa != null)
                {
                    foreach (var sum in Summa)
                    {
                        Payment payment = new Payment();

                        payment.Summa = sum;

                        payment.PaymentComment = PaymentComment[CommentIndex];
                        CommentIndex++;

                        payment.PaymentPaymentDoneUser = currentUser;
                        payment.PaymentWhoAddThis = currentUser;
                        payment.PaymentWhenEdited = DateTime.Now;
                        payment.PaymentwhoMadeLastChanges = currentUser;

                        payment.PaymentPaymentDone = PaymentPaymentDone[PaymentDoneIndex];
                        if (payment.PaymentPaymentDone == false)
                            PaymentDoneIndex++;
                        else
                            PaymentDoneIndex += 2;
                        currentPaymentStatement.Payments.Add(payment);
                    }

                    decimal PaymentsSumm = 0; // расчитываем сумму оплат
                    foreach (var i in currentPaymentStatement.Payments.ToList())
                    {
                        if (i.PaymentPaymentDone == true)
                        {
                            PaymentsSumm += i.Summa;
                            currentPaymentStatement.PaymentDone = true;
                        }
                        else
                            currentPaymentStatement.PaymentDone = false;
                    }

                    if (PaymentsSumm == currentPaymentStatement.InvoiceSumma)
                    {
                        currentPaymentStatement.PaymentDone = true;
                    }
                    else
                        currentPaymentStatement.PaymentDone = false;
                }
             
                db.Entry(currentPaymentStatement).State = EntityState.Modified;

  

                //paymentStatement.DocumentUrl = db.PaymentStatements.Find(paymentStatement.Id).DocumentUrl;
                //db.Entry(paymentStatement).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction(LastUrl != "" ? LastUrl : "Index");

            }
            return View(paymentStatement);
        }

        public PartialViewResult _PartialPartOfPayment(int? id)
        {
            Payment payment;
            if (id == null)
                payment = new Payment();
            else
                payment = db.Payments.Find(id);
            return PartialView(payment);
        }

        public ActionResult GetFileUrl(int? id)
        {
            PaymentStatement paymentStatement = db.PaymentStatements.Find(id); ;
            //return Redirect(paymentStatement.DocumentUrl.ToString());
            var documents = paymentStatement.PaymentsDocuments;
            string[] allFilesUrl = new string[documents.Count()];
            int index = 0;
            foreach (var i in documents)
            {
                allFilesUrl[index] = i.DocumentUrl;
                index++;
            }
            return Redirect(allFilesUrl[0]);

            // Пример из https://stackoverflow.com/questions/12713710/returning-multiple-files-from-mvc-action
            //var outputStream = new MemoryStream();

            //using (var zip = new ZipFile())
            //{
            //    zip.AddEntry("file1.txt", "content1");
            //    zip.AddEntry("file2.txt", "content2");
            //    zip.Save(outputStream);
            //}

            //outputStream.Position = 0;
            //return File(outputStream, "application/zip", "filename.zip");

        }

        public ActionResult FileViewer(int? id)
        {
            PaymentStatement paymentStatement = db.PaymentStatements.Find(id); ;
            //return Redirect(paymentStatement.DocumentUrl.ToString());
            var documents = paymentStatement.PaymentsDocuments;
            string[] allFilesUrl = new string[documents.Count()];
            int index = 0;
            foreach (var i in documents)
            {
                allFilesUrl[index] = i.DocumentUrl;
                index++;
            }
            return View(allFilesUrl);

            // Пример из https://stackoverflow.com/questions/12713710/returning-multiple-files-from-mvc-action
            //var outputStream = new MemoryStream();

            //using (var zip = new ZipFile())
            //{
            //    zip.AddEntry("file1.txt", "content1");
            //    zip.AddEntry("file2.txt", "content2");
            //    zip.Save(outputStream);
            //}

            //outputStream.Position = 0;
            //return File(outputStream, "application/zip", "filename.zip");

        }

        public string DeleteFile(int id, string documentUrl) // сделать запрос через Ajax
        {
            PaymentStatement paymentStatement = db.PaymentStatements.Find(id);
            
            string fullPath = Server.MapPath(documentUrl);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            paymentStatement.DocumentUrl = null;
            PaymentsDocument removingFile = paymentStatement.PaymentsDocuments.Where(i => i.DocumentUrl == documentUrl).FirstOrDefault();
            paymentStatement.PaymentsDocuments.Remove(removingFile);
            db.Entry(paymentStatement).State = EntityState.Modified;
            db.SaveChanges();
            return "Файл был удален";
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

        public string SetInvoiceChecked(int id, bool status)
        {
            try
            {
                db.PaymentStatements.Find(id).InvoiceChecked = status;
                db.SaveChanges();
                return "Изменение сохранено";
            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
        }

        public string SetPaymentApproved(int id, bool status)
        {
            try
            {
                db.PaymentStatements.Find(id).PaymentApproved = status;
                db.SaveChanges();
                return "Изменение сохранено";
            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
        }

        public string SetPaymentDone(int id, bool status)
        {
            try
            {
                db.Payments.Find(id).PaymentPaymentDone = status;
                db.SaveChanges();
                return "Изменение сохранено";
            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
        }

        public JsonResult GetCompaniesByName(string request)
        {
            var counterparties = OrestDb.klt.Where(i => i.name.Contains(request)).ToList();

            return Json(counterparties.Take(100), JsonRequestBehavior.AllowGet);
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
