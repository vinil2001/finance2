namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentRenew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Payments", "PaymentComment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "PaymentComment", c => c.String());
        }
    }
}
