namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatementUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.CurrencyId);
            
            AddColumn("dbo.PaymentStatements", "PaymentWhenEdited", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", c => c.Int());
            AddColumn("dbo.PaymentStatements", "WhoAddThis_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId");
            CreateIndex("dbo.PaymentStatements", "WhoAddThis_Id");
            CreateIndex("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            AddForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", "dbo.Currencies", "CurrencyId");
            AddForeignKey("dbo.PaymentStatements", "WhoAddThis_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.PaymentStatements", "PaymentWhoAddThis");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhoAddThis", c => c.String());
            DropForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "WhoAddThis_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", "dbo.Currencies");
            DropIndex("dbo.PaymentStatements", new[] { "whoMadeLastChanges_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "WhoAddThis_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "CurrencyInvoiceSumma_CurrencyId" });
            DropColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            DropColumn("dbo.PaymentStatements", "WhoAddThis_Id");
            DropColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId");
            DropColumn("dbo.PaymentStatements", "PaymentWhenEdited");
            DropTable("dbo.Currencies");
        }
    }
}
