namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SheetDetailFiealdsSetRequered : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SheetDetailsCalculations", "InvoiceNumber", c => c.String(nullable: false));
            AlterColumn("dbo.SheetDetails", "DetailName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SheetDetails", "DetailName", c => c.String());
            AlterColumn("dbo.SheetDetailsCalculations", "InvoiceNumber", c => c.String());
        }
    }
}
