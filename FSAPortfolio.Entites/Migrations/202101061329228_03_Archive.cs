namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03_Archive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortfolioConfigurations", "ArchiveAgeDays", c => c.Int(nullable: false, defaultValue: PortfolioSettings.DefaultProjectArchiveAgeDays));
            AddColumn("dbo.PortfolioConfigurations", "ArchivePhase_Id", c => c.Int());
            CreateIndex("dbo.PortfolioConfigurations", "ArchivePhase_Id");
            AddForeignKey("dbo.PortfolioConfigurations", "ArchivePhase_Id", "dbo.ProjectPhases", "Id");

            Sql(@"EXEC('
                WITH penultPhases AS (
                SELECT
                    ph.Configuration_Id,
                    ph.Id as PhaseId,
                    ph.[Order],
                    row_num = ROW_NUMBER() OVER(PARTITION BY ph.Configuration_Id ORDER BY ph.[Order] DESC)
                FROM[dbo].[ProjectPhases] ph)
                UPDATE c
                SET c.ArchivePhase_Id = pph.PhaseId
                FROM[dbo].[PortfolioConfigurations] c
                   JOIN penultPhases pph ON pph.Configuration_Id = c.Portfolio_Id
                WHERE pph.row_num = 2')");
        }

       
        public override void Down()
        {
            DropForeignKey("dbo.PortfolioConfigurations", "ArchivePhase_Id", "dbo.ProjectPhases");
            DropIndex("dbo.PortfolioConfigurations", new[] { "ArchivePhase_Id" });
            DropColumn("dbo.PortfolioConfigurations", "ArchivePhase_Id");
            DropColumn("dbo.PortfolioConfigurations", "ArchiveAgeDays");
        }
    }
}
