namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_MasterLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortfolioLabelConfigs", "IncludedLock", c => c.Boolean(nullable: false));
            AddColumn("dbo.PortfolioLabelConfigs", "AdminOnlyLock", c => c.Boolean(nullable: false));
            AddColumn("dbo.PortfolioLabelConfigs", "MasterLabel_Id", c => c.Int());
            CreateIndex("dbo.PortfolioLabelConfigs", "MasterLabel_Id");
            AddForeignKey("dbo.PortfolioLabelConfigs", "MasterLabel_Id", "dbo.PortfolioLabelConfigs", "Id");
            DropColumn("dbo.PortfolioLabelConfigs", "ReadOnly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PortfolioLabelConfigs", "ReadOnly", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PortfolioLabelConfigs", "MasterLabel_Id", "dbo.PortfolioLabelConfigs");
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "MasterLabel_Id" });
            DropColumn("dbo.PortfolioLabelConfigs", "MasterLabel_Id");
            DropColumn("dbo.PortfolioLabelConfigs", "AdminOnlyLock");
            DropColumn("dbo.PortfolioLabelConfigs", "IncludedLock");
        }
    }
}
