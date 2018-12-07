namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentTableChangeWho : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "whoAddThis_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Payments", "whoMadeLastChanges_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Payments", "whoAddThis_Id");
            CreateIndex("dbo.Payments", "whoMadeLastChanges_Id");
            AddForeignKey("dbo.Payments", "whoAddThis_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Payments", "whoMadeLastChanges_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Payments", "PaymentWhoAddThis");
            DropColumn("dbo.Payments", "PaymentwhoMadeLastChanges");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "PaymentwhoMadeLastChanges", c => c.String());
            AddColumn("dbo.Payments", "PaymentWhoAddThis", c => c.String());
            DropForeignKey("dbo.Payments", "whoMadeLastChanges_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "whoAddThis_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "whoMadeLastChanges_Id" });
            DropIndex("dbo.Payments", new[] { "whoAddThis_Id" });
            DropColumn("dbo.Payments", "whoMadeLastChanges_Id");
            DropColumn("dbo.Payments", "whoAddThis_Id");
        }
    }
}
