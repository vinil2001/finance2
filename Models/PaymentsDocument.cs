using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class PaymentsDocument
    {
        public int Id { get; set; }
        public string DocumentUrl { get; set; }
        public virtual PaymentStatement PaymentStatement { get; set; }
    }
}