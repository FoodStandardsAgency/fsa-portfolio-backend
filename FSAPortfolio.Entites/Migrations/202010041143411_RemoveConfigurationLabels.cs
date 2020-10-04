namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveConfigurationLabels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PortfolioConfigurations", "BCNumberLabel_Id", "dbo.PortfolioLabelConfigs");
            DropForeignKey("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", "dbo.PortfolioLabelConfigs");
            DropIndex("dbo.PortfolioConfigurations", new[] { "BCNumberLabel_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "ProjectIdLabel_Id" });
            DropColumn("dbo.PortfolioConfigurations", "BCNumberLabel_Id");
            DropColumn("dbo.PortfolioConfigurations", "ProjectIdLabel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", c => c.Int());
            AddColumn("dbo.PortfolioConfigurations", "BCNumberLabel_Id", c => c.Int());
            CreateIndex("dbo.PortfolioConfigurations", "ProjectIdLabel_Id");
            CreateIndex("dbo.PortfolioConfigurations", "BCNumberLabel_Id");
            AddForeignKey("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", "dbo.PortfolioLabelConfigs", "Id");
            AddForeignKey("dbo.PortfolioConfigurations", "BCNumberLabel_Id", "dbo.PortfolioLabelConfigs", "Id");
        }
    }
}
