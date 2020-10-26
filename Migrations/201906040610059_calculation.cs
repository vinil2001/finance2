namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class calculation : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.SheetDetails", "SheetDetailsCalculationId", "dbo.SheetDetailsCalculations");
            //DropIndex("dbo.SheetDetails", new[] { "SheetDetailsCalculationId" });
            //DropTable("dbo.SheetDetailsCalculations");
            //DropTable("dbo.SheetDetails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SheetDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailNumber = c.Int(nullable: false),
                        DetailName = c.String(),
                        DetailLength = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetailWidth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetailAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetailsSquare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaterialPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetailPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SheetDetailsCalculationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SheetDetailsCalculations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        MaterialPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CuttingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Area = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SheetThickness = c.Int(nullable: false),
                        WasteCoeficcient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculationDate = c.DateTime(nullable: false),
                        PriceFluctuationCoefficient = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SheetDetails", "SheetDetailsCalculationId");
            AddForeignKey("dbo.SheetDetails", "SheetDetailsCalculationId", "dbo.SheetDetailsCalculations", "Id");
        }
    }
}
