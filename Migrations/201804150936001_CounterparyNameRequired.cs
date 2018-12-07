namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CounterparyNameRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counterparties", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counterparties", "Name", c => c.String());
        }
    }
}
