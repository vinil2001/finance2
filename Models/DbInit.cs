using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finance.Models;
using System.Data.Entity;

namespace Finance.Models
{
    public class DbInit
    {
        public static void Init ()
        {
            ApplicationDbContext Db = new ApplicationDbContext();

            Bank newBank = new Bank() { Id = 0, BankName = "PrivatBank" };
            Bank newBank2 = new Bank() { Id = 1, BankName = "UkrSibBank" };
            Db.Banks.Add(newBank);
            Db.Banks.Add(newBank2);

            OwnershipType newOwnershipType = new OwnershipType() { Id = 0, OwnershipTypeName = "TOV" };
            OwnershipType newOwnershipType2 = new OwnershipType() { Id = 1, OwnershipTypeName = "PP" };
            Db.OwnershipTypes.Add(newOwnershipType);
            Db.OwnershipTypes.Add(newOwnershipType2);

            Counterparty newCounterparty = new Counterparty() { Name = "MetalHolding", OwneshipType = newOwnershipType };
            Counterparty newCounterparty2 = new Counterparty() { Name = "Парті", OwneshipType = newOwnershipType };
            Db.Counterparties.Add(newCounterparty);
            Db.Counterparties.Add(newCounterparty2);

            IncomingCategory newIncomingCategory = new IncomingCategory() { Id = 0, IncomingCategoryName = "Производство" };
            IncomingCategory newIncomingCategory2 = new IncomingCategory() { Id = 1, IncomingCategoryName = "Торговля" };
            Db.IncomingCategorys.Add(newIncomingCategory);
            Db.IncomingCategorys.Add(newIncomingCategory2);

            IncomingType newIncomingType = new IncomingType() { Id = 0, TypeName = "Технологическое" };
            IncomingType newIncomingType2 = new IncomingType() { Id = 1, TypeName = "ВЭД" };
            Db.IncomingTypes.Add(newIncomingType);
            Db.IncomingTypes.Add(newIncomingType2);

            WayOfPayment newWayOfPayment = new WayOfPayment() { Id = 0, WayOfPaymentName = "Bank" };
            WayOfPayment newWayOfPayment2 = new WayOfPayment() { Id = 1, WayOfPaymentName = "Cash" };
            Db.WayOfPayments.Add(newWayOfPayment);
            Db.WayOfPayments.Add(newWayOfPayment2);


            Incoming newIncoming = new Incoming()
            {
                Id = 0,
                Bank = newBank,
                Counterparty = newCounterparty,
                IncomingData = DateTime.Now,
                IncomingCategory = newIncomingCategory,
                IncomingSum = 23456,
                IncomingType = newIncomingType,
                InvoiceData = DateTime.Now,
                InvoiceNumber = 989898,
                Name = " ",
                WayOfPayment = newWayOfPayment
            };
            Db.Incomings.Add(newIncoming);
            Db.SaveChanges();
        }
        
    }
}