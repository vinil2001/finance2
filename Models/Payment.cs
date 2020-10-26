using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finance.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrestPaymentId { get; set; } // для хранения bkh.id

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public decimal Summa { get; set; }        // сумма в оплату
        //public string PaymentComment { get; set; }

        public int PaymentStatementId { get; set; }
        public virtual PaymentStatement PaymentStatement { get; set; }           // основной платежный документ

        public virtual ApplicationUser PaymentWhoAddThis { get; set; }      // Кто добавил запись
        public virtual ApplicationUser PaymentwhoMadeLastChanges { get; set; }     // Кто вносил посл.изменения
        public DateTime? PaymentWhenEdited { get; set; }          // Когда вносились изменения

        /*public string numberDocument { get; set; }*/ //Bkh.nDoc

        public bool PartialPaymentChecked { get; set; }         // Платеж проверен
        public bool PartialPaymentApproved { get; set; }           //Платеж подтвержден
        public bool PaymentPaymentDone { get; set; }              // Платеж выполнен (модет осуществляться только после подтвеждения рук.)

        public virtual ApplicationUser PaymentPaymentDoneUser { get; set; }

        public string MySQLBankName { get; set; }
        public long MySQLBankId { get; set; }
        [NotMapped]
        public List<bank> getBanks
        {
            get
            {
                orestEntities dbOrest = new orestEntities();
                bank defaultValueBank = new bank();
                defaultValueBank.id = 0;
                defaultValueBank.name = "Выберите банк";
                List<bank> banks = dbOrest.bank.ToList();
                banks.Add(defaultValueBank);
                return banks;
            }
            private set
            {
            }
            //get
            //{
            //    orestEntities dbOrest = new orestEntities();
            //    var sl = new SelectList(dbOrest.bank.ToList(), "id", "name");
            //    return new SelectList(dbOrest.bank.ToList(), "id", "name");
            //}
            //private set
            //{
            //}
        }

        //Показывает что общая сумма платежа для данного счёта присутствует в базе орест
        [NotMapped]
        public bool isPaymentSummaCorrect { get; set; }
        // поле хранящее id платежа из бд орест для того что бы не отмесать его повторно в CheckPayments()
        [NotMapped]
        public int orestOutgoingPaymentId { get; set; } 
    }
}