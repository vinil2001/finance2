using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class OutgoingsCategory // Категория затрат
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } 
        public virtual List<OutgoingsType> OutgoingsTypes { get; set; } // тип затрат
    }
}