namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Incomings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IncomingData = c.DateTime(nullable: false),
                        BankId = c.Int(nullable: false),
                        CounterpartyId = c.Int(nullable: false),
                        IncomingSum = c.Double(nullable: false),
                        IncomingCategoryId = c.Int(nullable: false),
                        IncomingTypeId = c.Int(nullable: false),
                        InvoiceNumber = c.Int(),
                        InvoiceData = c.DateTime(),
                        WayOfPaymentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .ForeignKey("dbo.Counterparties", t => t.CounterpartyId, cascadeDelete: true)
                .ForeignKey("dbo.IncomingCategories", t => t.IncomingCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.IncomingTypes", t => t.IncomingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.WayOfPayments", t => t.WayOfPaymentId, cascadeDelete: true)
                .Index(t => t.BankId)
                .Index(t => t.CounterpartyId)
                .Index(t => t.IncomingCategoryId)
                .Index(t => t.IncomingTypeId)
                .Index(t => t.WayOfPaymentId);
            
            CreateTable(
                "dbo.Counterparties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OwneshipTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OwnershipTypes", t => t.OwneshipTypeId, cascadeDelete: true)
                .Index(t => t.OwneshipTypeId);
            
            CreateTable(
                "dbo.OwnershipTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnershipTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomingCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncomingCategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomingTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WayOfPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WayOfPaymentName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Outgoings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OutgoingData = c.DateTime(),
                        OutgoingSum = c.Double(),
                        InvoiceNumber = c.Int(),
                        InvoiceData = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutgoingsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutgoingsTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OutgoingsCategories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OutgoingsTypes", "Category_Id", "dbo.OutgoingsCategories");
            DropForeignKey("dbo.Incomings", "WayOfPaymentId", "dbo.WayOfPayments");
            DropForeignKey("dbo.Incomings", "IncomingTypeId", "dbo.IncomingTypes");
            DropForeignKey("dbo.Incomings", "IncomingCategoryId", "dbo.IncomingCategories");
            DropForeignKey("dbo.Incomings", "CounterpartyId", "dbo.Counterparties");
            DropForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes");
            DropForeignKey("dbo.Incomings", "BankId", "dbo.Banks");
            DropIndex("dbo.OutgoingsTypes", new[] { "Category_Id" });
            DropIndex("dbo.Counterparties", new[] { "OwneshipTypeId" });
            DropIndex("dbo.Incomings", new[] { "WayOfPaymentId" });
            DropIndex("dbo.Incomings", new[] { "IncomingTypeId" });
            DropIndex("dbo.Incomings", new[] { "IncomingCategoryId" });
            DropIndex("dbo.Incomings", new[] { "CounterpartyId" });
            DropIndex("dbo.Incomings", new[] { "BankId" });
            DropTable("dbo.OutgoingsTypes");
            DropTable("dbo.OutgoingsCategories");
            DropTable("dbo.Outgoings");
            DropTable("dbo.WayOfPayments");
            DropTable("dbo.IncomingTypes");
            DropTable("dbo.IncomingCategories");
            DropTable("dbo.OwnershipTypes");
            DropTable("dbo.Counterparties");
            DropTable("dbo.Incomings");
            DropTable("dbo.Banks");
        }
    }
}
