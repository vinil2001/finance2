namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrestCounterpartyIdToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RemovedCounterparties", "OrestCounterpartyId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RemovedCounterparties", "OrestCounterpartyId", c => c.Int(nullable: false));
        }
    }
}
