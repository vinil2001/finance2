namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullabelToOwneshipTypeId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes");
            DropIndex("dbo.Counterparties", new[] { "OwneshipTypeId" });
            AlterColumn("dbo.Counterparties", "OwneshipTypeId", c => c.Int());
            CreateIndex("dbo.Counterparties", "OwneshipTypeId");
            AddForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes");
            DropIndex("dbo.Counterparties", new[] { "OwneshipTypeId" });
            AlterColumn("dbo.Counterparties", "OwneshipTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Counterparties", "OwneshipTypeId");
            AddForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes", "Id", cascadeDelete: true);
        }
    }
}
