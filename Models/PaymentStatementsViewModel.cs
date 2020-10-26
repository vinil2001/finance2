using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class PaymentStatementsViewModel
    {
        public IEnumerable<PaymentStatement> PaymentDocumnets { get; set; }
        public PaginationPageInfo PageInfo { get; set; }
    }

}