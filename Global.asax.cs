using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Finance.Controllers;
using System.Xml;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.Xml.Linq;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.IO;
using Finance.Models;

namespace Finance
{
    public class Debug
    {
        public static void Log(string Message)
        {
            string result = DateTime.Now.ToString() + ": " + Message;
            var stream = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("/Files/") + HttpContext.Current.User.Identity.Name + "_log.txt", true);
            stream.WriteLine(result);
            stream.Close();
        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //http://blogs.microsoft.co.il/idof/2008/08/22/change-entity-framework-storage-db-schema-in-runtime/#commentmessage
            //https://stackoverflow.com/questions/2663164/changing-schema-name-on-runtime-entity-framework?rq=1
            //

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ApplicationDbContext db = new ApplicationDbContext();
            //foreach (var i in db.PaymentStatements.Where(i => i.Id > 1284))
            //{
            //    Payment payment = new Payment();
            //    payment.InvoiceChecked = i.InvoiceChecked;
            //    payment.PaymentApproved = i.PaymentApproved;
            //    payment.PaymentPaymentDone = i.PaymentPaymentDone;
            //    payment.InvoiceCheckedUser = i.InvoiceCheckedUser;
            //    payment.PaymentApprovedUser = i.PaymentApprovedUser;
            //    payment.PaymentPaymentDoneUser = i.PaymentPaymentDoneUser;
            //    payment.Summa = i.InvoiceSumma;
            //    payment.PaymentStatement = i;
            //    db.Payments.Add(payment);
            //}
            //db.SaveChanges();

            //Наполнение таблицы  PaymentsDocument информацией о файлах в старых PaymentStatement.
            // Старые PaymentStatements -данные внесенные до создания возможности загрузки нескольких файлов(PaymentsDocuments).
            //ApplicationDbContext db = new ApplicationDbContext();
            //foreach (var i in db.PaymentStatements)
            //{
            //    PaymentsDocument paymentDocument = new PaymentsDocument();
            //    paymentDocument.DocumentUrl = i.DocumentUrl;
            //    paymentDocument.PaymentStatement = i;
            //    db.PaymentsDocuments.Add(paymentDocument);
            //}
            //db.SaveChanges();

        }
    }
}
