namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class calculation2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SheetDetailsCalculations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        MaterialPrice = c.Double(nullable: false),
                        CuttingPrice = c.Double(nullable: false),
                        Area = c.Double(nullable: false),
                        SheetThickness = c.Int(nullable: false),
                        WasteCoeficcient = c.Double(nullable: false),
                        CalculationDate = c.DateTime(nullable: false),
                        PriceFluctuationCoefficient = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SheetDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailNumber = c.Int(nullable: false),
                        DetailName = c.String(),
                        DetailLength = c.Double(nullable: false),
                        DetailWidth = c.Double(nullable: false),
                        DetailAmount = c.Double(nullable: false),
                        DetailsSquare = c.Double(nullable: false),
                        WorkPrice = c.Double(nullable: false),
                        MaterialPrice = c.Double(nullable: false),
                        DetailPrice = c.Double(nullable: false),
                        SheetDetailsCalculationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SheetDetailsCalculations", t => t.SheetDetailsCalculationId)
                .Index(t => t.SheetDetailsCalculationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SheetDetails", "SheetDetailsCalculationId", "dbo.SheetDetailsCalculations");
            DropIndex("dbo.SheetDetails", new[] { "SheetDetailsCalculationId" });
            DropTable("dbo.SheetDetails");
            DropTable("dbo.SheetDetailsCalculations");
        }
    }
}
