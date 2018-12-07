namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DublicatePayDone : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentStatements", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentStatements", new[] { "PaymentDoneUser_Id" });
            AddColumn("dbo.Payments", "PaymentPaymentDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "PaymentDoneUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Payments", "PaymentDoneUser_Id");
            AddForeignKey("dbo.Payments", "PaymentDoneUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.PaymentStatements", "PaymentDoneUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "PaymentDoneUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Payments", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "PaymentDoneUser_Id" });
            DropColumn("dbo.Payments", "PaymentDoneUser_Id");
            DropColumn("dbo.Payments", "PaymentPaymentDone");
            CreateIndex("dbo.PaymentStatements", "PaymentDoneUser_Id");
            AddForeignKey("dbo.PaymentStatements", "PaymentDoneUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
