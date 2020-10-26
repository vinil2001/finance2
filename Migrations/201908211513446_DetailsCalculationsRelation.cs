namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetailsCalculationsRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetail_Id", "dbo.SheetDetails");
            DropForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetailsCalculation_Id", "dbo.SheetDetailsCalculations");
            DropIndex("dbo.SheetDetailSheetDetailsCalculations", new[] { "SheetDetail_Id" });
            DropIndex("dbo.SheetDetailSheetDetailsCalculations", new[] { "SheetDetailsCalculation_Id" });
            CreateTable(
                "dbo.DetailCalculations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        DetailsSquare = c.Double(nullable: false),
                        Calculation_Id = c.Int(),
                        Detail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SheetDetailsCalculations", t => t.Calculation_Id)
                .ForeignKey("dbo.SheetDetails", t => t.Detail_Id)
                .Index(t => t.Calculation_Id)
                .Index(t => t.Detail_Id);
            
            DropColumn("dbo.SheetDetails", "DetailAmount");
            DropColumn("dbo.SheetDetails", "DetailsSquare");
            DropTable("dbo.SheetDetailSheetDetailsCalculations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SheetDetailSheetDetailsCalculations",
                c => new
                    {
                        SheetDetail_Id = c.Int(nullable: false),
                        SheetDetailsCalculation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SheetDetail_Id, t.SheetDetailsCalculation_Id });
            
            AddColumn("dbo.SheetDetails", "DetailsSquare", c => c.Double(nullable: false));
            AddColumn("dbo.SheetDetails", "DetailAmount", c => c.Double(nullable: false));
            DropForeignKey("dbo.DetailCalculations", "Detail_Id", "dbo.SheetDetails");
            DropForeignKey("dbo.DetailCalculations", "Calculation_Id", "dbo.SheetDetailsCalculations");
            DropIndex("dbo.DetailCalculations", new[] { "Detail_Id" });
            DropIndex("dbo.DetailCalculations", new[] { "Calculation_Id" });
            DropTable("dbo.DetailCalculations");
            CreateIndex("dbo.SheetDetailSheetDetailsCalculations", "SheetDetailsCalculation_Id");
            CreateIndex("dbo.SheetDetailSheetDetailsCalculations", "SheetDetail_Id");
            AddForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetailsCalculation_Id", "dbo.SheetDetailsCalculations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetail_Id", "dbo.SheetDetails", "Id", cascadeDelete: true);
        }
    }
}
