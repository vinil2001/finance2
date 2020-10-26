using Finance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Finance.Controllers
{
    public class SheetDetailsCalculationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: SheetDetailsCalculations
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Calculation(SheetDetailsCalculation sheetDetailsCalculation, SheetDetailViewModels sheetDetailViewModels)
        {
            DetailCalculation[] Details = Calculate(sheetDetailsCalculation, sheetDetailViewModels);

            //sheetDetailsCalculation.DetailsCalculation.details = details;
            DetailsPricesAndWasteCoef detailsPricesAndWasteCoef = new DetailsPricesAndWasteCoef();
            detailsPricesAndWasteCoef.DetailsPrices = Details.Select(i => i.DetailPrice).ToArray();
            detailsPricesAndWasteCoef.WasteCoef = sheetDetailsCalculation.WasteCoeficcient;

            //return Json(res);
            return Json(detailsPricesAndWasteCoef);
        }

        private static DetailCalculation[] Calculate(SheetDetailsCalculation sheetDetailsCalculation, SheetDetailViewModels sheetDetailViewModels)
        {
            DetailCalculation[] Details = new DetailCalculation[sheetDetailViewModels.DetailNumber.Length];

            sheetDetailsCalculation.DetailsSquares = new double[sheetDetailViewModels.DetailNumber.Length];
            for (int i = 0; i < sheetDetailViewModels.DetailNumber.Length; i++)
            {
                sheetDetailsCalculation.DetailsSquares[i] = (sheetDetailViewModels.DetailAmount[i] * sheetDetailViewModels.DetailLength[i] * sheetDetailViewModels.DetailWidth[i]) / 1000000;
            }
            // Рассчитываем коэф.отхода по всему заказу
            sheetDetailsCalculation.WasteCoeficcient = sheetDetailsCalculation.Area / sheetDetailsCalculation.DetailsSquares.Sum();

            // доля каждой детали от площади листа в %
            //decimal[] ItemsPercentages = new decimal[sheetDetailViewModels.DetailNumber.Length];
            sheetDetailsCalculation.ItemsPercentages = new double[sheetDetailViewModels.DetailNumber.Length];

            for (int i = 0; i < sheetDetailViewModels.DetailNumber.Length; i++)
            {
                sheetDetailsCalculation.ItemsPercentages[i] = (sheetDetailsCalculation.DetailsSquares[i] * sheetDetailsCalculation.WasteCoeficcient) / sheetDetailsCalculation.Area;
                // считаем стоимость за материалл для каждой детали

                Details[i] = new DetailCalculation();
                Details[i].MaterialPrice = sheetDetailsCalculation.MaterialPrice * sheetDetailsCalculation.ItemsPercentages[i];
                // считаем стоимость работы для каждой детали
                Details[i].WorkPrice = sheetDetailsCalculation.CuttingPrice * sheetDetailsCalculation.ItemsPercentages[i];
                // считаем стоимость каждой детали
                Details[i].DetailPrice = (Details[i].MaterialPrice + Details[i].WorkPrice) / sheetDetailViewModels.DetailAmount[i];
            }

            return Details;
        }

        [HttpPost]
        public ActionResult SaveCalculation(SheetDetailsCalculation sheetDetailsCalculation, SheetDetailViewModels sheetDetailViewModels/*, int? detId*/)
        {
            SheetDetail[] details = new SheetDetail[sheetDetailViewModels.DetailNumber.Length];
            for (int j = 0; j < details.Length; j++)
            {
                if(sheetDetailViewModels.DetailId[j] == -1)
                {
                    details[j] = new SheetDetail();
                    details[j].DetailLength = sheetDetailViewModels.DetailLength[j];
                    details[j].DetailName = sheetDetailViewModels.DetailName[j];
                    details[j].DetailNumber = sheetDetailViewModels.DetailNumber[j];
                    details[j].DetailWidth = sheetDetailViewModels.DetailWidth[j];
                    details[j].DetailThickness = sheetDetailsCalculation.SheetThickness;
                    db.SheetDetails.Add(details[j]);
                }
                else
                {
                    details[j] = db.SheetDetails.Find(sheetDetailViewModels.DetailId[j]);
                }

                DetailCalculation detailCalculation = new DetailCalculation();
                detailCalculation.Calculation = sheetDetailsCalculation;
                detailCalculation.Detail = details[j]; 
                detailCalculation.Amount = sheetDetailViewModels.DetailAmount[j];
                detailCalculation.DetailsSquare = detailCalculation.Amount * details[j].DetailLength * details[j].DetailWidth;
                detailCalculation.DetailPrice = sheetDetailViewModels.DetailPrice[j];

                db.DetailsCalculations.Add(detailCalculation);
            }

            db.SheetDetailsCalculations.Add(sheetDetailsCalculation);
            db.SaveChanges();

            return RedirectToAction("GetAllCalculations", db.SheetDetailsCalculations.ToList());
        }

        public ActionResult GetAllCalculations()
        {
            return View(db.SheetDetailsCalculations.ToList());
        }

        public ActionResult GetAllDetails()
        {
            var det = db.SheetDetails.ToList();
            return View(db.SheetDetails.ToList());
        }

        [HttpGet]
        public ActionResult EditCalculation(int id)
        {
            var editedCalc = db.SheetDetailsCalculations.Find(id);
            return View(db.SheetDetailsCalculations.Find(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditCalculation(SheetDetailsCalculation Calculation, SheetDetailViewModels sheetDetailViewModels /*int[] DetailId*/)
        {
            if (ModelState.IsValid)
            {
                SheetDetailsCalculation currentCalculation = await db.SheetDetailsCalculations.FindAsync(Calculation.Id);
                currentCalculation.Area = Calculation.Area;
                currentCalculation.CalculationDate = Calculation.CalculationDate;
                currentCalculation.CuttingPrice = Calculation.CuttingPrice;
                currentCalculation.DetailsSquares = Calculation.DetailsSquares;
                currentCalculation.InvoiceDate = Calculation.InvoiceDate;
                currentCalculation.InvoiceNumber = Calculation.InvoiceNumber;
                currentCalculation.ItemsPercentages = Calculation.ItemsPercentages;
                currentCalculation.MaterialPrice = Calculation.MaterialPrice;
                currentCalculation.SheetThickness = Calculation.SheetThickness;
                currentCalculation.WasteCoeficcient = Calculation.WasteCoeficcient;

                currentCalculation.DetailsCalculation.Clear();

                for(var i = 0; i < sheetDetailViewModels.DetailId.Length; i++)
                {
                    DetailCalculation detailCalculation = new DetailCalculation();
                    currentCalculation.DetailsCalculation.Add(detailCalculation);
                    if (sheetDetailViewModels.DetailId[i] != -1)
                    {
                        detailCalculation.Detail = db.SheetDetails.Find(sheetDetailViewModels.DetailId[i]);
                    }
                    else
                    {
                        SheetDetail newDetail = new SheetDetail();
                        newDetail.DetailLength = sheetDetailViewModels.DetailLength[i];
                        newDetail.DetailName = sheetDetailViewModels.DetailName[i];
                        newDetail.DetailNumber = sheetDetailViewModels.DetailNumber[i];
                        newDetail.DetailThickness = currentCalculation.SheetThickness;
                        newDetail.DetailWidth = sheetDetailViewModels.DetailWidth[i];
                        db.SheetDetails.Add(newDetail);
                        detailCalculation.Detail = newDetail;
                    }
                    detailCalculation.Amount = sheetDetailViewModels.DetailAmount[i]; ;
                    detailCalculation.DetailsSquare = (sheetDetailViewModels.DetailAmount[i] * sheetDetailViewModels.DetailLength[i] * sheetDetailViewModels.DetailWidth[i]) / 1000000;
                    detailCalculation.DetailPrice = sheetDetailViewModels.DetailPrice[i];
                }


                await db.SaveChangesAsync();

            }
            return RedirectToAction("GetAllCalculations");
        }

        class DetailInfo
        {
            public int ID { get; set; }
            public string DetailName { get; set; }
            public double DetailLength { get; set; }
            public double DetailWidth { get; set; }
        }

        [HttpGet]
        public JsonResult GetDetailsByName(string detailNameValue)
        {
            List<SheetDetail> DetailsList = db.SheetDetails.Where(n => n.DetailName.Contains(detailNameValue)).ToList();
            List<DetailInfo> DetailsInfo = new List<DetailInfo>();
            foreach (var i in DetailsList)
            {
                DetailInfo Detail = new DetailInfo();
                Detail.ID = i.Id;
                Detail.DetailName = i.DetailName;
                Detail.DetailLength = i.DetailLength;
                Detail.DetailWidth = i.DetailWidth;
                DetailsInfo.Add(Detail);
            }            
            return Json(DetailsInfo, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult Delete(int id)
        {
            //Для удаления калькуляции необходимо вначале удалить все связи с деталями (detailCalculations)
            
            SheetDetailsCalculation calculation = db.SheetDetailsCalculations.Find(id);
            List<DetailCalculation> detailCalculations = calculation.DetailsCalculation.ToList();
            for(var i =0; i<detailCalculations.Count; i++)
            {
                db.DetailsCalculations.Remove(detailCalculations[i]);
            }

            db.SheetDetailsCalculations.Remove(calculation);
            db.SaveChanges();
            return RedirectToAction("GetAllCalculations");
        }

        [HttpGet]
        public ActionResult GetDetailCalculationIfo(int detailId)
        {
            List<DetailCalculation> detailCalculations = db.DetailsCalculations.Where(calc => calc.Detail.Id == detailId).ToList();
            ViewBag.DetailName = db.SheetDetails.Find(detailId).DetailName;
            double avarageDetailPrice = 0;
            foreach (var i in detailCalculations)
            {
                avarageDetailPrice += i.DetailPrice;
            }
            avarageDetailPrice = avarageDetailPrice / detailCalculations.Count;
            ViewBag.AvarageDetailPrice = avarageDetailPrice;
            ViewBag.MinPrice = detailCalculations.Min(i => i.DetailPrice);
            ViewBag.MaxPrice = detailCalculations.Max(i => i.DetailPrice);
            return View(detailCalculations);
        }
    }
    public class SheetDetailViewModels
    {
        public int[] DetailNumber { get; set; }
        public string[] DetailName { get; set; }
        public double[] DetailLength { get; set; }
        public double[] DetailWidth { get; set; }
        public double[] DetailAmount { get; set; }
        public double[] DetailPrice { get; set; }
        public int[] DetailId { get; set; }
    }

    public class DetailsPricesAndWasteCoef
    {
        public double[] DetailsPrices { get; set; }
        public double WasteCoef { get; set; }
    }

}