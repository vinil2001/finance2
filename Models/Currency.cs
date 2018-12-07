using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }

        public virtual ICollection<PaymentStatement> PaymentStatements { get; set; }

    }
}