namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currency : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_Id", c => c.Int());
            CreateIndex("dbo.PaymentStatements", "CurrencyInvoiceSumma_Id");
            AddForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_Id", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_Id", "dbo.Currencies");
            DropIndex("dbo.PaymentStatements", new[] { "CurrencyInvoiceSumma_Id" });
            DropColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_Id");
            DropTable("dbo.Currencies");
        }
    }
}
