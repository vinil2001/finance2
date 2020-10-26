namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsPaymentPartialPaymentCheckedApprovedDone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "PartialPaymentChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "PartialPaymentApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "PartialPaymentApproved");
            DropColumn("dbo.Payments", "PartialPaymentChecked");
        }
    }
}
