namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOutgoingPayments : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.OutgoingPayments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OutgoingPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrestId = c.Int(nullable: false),
                        OrestDocumentNumber = c.String(),
                        OrestOurCompanyId = c.Int(nullable: false),
                        OrestClientId = c.Int(nullable: false),
                        OrestDocumentDate = c.String(),
                        OrestPaymentSum = c.Double(nullable: false),
                        OrestBankId = c.Int(nullable: false),
                        OrestSupplierInvoiceId = c.Int(nullable: false),
                        OrestPaymentComment = c.Int(nullable: false),
                        OrestDocumentStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
