namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BigUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incomings", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes");
            DropForeignKey("dbo.PaymentStatements", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.Counterparties", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels");
            DropIndex("dbo.Incomings", new[] { "Counterparty_Id" });
            DropIndex("dbo.Counterparties", new[] { "OwneshipTypeId" });
            DropIndex("dbo.Counterparties", new[] { "CounterpartySearchUnionModel_id" });
            DropIndex("dbo.PaymentStatements", new[] { "Counterparty_Id" });
            AddColumn("dbo.Incomings", "KltId", c => c.Long(nullable: false));       
           
            DropColumn("dbo.Incomings", "CounterpartyId");         
            DropTable("dbo.Counterparties");
            DropTable("dbo.OwnershipTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OwnershipTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnershipTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Counterparties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdOrest = c.Long(),
                        Name = c.String(nullable: false),
                        FullName = c.String(),
                        VATPayer = c.Boolean(nullable: false),
                        CodVATPayer = c.String(),
                        VATCertificateNumber = c.String(),
                        BankName = c.String(),
                        AccountNumber = c.String(),
                        BankMFO = c.String(),
                        EDRPO = c.String(),
                        LegalAdress = c.String(),
                        ActualAddress = c.String(),
                        PhoneNumber = c.String(),
                        ContactPerson = c.String(),
                        Comment = c.String(),
                        Discount = c.Int(),
                        OwneshipTypeId = c.Int(),
                        CounterpartySearchUnionModel_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentStatements", "Counterparty_Id", c => c.Int());
            AddColumn("dbo.PaymentStatements", "Klt_id", c => c.Int(nullable: false));
            AddColumn("dbo.Incomings", "Counterparty_Id", c => c.Int());
            AddColumn("dbo.Incomings", "Klt_Id", c => c.Int(nullable: false));
            DropColumn("dbo.PaymentStatements", "KltId");
            DropColumn("dbo.Incomings", "KltId");
            CreateIndex("dbo.PaymentStatements", "Counterparty_Id");
            CreateIndex("dbo.Counterparties", "CounterpartySearchUnionModel_id");
            CreateIndex("dbo.Counterparties", "OwneshipTypeId");
            CreateIndex("dbo.Incomings", "Counterparty_Id");
            AddForeignKey("dbo.Counterparties", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels", "id");
            AddForeignKey("dbo.PaymentStatements", "Counterparty_Id", "dbo.Counterparties", "Id");
            AddForeignKey("dbo.Counterparties", "OwneshipTypeId", "dbo.OwnershipTypes", "Id");
            AddForeignKey("dbo.Incomings", "Counterparty_Id", "dbo.Counterparties", "Id");
        }
    }
}
