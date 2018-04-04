using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finance.Models
{
    public class PaymentStatement
    {
        public int Id { get; set; }
        public long KltId { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Плательщик должен быть выбран. Если плательщик новый, нажмите + для его добавления")]
        public int Counterparty_Id { get; set; }
        
        [Display(Name = "Плательщик")]
        public virtual Counterparty Counterparty { get; set; }

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