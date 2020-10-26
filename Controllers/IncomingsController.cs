using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finance.Models;

namespace Finance.Controllers
{
    public class IncomingsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        static orestEntities dbOrest = new orestEntities();

        // В таблице bkn находятся такие типы записей (различающиеся полем tp):
        // 1) Банк-приход (перевод внутри старны в грн) - bkh.tp = 1;
        // 2) Банк-приход валюты при экспортных операциях - bkh.tp = 5;
        // 3) Банк-расход (перевод внутри старны в грн) - bkh.tp = 2;
        // 4) Банк-расход валюты при импортных операциях - bkh.tp = 4; 3 - заявка на покупку валюты на основе которой создается Банк-расход
        // 5) Отгрузка без 100% оплаты экспорт - bkh.tp = 6; -- пока только в двух случаях
        // У кассы-прихода ksh.tp = 1 !!!
        // Учитывая выше указанную расшифровку мне надо выбрать записи где bkh.tp = 1 || bkh.tp =5
        // tp - судя по всему имеет связь с таблицей _fin или fin или fin0 (одинаковые таблицы)
        // Только таблицы с именами _fin или fin или fin0 имеют 6 рядков - совпадающих по кол-ву с кол-м типов операций tp
        /*
         SELECT  TABLE_NAME, TABLE_ROWS
         FROM information_schema.TABLES
        */
        static List<BankTransactionsFromOrestdb> UnionOrestEntiry;

        public List<BankTransactionsFromOrestdb> ReturnAllIncomingsFromOrestDb()
        {
            return dbOrest.Database.SqlQuery<BankTransactionsFromOrestdb>(
                   @"SELECT  bkh.id AS BankIncomingId, bkh.ndoc AS DocumentNumbeInOrestDb, bkh.curs AS CurrencyRate, NULL AS PaymentSum, bkh.sd AS TotalPaymentSumm, bkh.ncht AS InvoiceNumber, bkh.datd AS DocumentCreated, bkh.dusr AS DucumentEdited, 
                   bkh.klt AS PayerId, bkh.comt AS Comment, klt.name AS NameKlt, bank.name AS NameBank, '0' AS type
                   FROM      bkh INNER JOIN
                   klt ON bkh.klt = klt.id INNER JOIN
                   bank ON bkh.bank = bank.id
                   WHERE   (bkh.tp = 1) OR
                   (bkh.tp = 5) OR
                   (bkh.tp = 6)
                   UNION
                   SELECT  ksh.id AS KassaIncomingId, ksh.ndoc AS DocumentNumbeInOrestDb, ksh.curs AS CurrencyRate, ksh.sm AS PaymentSum, ksh.sd AS TotalPaymentSumm, ksh.ncht AS InvoiceNumber, ksh.datd AS DocumentCreated, ksh.dusr AS DucumentEdited, 
                   ksh.klt AS PayerId, ksh.comt AS Comment, klt_1.name AS NameKlt, NULL AS NameBank, '1' AS type
                   FROM      ksh INNER JOIN
                   klt klt_1 ON ksh.klt = klt_1.id").ToList();
        }
       
        // GET: Incomings
        public ActionResult Index()
        {
            UnionOrestEntiry = ReturnAllIncomingsFromOrestDb().OrderByDescending(i=>i.BankIncomingId).Take(100).ToList();
            //return View(UnionOrestEntiry.Where(a => a.DocumentCreated >= DateTime.Today.AddMonths(-1) && a.DocumentCreated <= DateTime.Today));
            return View(UnionOrestEntiry);
        }

        public PartialViewResult SearchIncoming(string IncomingDataFrom, string IncomingDataTo, string searchRequest = null)
        {
            DateTime DataFrom = DateTime.ParseExact(IncomingDataFrom, "dd.MM.yyyy", null);
            DateTime DataTo = DateTime.ParseExact(IncomingDataTo, "dd.MM.yyyy", null);
            if (UnionOrestEntiry == null)
                UnionOrestEntiry = ReturnAllIncomingsFromOrestDb();
            var result = UnionOrestEntiry.Where(a => a.DocumentCreated >= DataFrom && a.DocumentCreated <= DataTo);
            if(result == null)
                UnionOrestEntiry = ReturnAllIncomingsFromOrestDb();
            if (!string.IsNullOrWhiteSpace(searchRequest))
                return PartialView(result.Where(i => i.NameKlt.Contains(searchRequest)));  
            else
                return PartialView(result);
        }
    }
}
 