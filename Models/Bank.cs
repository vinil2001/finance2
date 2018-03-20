using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankMFO {get; set;}
        public virtual ICollection<Incoming> Incomings { get; set; }
        public virtual ICollection<AccNumber> AccountNumbers { get; set; }
        //public virtual ICollection<Counterparty> Counterparties { get; set; }
    }
}