using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class SheetDetail
    {
        public int Id { get; set; }
        [Required]
        public int DetailNumber { get; set; }
        [Required]
        public string DetailName { get; set; }
        [Required]
        public double DetailLength { get; set; }
        [Required]
        public double DetailWidth { get; set; }
       
        //public double DetailAmount { get; set; }
        public double DetailThickness { get; set; }
       /* public double DetailsSquare { get; set; } */ // length*width*amount
        public virtual ICollection<DetailCalculation> DetailCalculations { get; set; }
    }
}