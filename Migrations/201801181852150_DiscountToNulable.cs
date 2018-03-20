namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscountToNulable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counterparties", "Discount", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counterparties", "Discount", c => c.Int(nullable: false));
        }
    }
}
