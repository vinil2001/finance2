namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class whenPaymentDoneRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentStatements", "WhenPaymentDone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "WhenPaymentDone", c => c.DateTime(nullable: false));
        }
    }
}
