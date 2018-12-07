namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatementCurrency : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PaymentStatements", name: "CurrencyInvoiceSumma_Id", newName: "Currency_Id");
            RenameIndex(table: "dbo.PaymentStatements", name: "IX_CurrencyInvoiceSumma_Id", newName: "IX_Currency_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PaymentStatements", name: "IX_Currency_Id", newName: "IX_CurrencyInvoiceSumma_Id");
            RenameColumn(table: "dbo.PaymentStatements", name: "Currency_Id", newName: "CurrencyInvoiceSumma_Id");
        }
    }
}
