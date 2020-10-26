namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhenPaymentDoneAsNulable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentStatements", "WhenPaymentDone", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentStatements", "WhenPaymentDone", c => c.DateTime(nullable: false));
        }
    }
}
