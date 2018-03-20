using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finance.Models
{
    public class Incoming
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Поле должно быть установлено")]
        //[Display(Name = "Дата")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата получения' должно быть установлено")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IncomingData { get; set; }
      
        public int ?BankId { get; set; }
      
        [Display(Name = "Банк")]
        public virtual Bank Bank { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Плательщик должен быть выбран. Если плательщик новый, нажмите + для его добавления")]
        public int CounterpartyId { get; set; }

        
        [Display(Name = "Плательщик")]
        public virtual Counterparty Counterparty { get; set; }

        [Required(ErrorMessage = "Поле 'Сумма' должно быть установлено")]
        [Display(Name = "Сумма")]
        public decimal IncomingSum { get; set; } // приход сумма

        [HiddenInput(DisplayValue = false)]
        public int IncomingCategoryId { get; set; }

        //[Required(ErrorMessage = "Поле 'Категория' должно быть установлено")]
        [Display(Name = "Категория")]
        public virtual IncomingCategory IncomingCategory { get; set; }
        // категория для ден.поступлений: Про-во, Сервис, Торговля, Поставки и др.

        [HiddenInput(DisplayValue = false)]
        public int IncomingTypeId { get; set; }

        //[Required(ErrorMessage = "Поле 'Тип' должно быть установлено")]
        [Display(Name = "Тип")]
        public virtual IncomingType IncomingType { get; set; }
        // статья прихода 

        public int? InvoiceNumber { get; set; } // номер счета    

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceData { get; set; } // дата счета 

        [HiddenInput(DisplayValue = false)]
        public int WayOfPaymentId { get; set; }

        [Display(Name = "Форма оплаты")]
        public virtual WayOfPayment WayOfPayment { get; set; }
        // способ оплаты: на карту(может быть несколько карт), нал., безнал 

    }
}