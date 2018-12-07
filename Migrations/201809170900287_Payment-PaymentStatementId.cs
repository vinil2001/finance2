namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentPaymentStatementId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "PaymentStatement_Id", "dbo.PaymentStatements");
            DropIndex("dbo.Payments", new[] { "PaymentStatement_Id" });
            RenameColumn(table: "dbo.Payments", name: "PaymentStatement_Id", newName: "PaymentStatementId");
            AlterColumn("dbo.Payments", "PaymentStatementId", c => c.Int(nullable: false));
            CreateIndex("dbo.Payments", "PaymentStatementId");
            AddForeignKey("dbo.Payments", "PaymentStatementId", "dbo.PaymentStatements", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "PaymentStatementId", "dbo.PaymentStatements");
            DropIndex("dbo.Payments", new[] { "PaymentStatementId" });
            AlterColumn("dbo.Payments", "PaymentStatementId", c => c.Int());
            RenameColumn(table: "dbo.Payments", name: "PaymentStatementId", newName: "PaymentStatement_Id");
            CreateIndex("dbo.Payments", "PaymentStatement_Id");
            AddForeignKey("dbo.Payments", "PaymentStatement_Id", "dbo.PaymentStatements", "Id");
        }
    }
}
