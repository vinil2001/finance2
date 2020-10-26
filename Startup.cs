using Microsoft.Owin;
using Owin;
using Finance.Services;
using Finance.Models;
using System.Collections.Generic;
using Finance.Controllers;
using System;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Finance.Startup))]
namespace Finance
{
    public partial class Startup
    {
        public static ApplicationDbContext dbPS = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //setPaymentCheckedAndApprovedWhenPaymentDone();       

        }
        //public void setPaymentCheckedAndApprovedWhenPaymentDone()
        //{
        //    foreach(var p in dbPS.PaymentStatements)
        //    {
        //        if(p.PaymentDone == true && p.InvoiceChecked == false)
        //        {
        //            p.InvoiceChecked = true;
        //            p.PaymentApproved = true;
        //        }
        //    }
        //    dbPS.SaveChanges();
        //}
    }
}
