using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class OrestSupplierInvoice
    {
        public int Id { get; set; }
        public int OrestId { get; set; }
        public string OrestDocumentNumber { get; set; }
        public int OrestOurCompanyId { get; set; }
        public int OrestClientId { get; set; }
        public string OrestInvoiceDate { get; set; }
        public double OrestInvoiceSum { get; set; }
        public string OrestInvoiceNumber { get; set; }       //ninv
        public string OrestPaymentComment { get; set; }
        public int OrestDocumentStatus { get; set; }
        public virtual List<OutgoingPayment> OutgoingPayments { get; set; } = new List<OutgoingPayment>();
    }
}