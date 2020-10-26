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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Finance.Controllers
{
    [Authorize]
    public class PaymentStatementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private orestEntities OrestDb = new orestEntities();

        public HttpClient GetAuthorizedHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-authorization", "3c7867b0c737e498882fa447629");
            client.BaseAddress = new Uri("http://www.crmorest-api.net.ua/");
            return client;
        }

        // GET: PaymentStatements
        public async Task<ActionResult> Index(int pageNumber = 1, int itemsOnView = 50)
        {
            try
            {
                ViewBag.errors = CheckPayments();
                //SaveOrestInvoiceNumberToFinance();
            }
            catch
            {
                ViewBag.errors = "Ошибка";
            }

            //List<PaymentStatement> lastPayments = await db.PaymentStatements.OrderByDescending(i => i.Id).Take(100).ToListAsync();
            //var lastElementId = lastPayments.Last().Id;
            //List<PaymentStatement> unpaidPayments = await db.PaymentStatements.Where(i => i.PaymentDone == false && i.Id < lastElementId).ToListAsync();

            //List<PaymentStatement> paymentsPlusUnpaidPayments = new List<PaymentStatement>();
            //paymentsPlusUnpaidPayments.AddRange(lastPayments);
            //paymentsPlusUnpaidPayments.AddRange(unpaidPayments);

            //return View(paymentsPlusUnpaidPayments.OrderBy(i => i.PaymentDone).ThenByDescending(j => j.Id).Take(100).ToList());

            IEnumerable<PaymentStatement> paymentDosc = db.PaymentStatements.OrderBy(i => i.PaymentDone).ThenByDescending(j => j.Id);

            PaginationPageInfo pageInfo = new PaginationPageInfo { PageNumber = pageNumber, PageSize = itemsOnView, TotalItems = paymentDosc.Count()};
            PaymentStatementsViewModel ivm = new PaymentStatementsViewModel { PageInfo = pageInfo, PaymentDocumnets = paymentDosc.Skip((pageNumber - 1) * itemsOnView).Take(itemsOnView).ToList()};
            return View(ivm);
        }

        class ApiResponse<T>
        {
            public int numRows { get; set; }
            public List<T> data { get; set; }
        }

        public List<OutgoingPayment> GetOutgoingPaymentsByDate(string startDate, string endDate)
        {
            HttpClient client = GetAuthorizedHttpClient();
            var response = client.GetAsync(client.BaseAddress + "Api/?table=OutgoingPayment&beg="+ startDate + "&end="+ endDate + "&fields=OrestId,OrestDocumentNumber,OrestOurCompanyId,OrestClientId,OrestDocumentDate,OrestPaymentSum,OrestBankId,OrestSupplierInvoiceId,OrestDocumentStatus");
            string stringResponse = response.Result.Content.ReadAsStringAsync().Result;
            ApiResponse<OutgoingPayment> apiResponse = JsonConvert.DeserializeObject<ApiResponse<OutgoingPayment>>(stringResponse);

            return apiResponse != null ? apiResponse.data : new List<OutgoingPayment>();
        }

        public OrestSupplierInvoice GetOrestSupplierInvoiceById(int id)
        {
            HttpClient client = GetAuthorizedHttpClient();
            var response = client.GetAsync(client.BaseAddress + "Api/?table=OrestSupplierInvoice&searchID=" + id + " & fields=OrestId,OrestDocumentNumber,OrestInvoiceDate,OrestInvoiceSum,OrestDocumentStatus,OrestInvoiceNumber");
            string stringResponse = response.Result.Content.ReadAsStringAsync().Result;
            ApiResponse<OrestSupplierInvoice> suplierInvoice = JsonConvert.DeserializeObject<ApiResponse<OrestSupplierInvoice>>(stringResponse);

            return suplierInvoice.data.FirstOrDefault();
        }

        public OutgoingPayment GetOutgoingPaymentById(int id)
        {
            HttpClient client = GetAuthorizedHttpClient();
            var response = client.GetAsync(client.BaseAddress + "Api/?table=OutgoingPayment&searchID=" + id + "&fields=OrestId,OrestDocumentNumber,OrestOurCompanyId,OrestClientId,OrestDocumentDate,OrestPaymentSum,OrestBankId,OrestSupplierInvoiceId,OrestDocumentStatus");
            string stringResponse = response.Result.Content.ReadAsStringAsync().Result;
            ApiResponse<OutgoingPayment> outgoingPyments = JsonConvert.DeserializeObject<ApiResponse<OutgoingPayment>>(stringResponse);

            return outgoingPyments.data.FirstOrDefault();
        }

        public List<OrestSupplierInvoice> GetOrestSupplierInvoiceByDate(string startDate, string endDate)
        {
            HttpClient client = GetAuthorizedHttpClient();
            var response = client.GetAsync(client.BaseAddress + "Api/?table=OrestSupplierInvoice&beg=" + startDate + "&end=" + endDate + " & fields=OrestId,OrestDocumentNumber,OrestInvoiceDate,OrestInvoiceSum,OrestDocumentStatus,OrestInvoiceNumber,OrestClientId");
            string stringResponse = response.Result.Content.ReadAsStringAsync().Result;
            ApiResponse<OrestSupplierInvoice> suplierInvoice = JsonConvert.DeserializeObject<ApiResponse<OrestSupplierInvoice>>(stringResponse);

                        
            return suplierInvoice != null ? suplierInvoice.data : new List<OrestSupplierInvoice>();
        }

        public List<string> CheckPayments()
        {
            // Добавить проверку оплат инвойсов при импорте 
            // Банк-расход валюты при импортных операциях -в API таблица - bkh-4
            // Что считать номером инвойса (может состоять из нескольких проформ + номер инвойса становится известен уже после оплаты или отгрузки)?
            // Возможно стоит привязаться к номеру спецификации к договору и сумме оплаты

            // Если платеж был проведен в ОРЕСТе но при этом в Finance не был продтверден в Оплату -
            // сообщить об ошибке!!!

            // формат год/месяц/день, startDate=<endDate
            string startDate = DateTime.Now.AddDays(-10).Date.ToString("yyyy.MM.dd");
            string endDate = DateTime.Now.Date.ToString("yyyy.MM.dd");

            // Получаем все исходящие платежы (банк-расходы) из Orest
            
            List<OutgoingPayment> outgoingPayments = GetOutgoingPaymentsByDate(startDate, endDate).GroupBy(i => i.OrestSupplierInvoiceId).Select(j => j.First()).ToList();

            // Получаем все счета-фактуры поставщика из Orest
            List<OrestSupplierInvoice> orestInvoices = GetOrestSupplierInvoiceByDate(DateTime.Now.AddYears(-1).Date.ToString("yyyy.MM.dd"), endDate);
            // Получаем все платежи (за посл.6 мес.) из Orest
            List<OutgoingPayment> orestOutgoingPayments = GetOutgoingPaymentsByDate(DateTime.Now.AddMonths(-6).Date.ToString("yyyy.MM.dd"), endDate);

            foreach (var outPayment in outgoingPayments)
            {              
                OrestSupplierInvoice supplierInvoice = orestInvoices.Find(i=>i.OrestId == outPayment.OrestSupplierInvoiceId);

                if (supplierInvoice != null)
                {
                    supplierInvoice.OutgoingPayments.AddRange(orestOutgoingPayments.Where(i=>i.OrestSupplierInvoiceId == outPayment.OrestSupplierInvoiceId));
                    outPayment.OrestSupplierInvoice = supplierInvoice;
                }
            }
            //db.SaveChanges();

            var ouPayNumbList = outgoingPayments.OrderByDescending(o=>o.OrestSupplierInvoice.OrestInvoiceNumber).Select(i => i.OrestSupplierInvoice.OrestInvoiceNumber).ToList();
            // Получаем все не завершенные платежи из finance
            var paymentStatement = db.PaymentStatements.Where(i=>i.PaymentDone == false).ToList();
            //var psInvNumbList = paymentStatement.OrderByDescending(o => o.InvoiceNumber).Select(i => i.InvoiceNumber).ToList();

            foreach (var p in paymentStatement)
            {
                p.InvoiceNumber = p.InvoiceNumber.Trim();
                db.Entry(p).State = EntityState.Modified;

                //if (p.InvoiceNumber == "23424")
                //    orestPaymentsIds = p.Payments.Select(i => i.OrestPaymentId).ToList();
            }

            List<string> errors = new List<string>();
            if (outgoingPayments != null)
            {
                foreach (var outgoingPayment in outgoingPayments)
                {
                    //if (outgoingPayment.OrestSupplierInvoice.OrestInvoiceNumber.Trim() == "23424")
                    //    ;

                    DateTime invoiceDate = Convert.ToDateTime(outgoingPayment.OrestSupplierInvoice.OrestInvoiceDate);
                    decimal invoiceSum = Convert.ToDecimal(outgoingPayment.OrestSupplierInvoice.OrestInvoiceSum);
                    var invoice = paymentStatement.FirstOrDefault(a => a.InvoiceNumber == outgoingPayment.OrestSupplierInvoice.OrestInvoiceNumber.Trim() &&
                    a.InvoiceDate == invoiceDate &&
                    a.InvoiceSumma == invoiceSum &&
                    a.KltId == outgoingPayment.OrestSupplierInvoice.OrestClientId
                    );
                    if (invoice != null)
                    {
                        foreach (var orestPayment in outgoingPayment.OrestSupplierInvoice.OutgoingPayments) 
                        {
                            if (orestPayment.OrestDocumentStatus == 1)
                            {
                                foreach (var payment in invoice.Payments)
                                {
                                    var checkedPayments = invoice.Payments.Find(i => i.OrestPaymentId == orestPayment.OrestId);
                                    if (checkedPayments != null)
                                        break;
                                    if (Convert.ToDouble(payment.Summa) == orestPayment.OrestPaymentSum && payment.PaymentPaymentDone == false)
                                    {
                                        if (payment.MySQLBankId != orestPayment.OrestBankId)
                                        {
                                            //ViewBag.BankError = "Банки в системах ОРЕСТ:" + OrestDb.bank.Find(orestPayment.OrestBankId).name + " и MFINANCE: " + payment.MySQLBankName + " не совпадают. Счет:" + OrestDb.klt.Find(orestPayment.OrestClientId).name + ", № " + payment.PaymentStatement.InvoiceNumber + "; " + payment.PaymentStatement.InvoiceDate + ", - " + payment.PaymentStatement.InvoiceSumma;
                                            errors.Add("Банки в системах ОРЕСТ:" + OrestDb.bank.Find(orestPayment.OrestBankId).name + " и MFINANCE: " + payment.MySQLBankName + " не совпадают. Счет:" + OrestDb.klt.Find(orestPayment.OrestClientId).name + ", № " + payment.PaymentStatement.InvoiceNumber + "; " + payment.PaymentStatement.InvoiceDate + ", - " + payment.PaymentStatement.InvoiceSumma);
                                        }

                                        if (payment.PartialPaymentApproved == true)
                                        {
                                            payment.OrestPaymentId = orestPayment.OrestId;
                                            payment.PaymentPaymentDone = true;
                                            //payment.orestOutgoingPaymentId = orestPayment.OrestId;
                                            db.Entry(payment).State = EntityState.Modified;

                                            db.SaveChanges();

                                            break;
                                        }
                                        else
                                        {
                                            //ViewBag.NotConfirmedPayment = "Не утвержденная оплата по счету: " + OrestDb.klt.Find(orestPayment.OrestClientId).name + ", № " + payment.PaymentStatement.InvoiceNumber + "; " + payment.PaymentStatement.InvoiceDate + ", - " + payment.PaymentStatement.InvoiceSumma;
                                            errors.Add("Не утвержденная оплата по счету: " + OrestDb.klt.Find(orestPayment.OrestClientId).name + ", № " + payment.PaymentStatement.InvoiceNumber + "; " + payment.PaymentStatement.InvoiceDate + ", - " + payment.PaymentStatement.InvoiceSumma);
                                        }
                                    }
                                }
                            }
                        }
                        invoice.OrestPrhId = outgoingPayment.OrestSupplierInvoice.OrestId;
                        db.Entry(invoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //else
                    //{
                    //    PaymentStatement lostPaymentStatement = db.PaymentStatements.FirstOrDefault(a => a.InvoiceNumber == orestInvoice.OrestInvoiceNumber.Trim() && a.PaymentDone != true);
                    //    if (lostPaymentStatement == null)
                    //        errors.Add("В finance не найден счет №: " + orestInvoice.OrestInvoiceNumber + "; " + orestInvoice.OrestInvoiceDate + "; " + orestInvoice.OrestInvoiceSum + "; ");
                    //    else
                    //    {
                    //        lostPaymentStatement = db.PaymentStatements.FirstOrDefault(a => a.InvoiceDate == Convert.ToDateTime(orestInvoice.OrestInvoiceDate) &&
                    //        a.InvoiceNumber == lostPaymentStatement.InvoiceNumber);

                    //        if (lostPaymentStatement == null)
                    //            errors.Add("В finance и Orest не совпадает дата счета №: " + orestInvoice.OrestInvoiceNumber + "; " + orestInvoice.OrestInvoiceDate + "; " + orestInvoice.OrestInvoiceSum + "; ");
                    //        else
                    //        {
                    //            lostPaymentStatement = db.PaymentStatements.FirstOrDefault(a => a.InvoiceSumma == Convert.ToDecimal(orestInvoice.OrestInvoiceSum) &&
                    //            a.InvoiceNumber == lostPaymentStatement.InvoiceNumber &&
                    //            a.InvoiceDate == lostPaymentStatement.InvoiceDate);

                    //            if (lostPaymentStatement == null)
                    //                errors.Add("В finance и Orest не совпадает сумма счета №: " + orestInvoice.OrestInvoiceNumber + "; " + orestInvoice.OrestInvoiceDate + "; " + orestInvoice.OrestInvoiceSum + "; ");
                    //            else
                    //            {
                    //                lostPaymentStatement = db.PaymentStatements.FirstOrDefault(a => a.InvoiceSumma == Convert.ToDecimal(orestInvoice.OrestInvoiceSum) &&
                    //                a.InvoiceNumber == lostPaymentStatement.InvoiceNumber &&
                    //                a.InvoiceDate == lostPaymentStatement.InvoiceDate &&
                    //                a.KltId == lostPaymentStatement.KltId);
                    //                if(lostPaymentStatement == null)
                    //                    errors.Add("В finance и Orest не совпадает id Поставщика счета №: " + orestInvoice.OrestInvoiceNumber + "; " + orestInvoice.OrestInvoiceDate + "; " + orestInvoice.OrestInvoiceSum + "; ");
                    //            }
                    //        }
                    //    }
                    //}

                }


                //Для поиска несанкционированных платежей (те которые отсутствуют в finance но есть в Orest)
                //Хранить в db finance id платежей из ОРЕСТа
                //Если полатеж есть в Orest но его id отсутсвует в finance - ощибка
                foreach (var op in outgoingPayments)
                {
                    var approvedPayment = db.Payments.FirstOrDefault(i => i.OrestPaymentId == op.OrestId);

                    if (approvedPayment == null)
                    {
                        var orestSupInvoice = OrestDb.prh.FirstOrDefault(i => i.id == op.OrestSupplierInvoiceId);
                        errors.Add("Несанкционированный платёж: " + OrestDb.klt.Find(op.OrestClientId).name + ", № " + orestSupInvoice.ninv + "; " + orestSupInvoice.datd + ", - " + op.OrestPaymentSum + "; " + OrestDb.bank.Find(op.OrestBankId).name);
                    }
                }

                foreach (var ps in paymentStatement)
                {
                    //if (ps.InvoiceNumber == "Ц-000063091")
                    //    ;
                    decimal sum = 0;
                    foreach (var payment in ps.Payments)
                    {
                        if (payment.PaymentPaymentDone == true)
                            sum += payment.Summa;
                        else
                            ps.PaymentDone = false;                             
                    }
                    if (sum == ps.InvoiceSumma)
                    {
                        ps.InvoiceChecked = true;
                        ps.PaymentApproved = true;
                        ps.PaymentDone = true;

                        db.Entry(ps).State = EntityState.Modified;
                    }
                    if (ps.OrestPrhId == 0 && ps.Payments.FirstOrDefault().PaymentPaymentDone == true)
                    {
                        //ViewBag.PaymentNotFound = "Не найден счет: #" + ps.InvoiceNumber + " дата" + ps.InvoiceDate + " компания" + ps.Counterparty.name + " сумма " + ps.InvoiceSumma + " комментарий" + ps.Comment + " файл" + ps.DocumentUrl;
                        errors.Add("Не найден счет: #" + ps.InvoiceNumber + " дата" + ps.InvoiceDate + " компания" + OrestDb.klt.Find(ps.KltId) + " сумма " + ps.InvoiceSumma + " комментарий" + ps.Comment + " файл" + ps.DocumentUrl);
                    }
                }
                db.SaveChanges();
            }

            //Проверка снятых с проводки банк-расходов!
            //Получить все bkh где bkh.lg = 0 - документ не проведен
            //Проверить есть ли в PaymentStatements соответствующий платеж. Если есть, изменить его статус - paymentDone = false;

            DateTime date = DateTime.Now.AddYears(-1);
            //// Получаем все не проведенные банк-расходы за последний год
            var unfinishedPaymentsId = OrestDb.bkh.Where(p => p.tp == 2 && p.lg == 0 && p.dusr > date).Select(i => i.id).ToList();
            List<OutgoingPayment> UnfinishedPayments = new List<OutgoingPayment>();
            if (unfinishedPaymentsId.Count > 0)
            {
                foreach (var id in unfinishedPaymentsId)
                {
                    var unfinishedPayment = GetOutgoingPaymentById(Convert.ToInt32(id));
                    var unPayment = db.PaymentStatements.FirstOrDefault(p => p.OrestPrhId == unfinishedPayment.OrestSupplierInvoiceId);
                    if (unPayment != null)
                    {
                        unPayment.PaymentDone = false;
                        foreach (var p in unPayment.Payments)
                        {
                            if (p.Summa == Convert.ToDecimal(unfinishedPayment.OrestPaymentSum))
                                p.PaymentPaymentDone = false;
                        }
                        //ViewBag.PaymentError = "ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + " ПРОВЕДЕН В MFINANCE СНЯТ С ПРОВОДКИ В СИСТЕМЕ OREST!!!";
                        errors.Add("ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + " ПРОВЕДЕН В MFINANCE СНЯТ С ПРОВОДКИ В СИСТЕМЕ OREST!!!");
                    }
                    else
                    {
                        //Обнаружен в Оресте платёж, счёт которого отсутствует в Финансе. 
                        errors.Add("ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + "Обнаружен в Оресте платёж, счёт которого отсутствует в Финансе.!!!");
                    }
                    db.Entry(unPayment).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }

            //if (UnfinishedPayments.Count > 0)
            //{
            //    foreach (var unfinishedPayment in UnfinishedPayments)
            //    {
            //        var unPayment = db.PaymentStatements.FirstOrDefault(p => p.OrestPrhId == unfinishedPayment.OrestSupplierInvoiceId);
            //        if (unPayment != null)
            //        {
            //            unPayment.PaymentDone = false;
            //            foreach (var p in unPayment.Payments)
            //            {
            //                if (p.Summa == Convert.ToDecimal(unfinishedPayment.OrestPaymentSum))
            //                    p.PaymentPaymentDone = false;
            //            }
            //            //ViewBag.PaymentError = "ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + " ПРОВЕДЕН В MFINANCE СНЯТ С ПРОВОДКИ В СИСТЕМЕ OREST!!!";
            //            errors.Add("ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + " ПРОВЕДЕН В MFINANCE СНЯТ С ПРОВОДКИ В СИСТЕМЕ OREST!!!");
            //        }
            //        else
            //        {
            //            //Обнаружен в Оресте платёж, счёт которого отсутствует в Финансе. 
            //            errors.Add("ПЛАТЕЖ ПО СЧЕТУ" + unPayment.Counterparty + " " + unPayment.InvoiceNumber + " " + unPayment.InvoiceDate + "Обнаружен в Оресте платёж, счёт которого отсутствует в Финансе.!!!");
            //        }
            //        db.Entry(unPayment).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //}
            if (errors.Count == 0)
                errors.Add("0-errors");
            return errors;
        }

        public ActionResult PaymentsByComment(string comment)
        {
            var result = db.PaymentStatements.Where(p => p.Comment == comment).ToList().OrderByDescending(i => i.Id);
            return View(db.PaymentStatements.Where(p => p.Comment == comment).ToList().OrderByDescending(i => i.Id));
        }
        public JsonResult GetPaymentsByComment(string request)
        {
            var result = db.PaymentStatements.Where(p => p.Comment.Contains(request)).Select(i=>i.Comment).Distinct().ToList();
            var jsonresult = Json(result.Take(100), JsonRequestBehavior.AllowGet);
            return Json(result.Take(100), JsonRequestBehavior.AllowGet);
        }


        public ActionResult PaymentsByCompany(int companyId)
        {
            return View(db.PaymentStatements.Where(c => c.KltId == companyId).ToList().OrderByDescending(i => i.Id));
        }

        public JsonResult GetCompaniesByName(string request)
        {
            var counterparties = OrestDb.klt.Where(i => i.name.Contains(request)).ToList();

            return Json(counterparties.Take(100), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult sortByPayment()
        //{
        //    return View("Index", db.PaymentStatements.OrderBy(i => i.PaymentDone).ThenByDescending(j => j.Id).Take(100).ToList());
        //}

        //public ActionResult SortByPaymentDate()
        //{
        //    return View("Index", db.PaymentStatements.OrderBy(i => i.Payments.Select(p => p.PaymentPaymentDone)));
        //}
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
            //ViewBag.CurrenciesList = new SelectList(db.Currencies, "Id", "CurrencyName");
            return View(payment);
        }
        // POST: PaymentStatements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,KltId,InvoiceNumber,InvoiceDate,InvoiceSumma,Comment,DocumentUrl,CurrencyId")]
        PaymentStatement paymentStatement, decimal[] Summa, bool[] PartialPaymentChecked, bool[] PartialPaymentApproved, bool[] PaymentPaymentDone, HttpPostedFileBase[] UploadDocumentUrl, int?[] MySQLBankId)
        {        
            if (paymentStatement.KltId == 0)
            {
                ModelState.AddModelError("Counterparty_Id", "Не выбран контрагент");
                if (UploadDocumentUrl != null)
                {
                    //ViewBag.FileName = System.IO.Path.GetFileName(UploadDocumentUrl.FileName);
                }
            }
            var currentUser = db.Users.Where(a => a.UserName == User.Identity.Name).First();

            paymentStatement.Payments = new List<Payment>();

            if (Summa != null)
            {
                int invoiceCheckedIndex = 0;
                int invoiceApprovedIndex = 0;
                int paymentDoneIndex = 0;

                for (var i = 0; i < Summa.Length; i++)
                {
                    Payment payment = new Payment();

                    payment.PaymentWhenEdited = DateTime.Now;
                    payment.PaymentWhoAddThis = currentUser;
                    payment.Summa = Summa[i];
                    if(MySQLBankId != null)
                    {
                        var orestBank = OrestDb.bank.Find(MySQLBankId[i]);
                        if (orestBank != null)
                        {
                            payment.MySQLBankName = orestBank.name;
                            payment.MySQLBankId = orestBank.id;
                        }
                    }
                    

                    payment.PartialPaymentChecked = PartialPaymentChecked[invoiceCheckedIndex];
                    if (PartialPaymentChecked[invoiceCheckedIndex] == true)
                        invoiceCheckedIndex = invoiceCheckedIndex + 2;
                    else
                        invoiceCheckedIndex++;

                    payment.PartialPaymentApproved = PartialPaymentApproved[invoiceApprovedIndex];
                    if (PartialPaymentApproved[invoiceApprovedIndex] == true)
                        invoiceApprovedIndex = invoiceApprovedIndex + 2;
                    else
                        invoiceApprovedIndex++;

                    payment.PaymentPaymentDone = PaymentPaymentDone[paymentDoneIndex];
                    if (PaymentPaymentDone[paymentDoneIndex] == true)
                        paymentDoneIndex = paymentDoneIndex + 2;
                    else
                        paymentDoneIndex++;

                    //payment.PaymentStatement = paymentStatement;

                    paymentStatement.Payments.Add(payment);
                }
            }

            if (ModelState.IsValid)
            {       
                
                if (UploadDocumentUrl != null)
                {
                    //// получаем имя файла
                    //string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(UploadDocumentUrl.FileName);
                    //// сохраняем файл в папку Files в проекте
                    //UploadDocumentUrl.SaveAs(Server.MapPath("~/Files/" + fileName));
                    //paymentStatement.DocumentUrl = "/Files/" + fileName;
                    foreach (var file in UploadDocumentUrl)
                    {
                        if (file == null)
                            break;
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
                paymentStatement.WhenCreated = DateTime.Now;
                paymentStatement.Currency = db.Currencies.Find(paymentStatement.CurrencyId);

                if (paymentStatement.PaymentDone)
                {
                    paymentStatement.InvoiceChecked = true;
                    paymentStatement.PaymentApproved = true;
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

            var invoiceOrest = await OrestDb.prh.FirstOrDefaultAsync(a => a.ninv == paymentStatement.InvoiceNumber&&a.datd == paymentStatement.InvoiceDate);

            //Ищем в базе Орест редактируемый счёт и ставим флаг, показывающий найден счёт или нет:
            paymentStatement.isInvoiceFound = invoiceOrest!=null;

            if (paymentStatement.isInvoiceFound)
            {
                //paymentStatement.isInvoiceSummaCorrect = invoiceOrest.sd == paymentStatement.InvoiceSumma;
                paymentStatement.isInvoicePaymentsCorrect = true;
                //Получаем список платежей по счёту из ореста.
                var paymentsOrest = OrestDb.bkh.Where(a => a.prh == invoiceOrest.id);
                foreach (var payment in paymentStatement.Payments)
                {
                    //Если сумма платежа не верна
                    if (!payment.isPaymentSummaCorrect)
                        //тогда общая сумма счёта также не верна
                        paymentStatement.isInvoicePaymentsCorrect = false;

                    //payment.PaymentPaymentDone = paymentsOrest.FirstOrDefault(a => a.ndoc == payment.numberDocument).lg == 1;
                }
                await db.SaveChangesAsync();
            }
            else
            {
                paymentStatement.isInvoiceSummaCorrect = false;
                paymentStatement.isInvoicePaymentsCorrect = false;
            }

            

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
        PaymentStatement paymentStatement, decimal[] Summa, bool[] PartialPaymentChecked, bool[] PartialPaymentApproved, bool[] PaymentPaymentDone, int?[] UploadDocumentId, HttpPostedFileBase[] UploadDocumentUrl, string LastUrl, int?[] MySQLBankId, int[] OrestPaymentId)
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
                currentPaymentStatement.PaymentDone = paymentStatement.PaymentDone;

                //1. Удаляем все ранее добавленные файлы
                if (currentPaymentStatement.PaymentsDocuments.Count != 0)
                {
                    for (int i = 0; i < currentPaymentStatement.PaymentsDocuments.Count; i++)
                    {
                        if(UploadDocumentId == null)  // если удалены все ранее добавленные файлы
                        {
                            string fullPath = Server.MapPath(currentPaymentStatement.PaymentsDocuments[i].DocumentUrl);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                            currentPaymentStatement.PaymentsDocuments.Remove(currentPaymentStatement.PaymentsDocuments[i]);
                        }
                        // если удалены некоторорые ранее добавленные файлы
                        else if (UploadDocumentId.Contains(currentPaymentStatement.PaymentsDocuments[i].Id) == false)
                        {
                            string fullPath = Server.MapPath(currentPaymentStatement.PaymentsDocuments[i].DocumentUrl);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                            currentPaymentStatement.PaymentsDocuments.Remove(currentPaymentStatement.PaymentsDocuments[i]);
                        }                    
                    }
                }
                //2.Сохраняем ново добавленные файлы из UploadDocumentUrl
                if(UploadDocumentUrl.Length > 0)
                    foreach (var file in UploadDocumentUrl)
                    {
                        if (file != null)
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

                if (currentPaymentStatement.Payments == null)
                    currentPaymentStatement.Payments = new List<Payment>();

                paymentStatement.Payments = new List<Payment>();

                if (Summa != null)
                {
                    int invoiceCheckedIndex = 0;
                    int invoiceApprovedIndex = 0;
                    int paymentDoneIndex = 0;

                    for (var i = 0; i < Summa.Length; i++)
                    {
                        Payment payment = new Payment();

                        payment.PaymentWhenEdited = DateTime.Now;
                        payment.PaymentWhoAddThis = currentUser;
                        payment.Summa = Summa[i];
                        payment.OrestPaymentId = OrestPaymentId[i];
                        try
                        {
                            var orestBank = OrestDb.bank.Find(MySQLBankId[i]);
                            if (orestBank != null)
                            {
                                payment.MySQLBankName = orestBank.name;
                                payment.MySQLBankId = orestBank.id;
                            }
                        }
                        catch
                        {
                            return View(paymentStatement);
                        }
                        
                       

                        payment.PartialPaymentChecked = PartialPaymentChecked[invoiceCheckedIndex];
                        if (PartialPaymentChecked[invoiceCheckedIndex] == true)
                            invoiceCheckedIndex = invoiceCheckedIndex + 2;
                        else
                            invoiceCheckedIndex++;

                        payment.PartialPaymentApproved = PartialPaymentApproved[invoiceApprovedIndex];
                        if (PartialPaymentApproved[invoiceApprovedIndex] == true)
                            invoiceApprovedIndex = invoiceApprovedIndex + 2;
                        else
                            invoiceApprovedIndex++;

                        payment.PaymentPaymentDone = PaymentPaymentDone[paymentDoneIndex];
                        if (PaymentPaymentDone[paymentDoneIndex] == true)
                            paymentDoneIndex = paymentDoneIndex + 2;
                        else
                            paymentDoneIndex++;

                        //payment.PaymentStatement = paymentStatement;

                        paymentStatement.Payments.Add(payment);
                    }
                    currentPaymentStatement.Payments = paymentStatement.Payments;

                    //decimal PaymentsSumm = 0; // расчитываем сумму оплат
                    //foreach (var i in currentPaymentStatement.Payments.ToList())
                    //{
                    //    if (i.PaymentPaymentDone == true)
                    //    {
                    //        PaymentsSumm += i.Summa;
                    //        currentPaymentStatement.PaymentDone = true;
                    //    }
                    //    else
                    //        currentPaymentStatement.PaymentDone = false;
                    //}

                    //if (PaymentsSumm == currentPaymentStatement.InvoiceSumma)
                    //{
                    //    currentPaymentStatement.PaymentDone = true; //оплачено 100%
                    //}
                    //else
                    //    currentPaymentStatement.PaymentDone = false;
                }

                //if (currentPaymentStatement.PaymentDone)
                //{
                //    currentPaymentStatement.InvoiceChecked = true;
                //    currentPaymentStatement.PaymentApproved = true;
                //}
             
                db.Entry(currentPaymentStatement).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");

                //return RedirectToAction(LastUrl != "" ? LastUrl : "Index");
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
            PaymentStatement paymentStatement = db.PaymentStatements.Find(id);
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
            //int index = 0;
            for(int i = 0; i < documents.Count; i++)
            {
                if (documents[i].DocumentUrl != null)
                    allFilesUrl[i] = documents[i].DocumentUrl;
                else
                    db.PaymentsDocuments.Remove(documents[i]);
            }
            return View(allFilesUrl);
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
            for (int i = 0; i < paymentStatement.PaymentsDocuments.Count; i++)
            {
                db.PaymentsDocuments.Remove(paymentStatement.PaymentsDocuments[i]);
            }
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

        //public string SetPaymentDone(int id, bool status)
        //{
        //    try
        //    {
        //        db.Payments.Find(id).PaymentPaymentDone = status;
        //        db.SaveChanges();
        //        return "Изменение сохранено";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Ошибка: " + ex.Message;
        //    }
        //}

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
