namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Back : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartialPayments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartialPayments", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartialPayments", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartialPayments", "PaymentStatementId", "dbo.PaymentStatements");
            DropIndex("dbo.PartialPayments", new[] { "PaymentStatementId" });
            DropIndex("dbo.PartialPayments", new[] { "InvoiceCheckedUser_Id" });
            DropIndex("dbo.PartialPayments", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.PartialPayments", new[] { "PaymentDoneUser_Id" });
            DropTable("dbo.PartialPayments");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PartialPayments", "PaymentDoneUser_Id");
            CreateIndex("dbo.PartialPayments", "PaymentApprovedUser_Id");
            CreateIndex("dbo.PartialPayments", "InvoiceCheckedUser_Id");
            CreateIndex("dbo.PartialPayments", "PaymentStatementId");
            AddForeignKey("dbo.PartialPayments", "PaymentStatementId", "dbo.PaymentStatements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PartialPayments", "PaymentDoneUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PartialPayments", "PaymentApprovedUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PartialPayments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
