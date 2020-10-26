namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrestPaymentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "OrestPaymentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "OrestPaymentId");
        }
    }
}
