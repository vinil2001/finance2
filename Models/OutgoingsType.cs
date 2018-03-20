using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class OutgoingsType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public virtual OutgoingsCategory Category { get; set; } // ссылка на род. категорию
    }
}