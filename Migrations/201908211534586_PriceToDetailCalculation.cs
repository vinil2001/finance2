namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceToDetailCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetailCalculations", "WorkPrice", c => c.Double(nullable: false));
            AddColumn("dbo.DetailCalculations", "MaterialPrice", c => c.Double(nullable: false));
            AddColumn("dbo.DetailCalculations", "DetailPrice", c => c.Double(nullable: false));
            DropColumn("dbo.SheetDetails", "WorkPrice");
            DropColumn("dbo.SheetDetails", "MaterialPrice");
            DropColumn("dbo.SheetDetails", "DetailPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SheetDetails", "DetailPrice", c => c.Double(nullable: false));
            AddColumn("dbo.SheetDetails", "MaterialPrice", c => c.Double(nullable: false));
            AddColumn("dbo.SheetDetails", "WorkPrice", c => c.Double(nullable: false));
            DropColumn("dbo.DetailCalculations", "DetailPrice");
            DropColumn("dbo.DetailCalculations", "MaterialPrice");
            DropColumn("dbo.DetailCalculations", "WorkPrice");
        }
    }
}
