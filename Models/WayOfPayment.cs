using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class WayOfPayment
    {
        public int Id { get; set; }
        public string WayOfPaymentName { get; set; } // карточка, б/н др.

        public virtual WayOfPayment ParentWayOfPayment { get; set; }
        public virtual ICollection<WayOfPayment> WayOfPayments { get; set; }
    }
}