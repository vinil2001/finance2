using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Outgoing // Исходящие оплаты: затраты, Банк, касса расход
    {
        public int Id { get; set; }
        public string Name { get; set; } // название 
        public DateTime? OutgoingData { get; set; } 
        //public List<Counterparty> Counterpartys { get; set; } // контрагент(получатель)
        //public List<Bank> Banks { get; set; } // банк с которого делался платеж
        public double? OutgoingSum { get; set; } // сумма исходящей оплаты
        public int? InvoiceNumber { get; set; } // номер счета    
        public DateTime? InvoiceData { get; set; } // дата счета   


    }
}