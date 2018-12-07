namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceSummaToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentStatements", "InvoiceNumber", c => c.String(nullable: false));
            AlterColumn("dbo.PaymentStatements", "InvoiceSumma", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentStatements", "InvoiceSumma", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PaymentStatements", "InvoiceNumber", c => c.String());
        }
    }
}
