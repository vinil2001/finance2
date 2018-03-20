namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Counterparties2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counterparties", "BankName", c => c.String());
            AddColumn("dbo.Counterparties", "AccountNumber", c => c.String());
            AddColumn("dbo.Counterparties", "BankMFO", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counterparties", "BankMFO");
            DropColumn("dbo.Counterparties", "AccountNumber");
            DropColumn("dbo.Counterparties", "BankName");
        }
    }
}
