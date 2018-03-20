using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Finance.Models; 

namespace Finance.Controllers
{
<<<<<<< HEAD
    [Authorize]
=======
    //[Authorize]  // Temporary commented. To do: Uncomment !!!
>>>>>>> paymentSatementOnly
    public class HomeController : Controller
    {

        ApplicationDbContext Db = new ApplicationDbContext();

         
        public ActionResult Index()
        {
            //if (!User.IsInRole("Developer"))
            //    return RedirectToAction("Index", "PaymentStatements"); // Temporary commented. To do: Uncomment !!!

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

   
        public ActionResult MenuBar()
        {
            return PartialView();
        }
    }
}