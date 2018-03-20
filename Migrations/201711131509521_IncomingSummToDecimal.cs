namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomingSummToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incomings", "IncomingSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incomings", "IncomingSum", c => c.Double(nullable: false));
        }
    }
}
