namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationUserManaget : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentStatements", "InvoiceCheckedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "PaymentApprovedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentStatements", "PaymentDoneUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentStatements", new[] { "InvoiceCheckedUser_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "PaymentApprovedUser_Id" });
            DropIndex("dbo.PaymentStatements", new[] { "PaymentDoneUser_Id" });
            AddColumn("dbo.PaymentStatements", "InvoiceCheckedUser_UserLockoutEnabledByDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentStatements", "InvoiceCheckedUser_MaxFailedAccessAttemptsBeforeLockout", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentStatements", "InvoiceCheckedUser_DefaultAccountLockoutTimeSpan", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.PaymentStatements", "PaymentApprovedUser_UserLockoutEnabledByDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentStatements", "PaymentApprovedUser_MaxFailedAccessAttemptsBeforeLockout", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentStatements", "PaymentApprovedUser_DefaultAccountLockoutTimeSpan", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.PaymentStatements", "PaymentDoneUser_UserLockoutEnabledByDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentStatements", "PaymentDoneUser_MaxFailedAccessAttemptsBeforeLockout", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentStatements", "PaymentDoneUser_DefaultAccountLockoutTimeSpan", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.PaymentStatements", "InvoiceCheckedUser_Id");
            DropColumn("dbo.PaymentStatements", "PaymentApprovedUser_Id");
            DropColumn("dbo.PaymentStatements", "PaymentDoneUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentStatements", "PaymentDoneUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "PaymentApprovedUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentStatements", "InvoiceCheckedUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.PaymentStatements", "PaymentDoneUser_DefaultAccountLockoutTimeSpan");
            DropColumn("dbo.PaymentStatements", "PaymentDoneUser_MaxFailedAccessAttemptsBeforeLockout");
            DropColumn("dbo.PaymentStatements", "PaymentDoneUser_UserLockoutEnabledByDefault");
            DropColumn("dbo.PaymentStatements", "PaymentApprovedUser_DefaultAccountLockoutTimeSpan");
            DropColumn("dbo.PaymentStatements", "PaymentApprovedUser_MaxFailedAccessAttemptsBeforeLockout");
            DropColumn("dbo.PaymentStatements", "PaymentApprovedUser_UserLockoutEnabledByDefault");
            DropColumn("dbo.PaymentStatements", "InvoiceCheckedUser_DefaultAccountLockoutTimeSpan");
            DropColumn("dbo.PaymentStatements", "InvoiceCheckedUser_MaxFailedAccessAttemptsBeforeLockout");
            DropColumn("dbo.PaymentStatements", "InvoiceCheckedUser_UserLockoutEnabledByDefault");
            CreateIndex("dbo.PaymentStatements", "PaymentDoneUser_Id");
            CreateIndex("dbo.PaymentStatements", "PaymentApprovedUser_Id");
            CreateIndex("dbo.PaymentStatements", "InvoiceCheckedUser_Id");
            AddForeignKey("dbo.PaymentStatements", "PaymentDoneUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "PaymentApprovedUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PaymentStatements", "InvoiceCheckedUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
