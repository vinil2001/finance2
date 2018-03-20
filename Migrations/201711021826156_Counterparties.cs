namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Counterparties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banks", "BankMFO", c => c.String());
            AddColumn("dbo.Counterparties", "FullName", c => c.String());
            AddColumn("dbo.Counterparties", "VATPayer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Counterparties", "CodVATPayer", c => c.String());
            AddColumn("dbo.Counterparties", "VATCertificateNumber", c => c.String());
            AddColumn("dbo.Counterparties", "BankAccountNumber", c => c.String());
            AddColumn("dbo.Counterparties", "EDRPO", c => c.String());
            AddColumn("dbo.Counterparties", "LegalAdress", c => c.String());
            AddColumn("dbo.Counterparties", "ActualAddress", c => c.String());
            AddColumn("dbo.Counterparties", "PhoneNumber", c => c.String());
            AddColumn("dbo.Counterparties", "ContactPerson", c => c.String());
            AddColumn("dbo.Counterparties", "Comment", c => c.String());
            AddColumn("dbo.Counterparties", "Discount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counterparties", "Discount");
            DropColumn("dbo.Counterparties", "Comment");
            DropColumn("dbo.Counterparties", "ContactPerson");
            DropColumn("dbo.Counterparties", "PhoneNumber");
            DropColumn("dbo.Counterparties", "ActualAddress");
            DropColumn("dbo.Counterparties", "LegalAdress");
            DropColumn("dbo.Counterparties", "EDRPO");
            DropColumn("dbo.Counterparties", "BankAccountNumber");
            DropColumn("dbo.Counterparties", "VATCertificateNumber");
            DropColumn("dbo.Counterparties", "CodVATPayer");
            DropColumn("dbo.Counterparties", "VATPayer");
            DropColumn("dbo.Counterparties", "FullName");
            DropColumn("dbo.Banks", "BankMFO");
        }
    }
}
