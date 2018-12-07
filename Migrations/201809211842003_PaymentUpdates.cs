namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "InvoiceCheckedUser_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentDoneUser_Id" });
            DropColumn("dbo.Payments", "InvoiceChecked");
            DropColumn("dbo.Payments", "PaymentApproved");
            DropColumn("dbo.Payments", "PaymentPaymentDone");
            DropColumn("dbo.Payments", "InvoiceCheckedUser_Id");
            DropColumn("dbo.Payments", "PaymentApprovedUser_Id");
            DropColumn("dbo.Payments", "PaymentDoneUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "PaymentDoneUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Payments", "PaymentApprovedUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Payments", "InvoiceCheckedUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Payments", "PaymentPaymentDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "PaymentApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "InvoiceChecked", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Payments", "PaymentDoneUser_Id");
            CreateIndex("dbo.Payments", "PaymentApprovedUser_Id");
            CreateIndex("dbo.Payments", "InvoiceCheckedUser_Id");
            AddForeignKey("dbo.Payments", "PaymentDoneUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Payments", "PaymentApprovedUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Payments", "InvoiceCheckedUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
