using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public decimal Summa { get; set; }        // сумма в оплату
        public string PaymentComment { get; set; }

        public int PaymentStatementId { get; set; }
        public virtual PaymentStatement PaymentStatement { get; set; }           // основной платежный документ

        public virtual ApplicationUser PaymentWhoAddThis { get; set; }      // Кто добавил запись
        public virtual ApplicationUser PaymentwhoMadeLastChanges { get; set; }     // Кто вносил посл.изменения
        public DateTime? PaymentWhenEdited { get; set; }          // Когда вносились изменения

        public bool PaymentPaymentDone { get; set; }              // Платеж выполнен (модет осуществляться только после подтвеждения рук.)

        public virtual ApplicationUser PaymentPaymentDoneUser { get; set; }
    }
}