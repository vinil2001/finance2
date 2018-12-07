namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class whoAddThisAndWhoMadeLastChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentStatements", "PaymentWhoAddThis", c => c.String());
            AddColumn("dbo.PaymentStatements", "PaymentwhoMadeLastChanges", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentStatements", "PaymentwhoMadeLastChanges");
            DropColumn("dbo.PaymentStatements", "PaymentWhoAddThis");
        }
    }
}
