namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountrapartyNameToPaymentStatement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "Counterparty_Id", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentStatements", "Counterparty_Id1", c => c.Int());
            CreateIndex("dbo.PaymentStatements", "Counterparty_Id1");
            AddForeignKey("dbo.PaymentStatements", "Counterparty_Id1", "dbo.Counterparties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentStatements", "Counterparty_Id1", "dbo.Counterparties");
            DropIndex("dbo.PaymentStatements", new[] { "Counterparty_Id1" });
            DropColumn("dbo.PaymentStatements", "Counterparty_Id1");
            DropColumn("dbo.PaymentStatements", "Counterparty_Id");
        }
    }
}
