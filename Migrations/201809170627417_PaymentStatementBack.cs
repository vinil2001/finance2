namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatementBack : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.PaymentStatements", "WhoAddThis_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentStatements", new[] { "CurrencyInvoiceSumma_CurrencyId" });
            DropIndex("dbo.PaymentStatements", new[] { "WhoAddThis_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "whoMadeLastChanges_Id" });
            AddColumn("dbo.PaymentStatements", "PaymentWhoAddThis", c => c.String());
            DropColumn("dbo.PaymentStatements", "PaymentWhenEdited");
            DropColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId");
            DropColumn("dbo.PaymentStatements", "WhoAddThis_Id");
            DropColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            DropTable("dbo.Currencies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.CurrencyId);
            
            AddColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "WhoAddThis_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", c => c.Int());
            AddColumn("dbo.PaymentStatements", "PaymentWhenEdited", c => c.DateTime(nullable: false));
            DropColumn("dbo.PaymentStatements", "PaymentWhoAddThis");
            CreateIndex("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            CreateIndex("dbo.PaymentStatements", "WhoAddThis_Id");
            CreateIndex("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId");
            AddForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "WhoAddThis_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "CurrencyInvoiceSumma_CurrencyId", "dbo.Currencies", "CurrencyId");
        }
    }
}
