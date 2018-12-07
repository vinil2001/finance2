namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCounterpary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RemovedCounterparties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        removedCounterpartyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RemovedCounterparties");
        }
    }
}
