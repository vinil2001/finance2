namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatement_changeDBCntext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentStatements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KltId = c.Long(nullable: false),
                        InvoiceNumber = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        InvoiceSumma = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        InvoiceChecked = c.Boolean(nullable: false),
                        PaymentApproved = c.Boolean(nullable: false),
                        PaymentDone = c.Boolean(nullable: false),
                        DocumentUrl = c.String(),
                        InvoiceCheckedUser_Id = c.String(maxLength: 128),
                        PaymentApprovedUser_Id = c.String(maxLength: 128),
                        PaymentDoneUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InvoiceCheckedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentApprovedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentDoneUser_Id)
                .Index(t => t.InvoiceCheckedUser_Id)
                .Index(t => t.PaymentApprovedUser_Id)
                .Index(t => t.PaymentDoneUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentStatements", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentStatements", new[] { "PaymentDoneUser_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "InvoiceCheckedUser_Id" });
            DropTable("dbo.PaymentStatements");
        }
    }
}
