namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdORest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counterparties", "IdOrest", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counterparties", "IdOrest");
        }
    }
}
