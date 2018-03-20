namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToLongIdOrest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counterparties", "IdOrest", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counterparties", "IdOrest", c => c.Int());
        }
    }
}
