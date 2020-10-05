namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PortfolioConfig1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProjectProjects", newName: "DependantProjects");
            RenameTable(name: "dbo.ProjectProject1", newName: "RelatedProjects");
            DropForeignKey("dbo.PortfolioConfigurations", "BCNumberLabel_Id", "dbo.PortfolioLabelConfigs");
            DropForeignKey("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", "dbo.PortfolioLabelConfigs");
            DropIndex("dbo.PortfolioConfigurations", new[] { "BCNumberLabel_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "ProjectIdLabel_Id" });
            RenameColumn(table: "dbo.DependantProjects", name: "Project_Id1", newName: "DependantProject_Id");
            RenameColumn(table: "dbo.RelatedProjects", name: "Project_Id1", newName: "RelatedProject_Id");
            RenameIndex(table: "dbo.DependantProjects", name: "IX_Project_Id1", newName: "IX_DependantProject_Id");
            RenameIndex(table: "dbo.RelatedProjects", name: "IX_Project_Id1", newName: "IX_RelatedProject_Id");
            AddColumn("dbo.PortfolioLabelConfigs", "FieldGroup", c => c.String(maxLength: 50));
            AddColumn("dbo.PortfolioLabelConfigs", "FieldTitle", c => c.String(maxLength: 50));
            AddColumn("dbo.PortfolioLabelConfigs", "FieldOrder", c => c.Int(nullable: false));
            AddColumn("dbo.PortfolioLabelConfigs", "AdminOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.PortfolioLabelConfigs", "ReadOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.PortfolioLabelConfigs", "FieldType", c => c.Int(nullable: false));
            CreateIndex("dbo.PortfolioLabelConfigs", new[] { "Configuration_Id", "FieldTitle" }, unique: true);
            DropColumn("dbo.PortfolioConfigurations", "BCNumberLabel_Id");
            DropColumn("dbo.PortfolioConfigurations", "ProjectIdLabel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", c => c.Int());
            AddColumn("dbo.PortfolioConfigurations", "BCNumberLabel_Id", c => c.Int());
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Configuration_Id", "FieldTitle" });
            DropColumn("dbo.PortfolioLabelConfigs", "FieldType");
            DropColumn("dbo.PortfolioLabelConfigs", "ReadOnly");
            DropColumn("dbo.PortfolioLabelConfigs", "AdminOnly");
            DropColumn("dbo.PortfolioLabelConfigs", "FieldOrder");
            DropColumn("dbo.PortfolioLabelConfigs", "FieldTitle");
            DropColumn("dbo.PortfolioLabelConfigs", "FieldGroup");
            RenameIndex(table: "dbo.RelatedProjects", name: "IX_RelatedProject_Id", newName: "IX_Project_Id1");
            RenameIndex(table: "dbo.DependantProjects", name: "IX_DependantProject_Id", newName: "IX_Project_Id1");
            RenameColumn(table: "dbo.RelatedProjects", name: "RelatedProject_Id", newName: "Project_Id1");
            RenameColumn(table: "dbo.DependantProjects", name: "DependantProject_Id", newName: "Project_Id1");
            CreateIndex("dbo.PortfolioConfigurations", "ProjectIdLabel_Id");
            CreateIndex("dbo.PortfolioConfigurations", "BCNumberLabel_Id");
            AddForeignKey("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", "dbo.PortfolioLabelConfigs", "Id");
            AddForeignKey("dbo.PortfolioConfigurations", "BCNumberLabel_Id", "dbo.PortfolioLabelConfigs", "Id");
            RenameTable(name: "dbo.RelatedProjects", newName: "ProjectProject1");
            RenameTable(name: "dbo.DependantProjects", newName: "ProjectProjects");
        }
    }
}
