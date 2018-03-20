namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckPayment : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.PaymentStatements", "KltId", "dbo.klts");
            //DropIndex("dbo.PaymentStatements", new[] { "KltId" });
        }
        
        public override void Down()
        {
            //CreateIndex("dbo.PaymentStatements", "KltId");
            //AddForeignKey("dbo.PaymentStatements", "KltId", "dbo.klts", "id", cascadeDelete: true);
        }
    }
}
