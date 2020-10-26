using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class DetailCalculation
    {
        public int Id { get; set; }
        public virtual SheetDetail Detail { get; set; }
        public virtual SheetDetailsCalculation Calculation { get; set; }
        public double Amount { get; set; }
        public double DetailsSquare { get; set; }

        public double WorkPrice { get; set; }      // Стоимость порезки детали - рассчитывается в контроллере
        public double MaterialPrice { get; set; }      // Стоимость за материалл - рассчитывается в контроллере 
        public double DetailPrice { get; set; }        // Цена за деталь (WorkPrice+MaterialPrice)- рассчитывается в контроллере
    }
}