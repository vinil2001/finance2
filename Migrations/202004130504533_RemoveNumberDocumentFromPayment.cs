namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNumberDocumentFromPayment : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Payments", "numberDocument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "numberDocument", c => c.String());
        }
    }
}
