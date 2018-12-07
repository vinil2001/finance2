namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePaymentFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payments", name: "PaymentDoneUser_Id", newName: "PaymentPaymentDoneUser_Id");
            RenameColumn(table: "dbo.Payments", name: "whoAddThis_Id", newName: "PaymentWhoAddThis_Id");
            RenameColumn(table: "dbo.Payments", name: "whoMadeLastChanges_Id", newName: "PaymentwhoMadeLastChanges_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentDoneUser_Id", newName: "IX_PaymentPaymentDoneUser_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_whoAddThis_Id", newName: "IX_PaymentWhoAddThis_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_whoMadeLastChanges_Id", newName: "IX_PaymentwhoMadeLastChanges_Id");
            AddColumn("dbo.Payments", "PaymentComment", c => c.String());
            AddColumn("dbo.Payments", "PaymentWhenEdited", c => c.DateTime());
            AddColumn("dbo.Payments", "PaymentPaymentDone", c => c.Boolean(nullable: false));
            DropColumn("dbo.Payments", "Comment");
            DropColumn("dbo.Payments", "WhenEdited");
            DropColumn("dbo.Payments", "PaymentDone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "PaymentDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "WhenEdited", c => c.DateTime());
            AddColumn("dbo.Payments", "Comment", c => c.String());
            DropColumn("dbo.Payments", "PaymentPaymentDone");
            DropColumn("dbo.Payments", "PaymentWhenEdited");
            DropColumn("dbo.Payments", "PaymentComment");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentwhoMadeLastChanges_Id", newName: "IX_whoMadeLastChanges_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentWhoAddThis_Id", newName: "IX_whoAddThis_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentPaymentDoneUser_Id", newName: "IX_PaymentDoneUser_Id");
            RenameColumn(table: "dbo.Payments", name: "PaymentwhoMadeLastChanges_Id", newName: "whoMadeLastChanges_Id");
            RenameColumn(table: "dbo.Payments", name: "PaymentWhoAddThis_Id", newName: "whoAddThis_Id");
            RenameColumn(table: "dbo.Payments", name: "PaymentPaymentDoneUser_Id", newName: "PaymentDoneUser_Id");
        }
    }
}
