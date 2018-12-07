using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class RemovedCounterparty
    {
        public int RemovedCounterpartyId { get; set; }
        public virtual long OrestCounterpartyId { get; set; }
        public string OrestCounterpartyName { get; set; }
        [NotMapped]
        public klt RemovedKlt { get; set; }
        public string WhoRemoveIt { get; set; }
        public DateTime WhenRemoved { get; set; }
        public string Comment { get; set; }
    }
}