using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class CounterpartySearchUnionModel
    {
       public int id { get; set; }
       public List<Counterparty> FinanceCounterparties { get; set; }
       public List<klt> OrestCounterparties { get; set; }
    }
}