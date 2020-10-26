namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhenCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "WhenCreated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "WhenCreated");
        }
    }
}
