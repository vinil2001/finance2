using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finance.Models
{
    public class PaymentStatement
    {
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Плательщик должен быть выбран. Если плательщик новый, нажмите + для его добавления")]
        public long KltId { get; set; }

        public long OrestPrhId { get; set; } // prh.id - счет фактура поставщика

        [Display(Name = "Плательщик")]
        [NotMapped]
        public klt Counterparty
        {
            get
            {
                orestEntities dbOrest = new orestEntities();
                return dbOrest.klt.Find(KltId);
            }
            private set { }
        }

        public virtual List<Payment> Payments { get; set; }

        //[NotMapped]
        //public SelectList Banks { get; set; }
        [NotMapped]
        public List<bank> getBanks
        {
            get
            {
                orestEntities dbOrest = new orestEntities();                
                return dbOrest.bank.ToList();
            }
            private set
            {
            }
        }


        [Required(ErrorMessage = "Номер счета должен быть указан")]
        public string InvoiceNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public decimal InvoiceSumma { get; set; }                       // Общая сумма в оплату

        public int? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }    // валюта счета

        [NotMapped]
        public string GetCurrencyName
        {
            get
            {
                ApplicationDbContext db = new ApplicationDbContext();
                if (CurrencyId != null)
                {
                    return db.Currencies.Find(CurrencyId).CurrencyName;
                }
                else
                    return db.Currencies.Find(1).CurrencyName;

            }
            private set { }
        }
        [NotMapped]
        public SelectList GetCurrencies
        {
            get
            {
                ApplicationDbContext db = new ApplicationDbContext();
                return new SelectList(db.Currencies.ToList(), "Id", "CurrencyName");

            }
            private set { }
        }


        public string Comment { get; set; }

        public DateTime? WhenCreated { get; set; }          // Когда создан
        public virtual ApplicationUser whoAddThis { get; set; }      // Кто добавил запись
        public virtual ApplicationUser whoMadeLastChanges { get; set; }     // Кто вносил посл.изменения
        public DateTime? WhenEdited { get; set; }          // Когда вносились изменения

        public bool InvoiceChecked { get; set; }           //Инвойс проверен по цене и содержанию
        public bool PaymentApproved { get; set; }          // Платеж подтвержден руководителем (может быть подтвержден без проверки)        
        public bool PaymentDone { get; set; }              // Платеж выполнен (может осуществляться только после подтвеждения рук.)

        public virtual ApplicationUser InvoiceCheckedUser { get; set; }
        public virtual ApplicationUser PaymentApprovedUser { get; set; }

        public string DocumentUrl { get; set; }
        public virtual List<PaymentsDocument> PaymentsDocuments { get; set; }

        //[NotMapped]
        //public string CheckMessage { get; set; }

        [NotMapped]
        public bool isInvoiceFound { get; set; }

        //Показывает что общая сумма счёта правильная
        [NotMapped]
        public bool isInvoiceSummaCorrect { get; set; }

        //Показывает что общая сумма платежей правильная (если у всех платежей флаг isPaymentSummaCorrect РАВЕН true)
        [NotMapped]
        public bool isInvoicePaymentsCorrect { get; set; }

        //Показывает что все платежи по счёту проведены
        [NotMapped]
        public bool isInvoiceSummaComplete { get; set; }

        [NotMapped]
        public int SypplierInvoiceOrestId { get; set; } // id счета-фактуры поставщика - prh.tp = 5
        [NotMapped]
        public List<prh> GetAllPaymentsBySypplyerInvoiceOrestId // возвращает все платежи по текущему счету-фактуре поставщика
        {
            get
            {
                orestEntities dbOrest = new orestEntities();
                return dbOrest.prh.Where(prh => prh.rsh == SypplierInvoiceOrestId).ToList();
            }
            private set
            {

            }
        }

    }
}