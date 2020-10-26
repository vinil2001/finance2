namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrestPrhId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "OrestPrhId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "OrestPrhId");
        }
    }
}
