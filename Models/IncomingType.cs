using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class IncomingType // статья прихода 
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public virtual ICollection<Incoming> Incomings { get; set; }
    

    }
}