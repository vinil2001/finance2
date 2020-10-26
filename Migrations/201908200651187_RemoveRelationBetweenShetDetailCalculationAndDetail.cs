namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRelationBetweenShetDetailCalculationAndDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SheetDetails", "SheetDetailsCalculationId", "dbo.SheetDetailsCalculations");
            DropIndex("dbo.SheetDetails", new[] { "SheetDetailsCalculationId" });
            DropColumn("dbo.SheetDetails", "SheetDetailsCalculationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SheetDetails", "SheetDetailsCalculationId", c => c.Int());
            CreateIndex("dbo.SheetDetails", "SheetDetailsCalculationId");
            AddForeignKey("dbo.SheetDetails", "SheetDetailsCalculationId", "dbo.SheetDetailsCalculations", "Id");
        }
    }
}
