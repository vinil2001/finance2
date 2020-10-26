namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OutgoingPayments : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.OutgoingPayments");
        }
    }
}
