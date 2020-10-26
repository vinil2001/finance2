namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBank : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "Bank_Id", "dbo.Banks");
            DropIndex("dbo.Payments", new[] { "Bank_Id" });
            AddColumn("dbo.Payments", "MySQLBankName", c => c.String());
            AddColumn("dbo.Payments", "MySQLBankId", c => c.Long(nullable: false));
            DropColumn("dbo.Payments", "Bank_Id");
            DropTable("dbo.Banks");
        }
        
        public override void Down()
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
            DropColumn("dbo.Payments", "MySQLBankId");
            DropColumn("dbo.Payments", "MySQLBankName");
            CreateIndex("dbo.Payments", "Bank_Id");
            AddForeignKey("dbo.Payments", "Bank_Id", "dbo.Banks", "Id");
        }
    }
}
