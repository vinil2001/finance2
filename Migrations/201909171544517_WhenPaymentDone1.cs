namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhenPaymentDone1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "WhenPaymentDone", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "WhenPaymentDone");
        }
    }
}
