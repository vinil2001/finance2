namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoToNewVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhenEdited", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "PaymentWhenEdited");
        }
    }
}
