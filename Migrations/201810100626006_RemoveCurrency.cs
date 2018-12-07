namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCurrency : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentStatements", "Currency_Id", "dbo.Currencies");
            DropIndex("dbo.PaymentStatements", new[] { "Currency_Id" });
            DropColumn("dbo.PaymentStatements", "Currency_Id");
            DropTable("dbo.Currencies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentStatements", "Currency_Id", c => c.Int());
            CreateIndex("dbo.PaymentStatements", "Currency_Id");
            AddForeignKey("dbo.PaymentStatements", "Currency_Id", "dbo.Currencies", "Id");
        }
    }
}
