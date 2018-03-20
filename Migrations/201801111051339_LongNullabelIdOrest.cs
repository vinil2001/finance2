namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongNullabelIdOrest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counterparties", "IdOrest", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counterparties", "IdOrest", c => c.Long(nullable: false));
        }
    }
}
