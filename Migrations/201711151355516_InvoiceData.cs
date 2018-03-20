namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incomings", "InvoiceData", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incomings", "InvoiceData", c => c.DateTime());
        }
    }
}
