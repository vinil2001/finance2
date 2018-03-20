namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CounterpartySearchUnionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CounterpartySearchUnionModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.klts",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        grp = c.Int(),
                        idp = c.Long(),
                        name = c.String(),
                        okpo = c.String(),
                        knds = c.String(),
                        snds = c.String(),
                        chet = c.String(),
                        bank = c.String(),
                        mfob = c.String(),
                        adft = c.String(),
                        adur = c.String(),
                        telf = c.String(),
                        cont = c.String(),
                        comt = c.String(),
                        per = c.Int(),
                        full = c.String(),
                        nds = c.Int(),
                        sld = c.Double(),
                        dsld = c.String(),
                        CounterpartySearchUnionModel_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CounterpartySearchUnionModels", t => t.CounterpartySearchUnionModel_id)
                .Index(t => t.CounterpartySearchUnionModel_id);
            
            AddColumn("dbo.Counterparties", "CounterpartySearchUnionModel_id", c => c.Int());
            CreateIndex("dbo.Counterparties", "CounterpartySearchUnionModel_id");
            AddForeignKey("dbo.Counterparties", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.klts", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels");
            DropForeignKey("dbo.Counterparties", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels");
            DropIndex("dbo.klts", new[] { "CounterpartySearchUnionModel_id" });
            DropIndex("dbo.Counterparties", new[] { "CounterpartySearchUnionModel_id" });
            DropColumn("dbo.Counterparties", "CounterpartySearchUnionModel_id");
            DropTable("dbo.klts");
            DropTable("dbo.CounterpartySearchUnionModels");
        }
    }
}
