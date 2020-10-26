namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyCalculationToDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SheetDetailSheetDetailsCalculations",
                c => new
                    {
                        SheetDetail_Id = c.Int(nullable: false),
                        SheetDetailsCalculation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SheetDetail_Id, t.SheetDetailsCalculation_Id })
                .ForeignKey("dbo.SheetDetails", t => t.SheetDetail_Id, cascadeDelete: true)
                .ForeignKey("dbo.SheetDetailsCalculations", t => t.SheetDetailsCalculation_Id, cascadeDelete: true)
                .Index(t => t.SheetDetail_Id)
                .Index(t => t.SheetDetailsCalculation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetailsCalculation_Id", "dbo.SheetDetailsCalculations");
            DropForeignKey("dbo.SheetDetailSheetDetailsCalculations", "SheetDetail_Id", "dbo.SheetDetails");
            DropIndex("dbo.SheetDetailSheetDetailsCalculations", new[] { "SheetDetailsCalculation_Id" });
            DropIndex("dbo.SheetDetailSheetDetailsCalculations", new[] { "SheetDetail_Id" });
            DropTable("dbo.SheetDetailSheetDetailsCalculations");
        }
    }
}
