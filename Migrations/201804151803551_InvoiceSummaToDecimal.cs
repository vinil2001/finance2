namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceSummaToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentStatements", "InvoiceSumma", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentStatements", "InvoiceSumma", c => c.String(nullable: false));
        }
    }
}
