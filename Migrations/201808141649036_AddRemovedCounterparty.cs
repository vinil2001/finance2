namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemovedCounterparty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RemovedCounterparties",
                c => new
                    {
                        RemovedCounterpartyId = c.Int(nullable: false, identity: true),
                        OrestCounterpartyId = c.Int(nullable: false),
                        WhoRemoveIt = c.String(),
                        WhenRemoved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RemovedCounterpartyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RemovedCounterparties");
        }
    }
}
