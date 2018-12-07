namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewIncomings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incomings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KshId = c.Long(nullable: false),
                        BkhId = c.Long(nullable: false),
                        IncomingCategories_Id = c.Int(),
                        WayOfPayments_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IncomingCategories", t => t.IncomingCategories_Id)
                .ForeignKey("dbo.WayOfPayments", t => t.WayOfPayments_Id)
                .Index(t => t.IncomingCategories_Id)
                .Index(t => t.WayOfPayments_Id);
            
            AddColumn("dbo.IncomingCategories", "ParentIncomingCategory_Id", c => c.Int());
            AddColumn("dbo.WayOfPayments", "ParentWayOfPayment_Id", c => c.Int());
            CreateIndex("dbo.IncomingCategories", "ParentIncomingCategory_Id");
            CreateIndex("dbo.WayOfPayments", "ParentWayOfPayment_Id");
            AddForeignKey("dbo.IncomingCategories", "ParentIncomingCategory_Id", "dbo.IncomingCategories", "Id");
            AddForeignKey("dbo.WayOfPayments", "ParentWayOfPayment_Id", "dbo.WayOfPayments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incomings", "WayOfPayments_Id", "dbo.WayOfPayments");
            DropForeignKey("dbo.WayOfPayments", "ParentWayOfPayment_Id", "dbo.WayOfPayments");
            DropForeignKey("dbo.Incomings", "IncomingCategories_Id", "dbo.IncomingCategories");
            DropForeignKey("dbo.IncomingCategories", "ParentIncomingCategory_Id", "dbo.IncomingCategories");
            DropIndex("dbo.WayOfPayments", new[] { "ParentWayOfPayment_Id" });
            DropIndex("dbo.Incomings", new[] { "WayOfPayments_Id" });
            DropIndex("dbo.Incomings", new[] { "IncomingCategories_Id" });
            DropIndex("dbo.IncomingCategories", new[] { "ParentIncomingCategory_Id" });
            DropColumn("dbo.WayOfPayments", "ParentWayOfPayment_Id");
            DropColumn("dbo.IncomingCategories", "ParentIncomingCategory_Id");
            DropTable("dbo.Incomings");
        }
    }
}
