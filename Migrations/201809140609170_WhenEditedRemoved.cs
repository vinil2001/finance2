namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhenEditedRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentStatements", "PaymentwhoMadeLastChanges");
            DropColumn("dbo.PaymentStatements", "PaymentWhenEdited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhenEdited", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentStatements", "PaymentwhoMadeLastChanges", c => c.String());
        }
    }
}
