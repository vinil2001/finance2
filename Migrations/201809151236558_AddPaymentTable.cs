namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        whoAddThis = c.String(),
                        whoMadeLastChanges = c.String(),
                        WhenEdited = c.DateTime(nullable: false),
                        InvoiceChecked = c.Boolean(nullable: false),
                        PaymentApproved = c.Boolean(nullable: false),
                        PaymentDone = c.Boolean(nullable: false),
                        InvoiceCheckedUser_Id = c.String(maxLength: 128),
                        PaymentApprovedUser_Id = c.String(maxLength: 128),
                        PaymentDoneUser_Id = c.String(maxLength: 128),
                        PaymentStatement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InvoiceCheckedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentApprovedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PaymentDoneUser_Id)
                .ForeignKey("dbo.PaymentStatements", t => t.PaymentStatement_Id)
                .Index(t => t.InvoiceCheckedUser_Id)
                .Index(t => t.PaymentApprovedUser_Id)
                .Index(t => t.PaymentDoneUser_Id)
                .Index(t => t.PaymentStatement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "PaymentStatement_Id", "dbo.PaymentStatements");
            DropForeignKey("dbo.Payments", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "PaymentStatement_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentDoneUser_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.Payments", new[] { "InvoiceCheckedUser_Id" });
            DropTable("dbo.Payments");
        }
    }
}
