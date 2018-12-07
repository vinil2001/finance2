namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatementsChanging2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhenEdited", c => c.DateTime());
            AddColumn("dbo.PaymentStatements", "whoAddThis_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.PaymentStatements", "whoAddThis_Id");
            CreateIndex("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            AddForeignKey("dbo.PaymentStatements", "whoAddThis_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentStatements", "whoMadeLastChanges_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "whoAddThis_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentStatements", new[] { "whoMadeLastChanges_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "whoAddThis_Id" });
            DropColumn("dbo.PaymentStatements", "whoMadeLastChanges_Id");
            DropColumn("dbo.PaymentStatements", "whoAddThis_Id");
            DropColumn("dbo.PaymentStatements", "PaymentWhenEdited");
        }
    }
}
