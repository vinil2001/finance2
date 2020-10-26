namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberDocumentInPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "numberDocument", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "numberDocument");
        }
    }
}
