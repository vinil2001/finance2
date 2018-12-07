namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartialPayment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartialPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountToPayment = c.Int(nullable: false),
                        Comment = c.String(),
                        PaymentStatementId = c.Int(nullable: false),
                        whoAddThis = c.String(),
                        whoMadeLastChanges = c.String(),
                        WhenEdited = c.DateTime(nullable: false),
                        InvoiceChecked = c.Boolean(nullable: false),
                        PaymentApproved = c.Boolean(nullable: false),
                        PaymentDone = c.Boolean(nullable: false),
                        InvoiceCheckedUser_Id = c.String(maxLength: 128),
                        PaymentApprovedUser_Id = c.String(maxLength: 128),
                        PaymentDoneUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InvoiceCheckedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentApprovedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentDoneUser_Id)
                .ForeignKey("dbo.PaymentStatements", t => t.PaymentStatementId, cascadeDelete: true)
                .Index(t => t.PaymentStatementId)
                .Index(t => t.InvoiceCheckedUser_Id)
                .Index(t => t.PaymentApprovedUser_Id)
                .Index(t => t.PaymentDoneUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartialPayments", "PaymentStatementId", "dbo.PaymentStatements");
            DropForeignKey("dbo.PartialPayments", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartialPayments", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartialPayments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PartialPayments", new[] { "PaymentDoneUser_Id" });
            DropIndex("dbo.PartialPayments", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.PartialPayments", new[] { "InvoiceCheckedUser_Id" });
            DropIndex("dbo.PartialPayments", new[] { "PaymentStatementId" });
            DropTable("dbo.PartialPayments");
        }
    }
}
