namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWhenPaymentDone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "WhenPaymentDone", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "WhenPaymentDone");
        }
    }
}
