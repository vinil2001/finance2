namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MySQLDbId = c.Int(nullable: false),
                        Name = c.String(),
                        AccountNumber = c.String(),
                        MFO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Payments", "Bank_Id", c => c.Int());
            CreateIndex("dbo.Payments", "Bank_Id");
            AddForeignKey("dbo.Payments", "Bank_Id", "dbo.Banks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "Bank_Id", "dbo.Banks");
            DropIndex("dbo.Payments", new[] { "Bank_Id" });
            DropColumn("dbo.Payments", "Bank_Id");
            DropTable("dbo.Banks");
        }
    }
}
