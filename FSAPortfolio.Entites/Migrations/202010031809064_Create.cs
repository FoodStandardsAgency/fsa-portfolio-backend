namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BudgetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.PortfolioConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BCNumberLabel_Id = c.Int(),
                        CompletedPhase_Id = c.Int(),
                        ProjectIdLabel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioLabelConfigs", t => t.BCNumberLabel_Id)
                .ForeignKey("dbo.ProjectPhases", t => t.CompletedPhase_Id)
                .ForeignKey("dbo.Portfolios", t => t.Id)
                .ForeignKey("dbo.PortfolioLabelConfigs", t => t.ProjectIdLabel_Id)
                .Index(t => t.Id)
                .Index(t => t.BCNumberLabel_Id)
                .Index(t => t.CompletedPhase_Id)
                .Index(t => t.ProjectIdLabel_Id);
            
            CreateTable(
                "dbo.PortfolioLabelConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Included = c.Boolean(nullable: false),
                        FieldName = c.String(maxLength: 50),
                        Label = c.String(maxLength: 50),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.FieldName }, unique: true);
            
            CreateTable(
                "dbo.ProjectCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.ProjectPhases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 50),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.ProjectOnHoldStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 20),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 10),
                        ShortName = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.ShortName, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.String(maxLength: 10),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 1000),
                        Directorate = c.String(maxLength: 150),
                        OwningPortfolio_Id = c.Int(nullable: false),
                        ProjectCategory_Id = c.Int(),
                        ProjectSize_Id = c.Int(),
                        BudgetType_Id = c.Int(),
                        Funded = c.Int(nullable: false),
                        Confidence = c.Int(nullable: false),
                        Priorities = c.Int(nullable: false),
                        Benefits = c.Int(nullable: false),
                        Criticality = c.Int(nullable: false),
                        Lead_Id = c.Int(),
                        ServiceLead_Id = c.Int(),
                        Team = c.String(maxLength: 500),
                        Priority = c.Int(),
                        StartDate = c.DateTime(),
                        ActualStartDate = c.DateTime(),
                        ExpectedEndDate = c.DateTime(),
                        HardEndDate = c.DateTime(),
                        LatestUpdate_Id = c.Int(),
                        FirstUpdate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetTypes", t => t.BudgetType_Id)
                .ForeignKey("dbo.ProjectCategories", t => t.ProjectCategory_Id)
                .ForeignKey("dbo.ProjectUpdateItems", t => t.FirstUpdate_Id)
                .ForeignKey("dbo.ProjectUpdateItems", t => t.LatestUpdate_Id)
                .ForeignKey("dbo.People", t => t.Lead_Id)
                .ForeignKey("dbo.Portfolios", t => t.OwningPortfolio_Id)
                .ForeignKey("dbo.People", t => t.ServiceLead_Id)
                .ForeignKey("dbo.ProjectSizes", t => t.ProjectSize_Id)
                .Index(t => t.ProjectId, unique: true)
                .Index(t => t.OwningPortfolio_Id)
                .Index(t => t.ProjectCategory_Id)
                .Index(t => t.ProjectSize_Id)
                .Index(t => t.BudgetType_Id)
                .Index(t => t.Lead_Id)
                .Index(t => t.ServiceLead_Id)
                .Index(t => t.LatestUpdate_Id)
                .Index(t => t.FirstUpdate_Id);
            
            CreateTable(
                "dbo.ProjectAuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.ProjectUpdateItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                        PercentageComplete = c.Single(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Spent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedCurrentPhaseEnd = c.DateTime(),
                        SyncId = c.Int(nullable: false),
                        OnHoldStatus_Id = c.Int(),
                        Person_Id = c.Int(),
                        Phase_Id = c.Int(),
                        RAGStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectOnHoldStatus", t => t.OnHoldStatus_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.ProjectPhases", t => t.Phase_Id)
                .ForeignKey("dbo.ProjectRAGStatus", t => t.RAGStatus_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.OnHoldStatus_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Phase_Id)
                .Index(t => t.RAGStatus_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Surname = c.String(maxLength: 250),
                        Firstname = c.String(maxLength: 250),
                        Email = c.String(maxLength: 250),
                        G6team = c.String(maxLength: 50),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectRAGStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 20),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.ProjectSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                        Configuration_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.ViewKey }, unique: true)
                .Index(t => new { t.Configuration_Id, t.Name }, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        UserName = c.String(maxLength: 50),
                        PasswordHash = c.String(maxLength: 300),
                        AccessGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessGroups", t => t.AccessGroupId)
                .Index(t => t.AccessGroupId);
            
            CreateTable(
                "dbo.ProjectProjects",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Project_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Project_Id1 })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id1)
                .Index(t => t.Project_Id)
                .Index(t => t.Project_Id1);
            
            CreateTable(
                "dbo.ProjectProject1",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Project_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Project_Id1 })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id1)
                .Index(t => t.Project_Id)
                .Index(t => t.Project_Id1);
            
            CreateTable(
                "dbo.PortfolioProjects",
                c => new
                    {
                        Portfolio_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Portfolio_Id, t.Project_Id })
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Portfolio_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.ProjectRAGStatus", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.ProjectSizes", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioConfigurations", "ProjectIdLabel_Id", "dbo.PortfolioLabelConfigs");
            DropForeignKey("dbo.PortfolioProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.PortfolioProjects", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.ProjectUpdateItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectSize_Id", "dbo.ProjectSizes");
            DropForeignKey("dbo.Projects", "ServiceLead_Id", "dbo.People");
            DropForeignKey("dbo.ProjectProject1", "Project_Id1", "dbo.Projects");
            DropForeignKey("dbo.ProjectProject1", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "OwningPortfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.Projects", "Lead_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "LatestUpdate_Id", "dbo.ProjectUpdateItems");
            DropForeignKey("dbo.Projects", "FirstUpdate_Id", "dbo.ProjectUpdateItems");
            DropForeignKey("dbo.ProjectUpdateItems", "RAGStatus_Id", "dbo.ProjectRAGStatus");
            DropForeignKey("dbo.ProjectUpdateItems", "Phase_Id", "dbo.ProjectPhases");
            DropForeignKey("dbo.ProjectUpdateItems", "Person_Id", "dbo.People");
            DropForeignKey("dbo.ProjectUpdateItems", "OnHoldStatus_Id", "dbo.ProjectOnHoldStatus");
            DropForeignKey("dbo.ProjectProjects", "Project_Id1", "dbo.Projects");
            DropForeignKey("dbo.ProjectProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectCategory_Id", "dbo.ProjectCategories");
            DropForeignKey("dbo.Projects", "BudgetType_Id", "dbo.BudgetTypes");
            DropForeignKey("dbo.ProjectAuditLogs", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.PortfolioConfigurations", "Id", "dbo.Portfolios");
            DropForeignKey("dbo.ProjectPhases", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.ProjectOnHoldStatus", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioLabelConfigs", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioConfigurations", "CompletedPhase_Id", "dbo.ProjectPhases");
            DropForeignKey("dbo.ProjectCategories", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.BudgetTypes", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioConfigurations", "BCNumberLabel_Id", "dbo.PortfolioLabelConfigs");
            DropIndex("dbo.PortfolioProjects", new[] { "Project_Id" });
            DropIndex("dbo.PortfolioProjects", new[] { "Portfolio_Id" });
            DropIndex("dbo.ProjectProject1", new[] { "Project_Id1" });
            DropIndex("dbo.ProjectProject1", new[] { "Project_Id" });
            DropIndex("dbo.ProjectProjects", new[] { "Project_Id1" });
            DropIndex("dbo.ProjectProjects", new[] { "Project_Id" });
            DropIndex("dbo.Users", new[] { "AccessGroupId" });
            DropIndex("dbo.ProjectSizes", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectSizes", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectRAGStatus", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectRAGStatus", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "RAGStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Phase_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Person_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "OnHoldStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Project_Id" });
            DropIndex("dbo.ProjectAuditLogs", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "FirstUpdate_Id" });
            DropIndex("dbo.Projects", new[] { "LatestUpdate_Id" });
            DropIndex("dbo.Projects", new[] { "ServiceLead_Id" });
            DropIndex("dbo.Projects", new[] { "Lead_Id" });
            DropIndex("dbo.Projects", new[] { "BudgetType_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectSize_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectCategory_Id" });
            DropIndex("dbo.Projects", new[] { "OwningPortfolio_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectId" });
            DropIndex("dbo.Portfolios", new[] { "Name" });
            DropIndex("dbo.Portfolios", new[] { "ShortName" });
            DropIndex("dbo.Portfolios", new[] { "ViewKey" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectPhases", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectPhases", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectCategories", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectCategories", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Configuration_Id", "FieldName" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "ProjectIdLabel_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "CompletedPhase_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "BCNumberLabel_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "Id" });
            DropIndex("dbo.BudgetTypes", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.BudgetTypes", new[] { "Configuration_Id", "ViewKey" });
            DropTable("dbo.PortfolioProjects");
            DropTable("dbo.ProjectProject1");
            DropTable("dbo.ProjectProjects");
            DropTable("dbo.Users");
            DropTable("dbo.ProjectSizes");
            DropTable("dbo.ProjectRAGStatus");
            DropTable("dbo.People");
            DropTable("dbo.ProjectUpdateItems");
            DropTable("dbo.ProjectAuditLogs");
            DropTable("dbo.Projects");
            DropTable("dbo.Portfolios");
            DropTable("dbo.ProjectOnHoldStatus");
            DropTable("dbo.ProjectPhases");
            DropTable("dbo.ProjectCategories");
            DropTable("dbo.PortfolioLabelConfigs");
            DropTable("dbo.PortfolioConfigurations");
            DropTable("dbo.BudgetTypes");
            DropTable("dbo.AccessGroups");
        }
    }
}
