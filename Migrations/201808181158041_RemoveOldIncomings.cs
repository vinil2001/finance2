namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOldIncomings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccNumbers", "BankId", "dbo.Banks");
            DropForeignKey("dbo.Incomings", "BankId", "dbo.Banks");
            DropForeignKey("dbo.Incomings", "IncomingCategoryId", "dbo.IncomingCategories");
            DropForeignKey("dbo.Incomings", "IncomingTypeId", "dbo.IncomingTypes");
            DropForeignKey("dbo.Incomings", "WayOfPaymentId", "dbo.WayOfPayments");
            DropIndex("dbo.AccNumbers", new[] { "BankId" });
            DropIndex("dbo.Incomings", new[] { "BankId" });
            DropIndex("dbo.Incomings", new[] { "IncomingCategoryId" });
            DropIndex("dbo.Incomings", new[] { "IncomingTypeId" });
            DropIndex("dbo.Incomings", new[] { "WayOfPaymentId" });
            DropTable("dbo.AccNumbers");
            DropTable("dbo.Banks");
            DropTable("dbo.Incomings");
            DropTable("dbo.IncomingTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IncomingTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Incomings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IncomingData = c.DateTime(nullable: false),
                        BankId = c.Int(),
                        KltId = c.Long(nullable: false),
                        IncomingSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomingCategoryId = c.Int(nullable: false),
                        IncomingTypeId = c.Int(nullable: false),
                        InvoiceNumber = c.Int(),
                        InvoiceData = c.DateTime(),
                        WayOfPaymentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        BankMFO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        AccountNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Incomings", "WayOfPaymentId");
            CreateIndex("dbo.Incomings", "IncomingTypeId");
            CreateIndex("dbo.Incomings", "IncomingCategoryId");
            CreateIndex("dbo.Incomings", "BankId");
            CreateIndex("dbo.AccNumbers", "BankId");
            AddForeignKey("dbo.Incomings", "WayOfPaymentId", "dbo.WayOfPayments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Incomings", "IncomingTypeId", "dbo.IncomingTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Incomings", "IncomingCategoryId", "dbo.IncomingCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Incomings", "BankId", "dbo.Banks", "Id");
            AddForeignKey("dbo.AccNumbers", "BankId", "dbo.Banks", "Id", cascadeDelete: true);
        }
    }
}
