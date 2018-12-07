namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrency : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentStatements", "CurrencyId", c => c.Int());
            CreateIndex("dbo.PaymentStatements", "CurrencyId");
            AddForeignKey("dbo.PaymentStatements", "CurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentStatements", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.PaymentStatements", new[] { "CurrencyId" });
            DropColumn("dbo.PaymentStatements", "CurrencyId");
            DropTable("dbo.Currencies");
        }
    }
}
