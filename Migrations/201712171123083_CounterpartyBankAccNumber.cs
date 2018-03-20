namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CounterpartyBankAccNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Counterparties", "BankAccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Counterparties", "BankAccountNumber", c => c.String());
        }
    }
}
