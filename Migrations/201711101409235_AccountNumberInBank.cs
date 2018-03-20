namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountNumberInBank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        AccountNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccNumbers", "BankId", "dbo.Banks");
            DropIndex("dbo.AccNumbers", new[] { "BankId" });
            DropTable("dbo.AccNumbers");
        }
    }
}
