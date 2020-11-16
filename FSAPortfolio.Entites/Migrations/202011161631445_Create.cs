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
                        Portfolio_Id = c.Int(nullable: false),
                        CompletedPhase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Portfolio_Id)
                .ForeignKey("dbo.ProjectPhases", t => t.CompletedPhase_Id)
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .Index(t => t.Portfolio_Id)
                .Index(t => t.CompletedPhase_Id);
            
            CreateTable(
                "dbo.PortfolioConfigAuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PortfolioConfiguration_Id = c.Int(nullable: false),
                        AuditType = c.String(maxLength: 50),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.PortfolioConfiguration_Id)
                .Index(t => t.PortfolioConfiguration_Id);
            
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
                "dbo.PortfolioLabelGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Configuration_Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => t.Configuration_Id);
            
            CreateTable(
                "dbo.PortfolioLabelConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Configuration_Id = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        FieldName = c.String(maxLength: 50),
                        FieldTitle = c.String(maxLength: 50),
                        FieldOrder = c.Int(nullable: false),
                        Included = c.Boolean(nullable: false),
                        IncludedLock = c.Boolean(nullable: false),
                        AdminOnly = c.Boolean(nullable: false),
                        AdminOnlyLock = c.Boolean(nullable: false),
                        Label = c.String(maxLength: 50),
                        FieldType = c.Int(nullable: false),
                        FieldTypeLocked = c.Boolean(nullable: false),
                        FieldOptions = c.String(),
                        MasterLabel_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioLabelConfigs", t => t.MasterLabel_Id)
                .ForeignKey("dbo.PortfolioLabelGroups", t => t.Group_Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => new { t.Configuration_Id, t.FieldName }, unique: true)
                .Index(t => new { t.Configuration_Id, t.FieldTitle }, unique: true)
                .Index(t => t.MasterLabel_Id)
                .Index(t => t.Group_Id);
            
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
                        IDPrefix = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.ShortName, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.IDPrefix, unique: true);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectReservation_Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 1000),
                        Theme = c.String(maxLength: 50),
                        ProjectType = c.String(maxLength: 50),
                        StrategicObjectives = c.String(maxLength: 50),
                        Programme = c.String(maxLength: 150),
                        ProjectCategory_Id = c.Int(),
                        ProjectSize_Id = c.Int(),
                        BudgetType_Id = c.Int(),
                        Funded = c.Int(nullable: false),
                        Confidence = c.Int(nullable: false),
                        Priorities = c.Int(nullable: false),
                        Benefits = c.Int(nullable: false),
                        Criticality = c.Int(nullable: false),
                        ChannelLink_Name = c.String(maxLength: 150),
                        ChannelLink_Link = c.String(maxLength: 250),
                        Lead_Id = c.Int(),
                        ServiceLead_Id = c.Int(),
                        Supplier = c.String(maxLength: 150),
                        Priority = c.Int(),
                        StartDate_Date = c.DateTime(),
                        StartDate_Flags = c.Int(nullable: false),
                        ActualStartDate_Date = c.DateTime(),
                        ActualStartDate_Flags = c.Int(nullable: false),
                        ExpectedEndDate_Date = c.DateTime(),
                        ExpectedEndDate_Flags = c.Int(nullable: false),
                        HardEndDate_Date = c.DateTime(),
                        HardEndDate_Flags = c.Int(nullable: false),
                        ActualEndDate_Date = c.DateTime(),
                        ActualEndDate_Flags = c.Int(nullable: false),
                        BusinessCaseNumber = c.String(maxLength: 50),
                        FSNumber = c.String(maxLength: 50),
                        RiskRating = c.String(maxLength: 50),
                        ProgrammeDescription = c.String(),
                        LatestUpdate_Id = c.Int(),
                        FirstUpdate_Id = c.Int(),
                        Directorate_Id = c.Int(),
                        KeyContact1_Id = c.Int(),
                        KeyContact2_Id = c.Int(),
                        KeyContact3_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectReservation_Id)
                .ForeignKey("dbo.BudgetTypes", t => t.BudgetType_Id)
                .ForeignKey("dbo.ProjectCategories", t => t.ProjectCategory_Id)
                .ForeignKey("dbo.Directorates", t => t.Directorate_Id)
                .ForeignKey("dbo.ProjectUpdateItems", t => t.FirstUpdate_Id)
                .ForeignKey("dbo.People", t => t.KeyContact1_Id)
                .ForeignKey("dbo.People", t => t.KeyContact2_Id)
                .ForeignKey("dbo.People", t => t.KeyContact3_Id)
                .ForeignKey("dbo.ProjectUpdateItems", t => t.LatestUpdate_Id)
                .ForeignKey("dbo.People", t => t.Lead_Id)
                .ForeignKey("dbo.ProjectReservations", t => t.ProjectReservation_Id)
                .ForeignKey("dbo.People", t => t.ServiceLead_Id)
                .ForeignKey("dbo.ProjectSizes", t => t.ProjectSize_Id)
                .Index(t => t.ProjectReservation_Id)
                .Index(t => t.ProjectCategory_Id)
                .Index(t => t.ProjectSize_Id)
                .Index(t => t.BudgetType_Id)
                .Index(t => t.Lead_Id)
                .Index(t => t.ServiceLead_Id)
                .Index(t => t.LatestUpdate_Id)
                .Index(t => t.FirstUpdate_Id)
                .Index(t => t.Directorate_Id)
                .Index(t => t.KeyContact1_Id)
                .Index(t => t.KeyContact2_Id)
                .Index(t => t.KeyContact3_Id);
            
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
                "dbo.Directorates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 10),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        Link = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        ExpectedCurrentPhaseEnd_Date = c.DateTime(),
                        ExpectedCurrentPhaseEnd_Flags = c.Int(nullable: false),
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
                        Team_Id = c.Int(),
                        ActiveDirectoryPrincipalName = c.String(maxLength: 150),
                        ActiveDirectoryId = c.String(maxLength: 150),
                        Department = c.String(maxLength: 150),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
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
                "dbo.ProjectDataItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(nullable: false),
                        Label_Id = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioLabelConfigs", t => t.Label_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Label_Id);
            
            CreateTable(
                "dbo.ProjectReservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Portfolio_Id = c.Int(nullable: false),
                        ProjectId = c.String(maxLength: 20),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Index = c.Int(nullable: false),
                        ReservedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .Index(t => new { t.Portfolio_Id, t.Year, t.Month, t.Index }, unique: true)
                .Index(t => t.ProjectId, unique: true);
            
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
                "dbo.DependantProjects",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        DependantProject_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.DependantProject_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Projects", t => t.DependantProject_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.DependantProject_Id);
            
            CreateTable(
                "dbo.ProjectDocuments",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Document_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Document_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Documents", t => t.Document_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Document_Id);
            
            CreateTable(
                "dbo.ProjectTeamMembers",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Person_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Person_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.RelatedProjects",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        RelatedProject_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.RelatedProject_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Projects", t => t.RelatedProject_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.RelatedProject_Id);
            
            CreateTable(
                "dbo.ProjectSubcategories",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Subcategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Subcategory_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.ProjectCategories", t => t.Subcategory_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Subcategory_Id);
            
            CreateTable(
                "dbo.PortfolioProjects",
                c => new
                    {
                        Portfolio_Id = c.Int(nullable: false),
                        Project_ProjectReservation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Portfolio_Id, t.Project_ProjectReservation_Id })
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectReservation_Id)
                .Index(t => t.Portfolio_Id)
                .Index(t => t.Project_ProjectReservation_Id);
            
            CreateTable(
                "dbo.PortfolioTeams",
                c => new
                    {
                        Portfolio_Id = c.Int(nullable: false),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Portfolio_Id, t.Team_Id })
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Portfolio_Id)
                .Index(t => t.Team_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.ProjectRAGStatus", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.ProjectSizes", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.PortfolioTeams", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.PortfolioProjects", "Project_ProjectReservation_Id", "dbo.Projects");
            DropForeignKey("dbo.PortfolioProjects", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.ProjectUpdateItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectSubcategories", "Subcategory_Id", "dbo.ProjectCategories");
            DropForeignKey("dbo.ProjectSubcategories", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectSize_Id", "dbo.ProjectSizes");
            DropForeignKey("dbo.Projects", "ServiceLead_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "ProjectReservation_Id", "dbo.ProjectReservations");
            DropForeignKey("dbo.ProjectReservations", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.RelatedProjects", "RelatedProject_Id", "dbo.Projects");
            DropForeignKey("dbo.RelatedProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectDataItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectDataItems", "Label_Id", "dbo.PortfolioLabelConfigs");
            DropForeignKey("dbo.ProjectTeamMembers", "Person_Id", "dbo.People");
            DropForeignKey("dbo.ProjectTeamMembers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Lead_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "LatestUpdate_Id", "dbo.ProjectUpdateItems");
            DropForeignKey("dbo.Projects", "KeyContact3_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "KeyContact2_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "KeyContact1_Id", "dbo.People");
            DropForeignKey("dbo.Projects", "FirstUpdate_Id", "dbo.ProjectUpdateItems");
            DropForeignKey("dbo.ProjectUpdateItems", "RAGStatus_Id", "dbo.ProjectRAGStatus");
            DropForeignKey("dbo.ProjectUpdateItems", "Phase_Id", "dbo.ProjectPhases");
            DropForeignKey("dbo.ProjectUpdateItems", "Person_Id", "dbo.People");
            DropForeignKey("dbo.People", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.ProjectUpdateItems", "OnHoldStatus_Id", "dbo.ProjectOnHoldStatus");
            DropForeignKey("dbo.ProjectDocuments", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.ProjectDocuments", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Directorate_Id", "dbo.Directorates");
            DropForeignKey("dbo.DependantProjects", "DependantProject_Id", "dbo.Projects");
            DropForeignKey("dbo.DependantProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectCategory_Id", "dbo.ProjectCategories");
            DropForeignKey("dbo.Projects", "BudgetType_Id", "dbo.BudgetTypes");
            DropForeignKey("dbo.ProjectAuditLogs", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.PortfolioConfigurations", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.ProjectPhases", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.ProjectOnHoldStatus", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioLabelConfigs", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioLabelGroups", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioLabelConfigs", "Group_Id", "dbo.PortfolioLabelGroups");
            DropForeignKey("dbo.PortfolioLabelConfigs", "MasterLabel_Id", "dbo.PortfolioLabelConfigs");
            DropForeignKey("dbo.PortfolioConfigurations", "CompletedPhase_Id", "dbo.ProjectPhases");
            DropForeignKey("dbo.ProjectCategories", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.BudgetTypes", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioConfigAuditLogs", "PortfolioConfiguration_Id", "dbo.PortfolioConfigurations");
            DropIndex("dbo.PortfolioTeams", new[] { "Team_Id" });
            DropIndex("dbo.PortfolioTeams", new[] { "Portfolio_Id" });
            DropIndex("dbo.PortfolioProjects", new[] { "Project_ProjectReservation_Id" });
            DropIndex("dbo.PortfolioProjects", new[] { "Portfolio_Id" });
            DropIndex("dbo.ProjectSubcategories", new[] { "Subcategory_Id" });
            DropIndex("dbo.ProjectSubcategories", new[] { "Project_Id" });
            DropIndex("dbo.RelatedProjects", new[] { "RelatedProject_Id" });
            DropIndex("dbo.RelatedProjects", new[] { "Project_Id" });
            DropIndex("dbo.ProjectTeamMembers", new[] { "Person_Id" });
            DropIndex("dbo.ProjectTeamMembers", new[] { "Project_Id" });
            DropIndex("dbo.ProjectDocuments", new[] { "Document_Id" });
            DropIndex("dbo.ProjectDocuments", new[] { "Project_Id" });
            DropIndex("dbo.DependantProjects", new[] { "DependantProject_Id" });
            DropIndex("dbo.DependantProjects", new[] { "Project_Id" });
            DropIndex("dbo.Users", new[] { "AccessGroupId" });
            DropIndex("dbo.ProjectSizes", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectSizes", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectReservations", new[] { "ProjectId" });
            DropIndex("dbo.ProjectReservations", new[] { "Portfolio_Id", "Year", "Month", "Index" });
            DropIndex("dbo.ProjectDataItems", new[] { "Label_Id" });
            DropIndex("dbo.ProjectDataItems", new[] { "Project_Id" });
            DropIndex("dbo.ProjectRAGStatus", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectRAGStatus", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.People", new[] { "Team_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "RAGStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Phase_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Person_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "OnHoldStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Project_Id" });
            DropIndex("dbo.Directorates", new[] { "Name" });
            DropIndex("dbo.Directorates", new[] { "ViewKey" });
            DropIndex("dbo.ProjectAuditLogs", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "KeyContact3_Id" });
            DropIndex("dbo.Projects", new[] { "KeyContact2_Id" });
            DropIndex("dbo.Projects", new[] { "KeyContact1_Id" });
            DropIndex("dbo.Projects", new[] { "Directorate_Id" });
            DropIndex("dbo.Projects", new[] { "FirstUpdate_Id" });
            DropIndex("dbo.Projects", new[] { "LatestUpdate_Id" });
            DropIndex("dbo.Projects", new[] { "ServiceLead_Id" });
            DropIndex("dbo.Projects", new[] { "Lead_Id" });
            DropIndex("dbo.Projects", new[] { "BudgetType_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectSize_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectCategory_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectReservation_Id" });
            DropIndex("dbo.Portfolios", new[] { "IDPrefix" });
            DropIndex("dbo.Portfolios", new[] { "Name" });
            DropIndex("dbo.Portfolios", new[] { "ShortName" });
            DropIndex("dbo.Portfolios", new[] { "ViewKey" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Group_Id" });
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "MasterLabel_Id" });
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Configuration_Id", "FieldTitle" });
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Configuration_Id", "FieldName" });
            DropIndex("dbo.PortfolioLabelGroups", new[] { "Configuration_Id" });
            DropIndex("dbo.ProjectPhases", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectPhases", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.ProjectCategories", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.ProjectCategories", new[] { "Configuration_Id", "ViewKey" });
            DropIndex("dbo.PortfolioConfigAuditLogs", new[] { "PortfolioConfiguration_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "CompletedPhase_Id" });
            DropIndex("dbo.PortfolioConfigurations", new[] { "Portfolio_Id" });
            DropIndex("dbo.BudgetTypes", new[] { "Configuration_Id", "Name" });
            DropIndex("dbo.BudgetTypes", new[] { "Configuration_Id", "ViewKey" });
            DropTable("dbo.PortfolioTeams");
            DropTable("dbo.PortfolioProjects");
            DropTable("dbo.ProjectSubcategories");
            DropTable("dbo.RelatedProjects");
            DropTable("dbo.ProjectTeamMembers");
            DropTable("dbo.ProjectDocuments");
            DropTable("dbo.DependantProjects");
            DropTable("dbo.Users");
            DropTable("dbo.ProjectSizes");
            DropTable("dbo.ProjectReservations");
            DropTable("dbo.ProjectDataItems");
            DropTable("dbo.ProjectRAGStatus");
            DropTable("dbo.Teams");
            DropTable("dbo.People");
            DropTable("dbo.ProjectUpdateItems");
            DropTable("dbo.Documents");
            DropTable("dbo.Directorates");
            DropTable("dbo.ProjectAuditLogs");
            DropTable("dbo.Projects");
            DropTable("dbo.Portfolios");
            DropTable("dbo.ProjectOnHoldStatus");
            DropTable("dbo.PortfolioLabelConfigs");
            DropTable("dbo.PortfolioLabelGroups");
            DropTable("dbo.ProjectPhases");
            DropTable("dbo.ProjectCategories");
            DropTable("dbo.PortfolioConfigAuditLogs");
            DropTable("dbo.PortfolioConfigurations");
            DropTable("dbo.BudgetTypes");
            DropTable("dbo.AccessGroups");
        }
    }
}
