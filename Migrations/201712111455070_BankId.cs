namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incomings", "BankId", "dbo.Banks");
            DropIndex("dbo.Incomings", new[] { "BankId" });
            AlterColumn("dbo.Incomings", "BankId", c => c.Int());
            CreateIndex("dbo.Incomings", "BankId");
            AddForeignKey("dbo.Incomings", "BankId", "dbo.Banks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incomings", "BankId", "dbo.Banks");
            DropIndex("dbo.Incomings", new[] { "BankId" });
            AlterColumn("dbo.Incomings", "BankId", c => c.Int(nullable: false));
            CreateIndex("dbo.Incomings", "BankId");
            AddForeignKey("dbo.Incomings", "BankId", "dbo.Banks", "Id", cascadeDelete: true);
        }
    }
}
