namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_RemovedCounterpartyComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RemovedCounterparties", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RemovedCounterparties", "Comment");
        }
    }
}
