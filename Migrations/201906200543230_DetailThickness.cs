namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetailThickness : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SheetDetails", "DetailThickness", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SheetDetails", "DetailThickness");
        }
    }
}
