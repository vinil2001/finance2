namespace Finance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSearchUnionModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.klts", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels");
            DropIndex("dbo.klts", new[] { "CounterpartySearchUnionModel_id" });
            DropTable("dbo.CounterpartySearchUnionModels");
            DropTable("dbo.klts");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.CounterpartySearchUnionModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateIndex("dbo.klts", "CounterpartySearchUnionModel_id");
            AddForeignKey("dbo.klts", "CounterpartySearchUnionModel_id", "dbo.CounterpartySearchUnionModels", "id");
        }
    }
}
