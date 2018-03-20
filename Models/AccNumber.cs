using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class AccNumber
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
        public string AccountNumber { get; set; }
    }
}