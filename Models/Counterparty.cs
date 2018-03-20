using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Counterparty // контрагент, может быть Плательщик либо Получатель оплаты (Покупатель, Поставщик)
    {
        public int Id { get; set; }
        public long? IdOrest { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } // полное официальное имя
        public bool VATPayer { get; set; } // плательщик НДС или нет
        public string CodVATPayer {get;set;} // код плательщика НДС
        public string VATCertificateNumber { get; set; } // номер свидетельства плательщика НДС
        public string BankName { get; set; }
        public string AccountNumber { get; set;}
        public string BankMFO { get; set; }
        //public virtual int BankId { get; set; }
        //public virtual Bank Bank { get; set; }
        public string EDRPO { get; set; }
        public string LegalAdress { get; set; } // Адресс юридический
        public string ActualAddress { get; set; } // Фактический адресс
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Comment { get; set; }
        public int? Discount { get; set; }
        public virtual ICollection<Incoming> Incomings { get; set; }
        public int? OwneshipTypeId { get; set; }
        public virtual OwnershipType OwneshipType { get; set; }

    }
}