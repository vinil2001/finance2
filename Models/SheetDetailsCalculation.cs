using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class SheetDetailsCalculation
    {
        public int Id { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public double MaterialPrice { get; set; } //Сумма без НДС за метериал из счета поставщика
        [Required]
        public double CuttingPrice { get; set; } // Сумма без НДС за порезку из счета поставщика
        [Required]
        public double Area { get; set; } // площадь листа в м.кв. из которого будут вырезаться заготовки
        [Required]
        public int SheetThickness { get; set; }
        public double WasteCoeficcient { get; set; }  // коэфициент отхода, расчтывется для каждого случая отдельно
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime CalculationDate { get; set; }
        public double[] DetailsSquares { get; set; } // // Площадь деталей. Нужна для вычисления коэф.отхода - sheetDetailsCalculation.WasteCoeficcient 
        public double[] ItemsPercentages { get; set; } // доля каждой детали от площади листа в %

        public double PriceFluctuationCoefficient { get; set; } // коэффициент колебания цены, закладывается в стоимость изделия

        public virtual ICollection<DetailCalculation> DetailsCalculation { get; set; }
    }
}