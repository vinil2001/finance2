using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    // категория для ден.поступлений: Про-во, Сервис, Торговля, Поставки и др.
    public class IncomingCategory   
    {
        public int Id { get; set; }
        public string IncomingCategoryName { get; set; }
        public virtual IncomingCategory ParentIncomingCategory { get; set; }
        public virtual ICollection<IncomingCategory> IncomingCategories { get; set; }

        //public int ParentIncomingCategory { get; set; }
        //public virtual ICollection<Incoming> Incomings { get; set; }

    }
}