using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finance.Models;
using System.Web.Mvc;

namespace Finance.Models
{
    public class EditIncomingModel
    {
        public virtual Incoming EditedIncoming { get; set; }
        public int ?SelectedBankId { get; set; }
        public int ?SelectedWayOfPayment { get; set; }
        public int ?SelectedIncomingCategory { get; set; }
        public int ?SelectedIncomingType { get; set; }
        public SelectList Banks { get; set; }
        public SelectList Companies { get; set; }
        public SelectList Categories { get; set;}
        public SelectList Types { get; set; }
        public SelectList WayOfPayments { get; set; }
        public EditIncomingModel()
        {

        }
        public EditIncomingModel(Incoming incoming)
        {
            EditedIncoming = incoming;
            ApplicationDbContext db = new ApplicationDbContext();
            Banks = new SelectList(db.Banks, "Id", "BankName", EditedIncoming.BankId);
            Companies = new SelectList(db.Counterparties, "Id", "Name", EditedIncoming.CounterpartyId);
            Categories = new SelectList(db.IncomingCategorys, "Id", "IncomingCategoryName", EditedIncoming.IncomingCategoryId);
            Types = new SelectList(db.IncomingTypes, "Id", "TypeName", incoming.IncomingTypeId);
            WayOfPayments = new SelectList(db.WayOfPayments, "Id", "WayOfPaymentName", EditedIncoming.WayOfPaymentId);
            
            if(EditedIncoming.BankId != null)
            {
                SelectedBankId = db.Banks.Where(b => b.Id == incoming.Bank.Id).First().Id;
            }
            SelectedIncomingCategory = db.IncomingCategorys.Where(ic => ic.Id == incoming.IncomingCategory.Id).First().Id;
            SelectedIncomingType = db.IncomingTypes.Where(it => it.Id == incoming.IncomingType.Id).First().Id;
            SelectedWayOfPayment = db.WayOfPayments.Where(w => w.Id == incoming.WayOfPayment.Id).First().Id;
        }

    }
}