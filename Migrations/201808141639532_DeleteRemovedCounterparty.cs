namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRemovedCounterparty : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RemovedCounterparties");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RemovedCounterparties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        removedCounterpartyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
    }
}
