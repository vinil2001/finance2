namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentsDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentUrl = c.String(),
                        PaymentStatement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentStatements", t => t.PaymentStatement_Id)
                .Index(t => t.PaymentStatement_Id);          
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentsDocuments", "PaymentStatement_Id", "dbo.PaymentStatements");
            DropIndex("dbo.PaymentsDocuments", new[] { "PaymentStatement_Id" });
            DropTable("dbo.PaymentsDocuments");
        }
    }
}
