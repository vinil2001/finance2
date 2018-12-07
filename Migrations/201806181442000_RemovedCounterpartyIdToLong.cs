namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCounterpartyIdToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RemovedCounterparties", "removedCounterpartyId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RemovedCounterparties", "removedCounterpartyId", c => c.Int(nullable: false));
        }
    }
}
