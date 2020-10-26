using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class BankTransactionsFromOrestdb
    {
        public long KassaIncomingId { get; set; }          //ksh.id
                                                           //public int CurrencyId { get; set; }                //ksh.vlt + bkh.vlt - валюта, 1 - гривна, 3 - евро.

        [DisplayName("Курс валюты")]
        public double CurrencyRate { get; set; }          //ksh.curs + bkh.curs - курс валюты
        public long? PaymentSum { get; set; }              //ksh.sm  -- сумма платежа
        public long TotalPaymentSumm { get; set; }        //ksh.sd = ksh.sm * ksh.curs; + bkn.sd - сумма платежа
        public string InvoiceNumber { get; set; }         //ksh.ncht + bkn.ncht !!!
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DocumentCreated { get; set; }     //ksh.datd + bkn.datd
        public DateTime DucumentEdited { get; set; }      //ksh.dusr + bkh.dusr - дата редактирования 
        public long PayerId { get; set; }                 //ksh.klt + bkn.klt

        public string NameKlt { get; set; }     //Название клиента  !!!!!!!

        public string Comment { get; set; }               //ksh.comt

        // Select bkh Where bkh.tp = 1 || 5 - Выбрать только приходы

        public long BankIncomingId { get; set; }         //bkn.id

        public string DocumentNumbeInOrestDb { get; set; }   //bkh.ndoc -- always == bkn.num
                                                             //---------------------
        public int BankId { get; set; }                   // bkh.bank - в какой банк зашли деньги (напр.1 - Укрсиб., 2 - Приват...)


        public string NameBank { get; set; }  //Название банка  !!!!!!!    // bkh.bank - в какой банк зашли деньги (напр.1 - Укрсиб., 2 - Приват...)

        public double SumCurrencyForSale { get; set; }   // bkh.svl -- сумма валюты которая продается согласно закону о прод.вал.выручки (50% до 13 декабря 2018)
        public double CurrencyRateNBU { get; set; }       // bkh.curs2 - курс НБУ
        public double SumIncomingCurrency { get; set; }  // bkh.svl2 - сумма вал.полученная от плательщика
        public double SumCurrencyAfterSale { get; set; }  // bkh.sdv сумма зачисляемая на вал.счет после обяз.продажи
                                                          // bkh.brsh --- ?????
                                                          //public virtual IncomingCategory IncomingCategories { get; set; } // добавить эти связи, выполнить миграции
                                                          //public virtual WayOfPayment WayOfPayments { get; set; }
    }
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
}