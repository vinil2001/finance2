namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomingsAndOutgoingsRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incomings", "IncomingCategories_Id", "dbo.IncomingCategories");
            DropForeignKey("dbo.Incomings", "WayOfPayments_Id", "dbo.WayOfPayments");
            DropIndex("dbo.Incomings", new[] { "IncomingCategories_Id" });
            DropIndex("dbo.Incomings", new[] { "WayOfPayments_Id" });
            DropTable("dbo.Incomings");
            DropTable("dbo.Outgoings");
        }
        
        public override void Down()
        {
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
                "dbo.Incomings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KshId = c.Long(nullable: false),
                        BkhId = c.Long(nullable: false),
                        IncomingCategories_Id = c.Int(),
                        WayOfPayments_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Incomings", "WayOfPayments_Id");
            CreateIndex("dbo.Incomings", "IncomingCategories_Id");
            AddForeignKey("dbo.Incomings", "WayOfPayments_Id", "dbo.WayOfPayments", "Id");
            AddForeignKey("dbo.Incomings", "IncomingCategories_Id", "dbo.IncomingCategories", "Id");
        }
    }
}
