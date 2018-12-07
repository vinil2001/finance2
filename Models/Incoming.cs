using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class Incoming
    {
        public int Id { get; set; }
        public long KshId { get; set; }
        public long BkhId { get; set; }
        public virtual IncomingCategory IncomingCategories { get; set; }
        public virtual WayOfPayment WayOfPayments { get; set; }
    }
}