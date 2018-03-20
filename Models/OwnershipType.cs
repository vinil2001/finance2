using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class OwnershipType // тип собственности ТОВ, ПП и т.д.
    {
        public int Id { get; set; }
        public string OwnershipTypeName { get; set; }
        public virtual ICollection<Counterparty> Counterpartys { get; set; }
    }
}