namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_GenericSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "TeamSettings_Setting1", c => c.String());
            AddColumn("dbo.Projects", "TeamSettings_Setting2", c => c.String());
            AddColumn("dbo.Projects", "TeamSettings_Option1", c => c.String());
            AddColumn("dbo.Projects", "TeamSettings_Option2", c => c.String());
            AddColumn("dbo.Projects", "PlanSettings_Setting1", c => c.String());
            AddColumn("dbo.Projects", "PlanSettings_Setting2", c => c.String());
            AddColumn("dbo.Projects", "PlanSettings_Option1", c => c.String());
            AddColumn("dbo.Projects", "PlanSettings_Option2", c => c.String());
            AddColumn("dbo.Projects", "ProgressSettings_Setting1", c => c.String());
            AddColumn("dbo.Projects", "ProgressSettings_Setting2", c => c.String());
            AddColumn("dbo.Projects", "ProgressSettings_Option1", c => c.String());
            AddColumn("dbo.Projects", "ProgressSettings_Option2", c => c.String());
            AddColumn("dbo.Projects", "BudgetSettings_Setting1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Projects", "BudgetSettings_Setting2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Projects", "BudgetSettings_Option1", c => c.String());
            AddColumn("dbo.Projects", "BudgetSettings_Option2", c => c.String());
            AddColumn("dbo.Projects", "ProcessSettings_Setting1", c => c.String());
            AddColumn("dbo.Projects", "ProcessSettings_Setting2", c => c.String());
            AddColumn("dbo.Projects", "ProcessSettings_Option1", c => c.String());
            AddColumn("dbo.Projects", "ProcessSettings_Option2", c => c.String());
            AddColumn("dbo.Projects", "HowToGetToGreen", c => c.String());
            AddColumn("dbo.Projects", "ForwardLook", c => c.String());
            AddColumn("dbo.Projects", "EmergingIssues", c => c.String());
            AddColumn("dbo.Projects", "ForecastSpend", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "CostCentre", c => c.String());
            AddColumn("dbo.Projects", "AssuranceGateNumber", c => c.String());
            AddColumn("dbo.Projects", "NextAssuranceGateNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "NextAssuranceGateNumber");
            DropColumn("dbo.Projects", "AssuranceGateNumber");
            DropColumn("dbo.Projects", "CostCentre");
            DropColumn("dbo.Projects", "ForecastSpend");
            DropColumn("dbo.Projects", "EmergingIssues");
            DropColumn("dbo.Projects", "ForwardLook");
            DropColumn("dbo.Projects", "HowToGetToGreen");
            DropColumn("dbo.Projects", "ProcessSettings_Option2");
            DropColumn("dbo.Projects", "ProcessSettings_Option1");
            DropColumn("dbo.Projects", "ProcessSettings_Setting2");
            DropColumn("dbo.Projects", "ProcessSettings_Setting1");
            DropColumn("dbo.Projects", "BudgetSettings_Option2");
            DropColumn("dbo.Projects", "BudgetSettings_Option1");
            DropColumn("dbo.Projects", "BudgetSettings_Setting2");
            DropColumn("dbo.Projects", "BudgetSettings_Setting1");
            DropColumn("dbo.Projects", "ProgressSettings_Option2");
            DropColumn("dbo.Projects", "ProgressSettings_Option1");
            DropColumn("dbo.Projects", "ProgressSettings_Setting2");
            DropColumn("dbo.Projects", "ProgressSettings_Setting1");
            DropColumn("dbo.Projects", "PlanSettings_Option2");
            DropColumn("dbo.Projects", "PlanSettings_Option1");
            DropColumn("dbo.Projects", "PlanSettings_Setting2");
            DropColumn("dbo.Projects", "PlanSettings_Setting1");
            DropColumn("dbo.Projects", "TeamSettings_Option2");
            DropColumn("dbo.Projects", "TeamSettings_Option1");
            DropColumn("dbo.Projects", "TeamSettings_Setting2");
            DropColumn("dbo.Projects", "TeamSettings_Setting1");
        }
    }
}
