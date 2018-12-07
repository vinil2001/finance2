namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStatementsChanging1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentStatements", "PaymentWhoAddThis");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhoAddThis", c => c.String());
        }
    }
}
