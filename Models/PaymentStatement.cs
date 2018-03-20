using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class PaymentStatement
    {
        public int Id { get; set; }
        public long KltId { get; set; }

        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceSumma { get; set; }
        public string Comment { get; set; }
        public bool InvoiceChecked { get; set; }           //Инвойс проверен по цене и содержанию
        public bool PaymentApproved { get; set; }          // Платеж подтвержден руководителем (может быть подтвержден без проверки)
        public bool PaymentDone { get; set; }              // Платеж выполнен (модет осуществляться только после подтвеждения рук.)

        public virtual ApplicationUser InvoiceCheckedUser { get; set; }
        public virtual ApplicationUser PaymentApprovedUser { get; set; }
        public virtual ApplicationUser PaymentDoneUser { get; set; }
            
        public string DocumentUrl { get; set; }
    }
}