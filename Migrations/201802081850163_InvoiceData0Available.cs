namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceData0Available : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incomings", "InvoiceData", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incomings", "InvoiceData", c => c.DateTime(nullable: false));
        }
    }
}
