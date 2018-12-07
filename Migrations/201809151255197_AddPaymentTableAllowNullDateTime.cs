namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentTableAllowNullDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "PaymentWhenEdited", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "PaymentWhenEdited", c => c.DateTime(nullable: false));
        }
    }
}
