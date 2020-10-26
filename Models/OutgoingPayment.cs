using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class OutgoingPayment
    {
        public int Id { get; set; }
        //Data from OrestDb(MySQL)
        public int OrestId { get; set; }                    
        public string OrestDocumentNumber { get; set; }    //ndoc
        public int OrestOurCompanyId { get; set; }        //frm
        public int OrestClientId { get; set; }           // klt
        public string OrestDocumentDate { get; set; }    // datd
        public double OrestPaymentSum { get; set; }      // sd
        public int OrestBankId { get; set; }             // bank     
        public int OrestSupplierInvoiceId { get; set; }  //prh
        public int OrestPaymentComment { get; set; }      //comt
        public int OrestDocumentStatus { get; set; }      // lg = 1 - документ проведен, 0 - нет
        public virtual OrestSupplierInvoice OrestSupplierInvoice { get; set; }
        //public bool PaymentDetected { get; set; }      // если платеж был проверен и отмечен в бд Орест
    }
}