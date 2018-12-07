namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCounterpartyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RemovedCounterparties", "OrestCounterpartyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RemovedCounterparties", "OrestCounterpartyName");
        }
    }
}
