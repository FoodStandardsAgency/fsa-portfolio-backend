namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ProjectSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.Name, unique: true);
            
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
            
            AddColumn("dbo.Projects", "Directorate", c => c.String(maxLength: 150));
            AddColumn("dbo.Projects", "ProjectSize_Id", c => c.Int());
            AddColumn("dbo.Projects", "BudgetType_Id", c => c.Int());
            AddColumn("dbo.Projects", "Funded", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Confidence", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Priorities", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Benefits", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Criticality", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Team", c => c.String(maxLength: 500));
            AddColumn("dbo.ProjectUpdateItems", "PercentageComplete", c => c.Single());
            AddColumn("dbo.ProjectUpdateItems", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProjectUpdateItems", "Spent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProjectUpdateItems", "ExpectedCurrentPhaseEnd", c => c.DateTime());
            CreateIndex("dbo.Projects", "ProjectSize_Id");
            CreateIndex("dbo.Projects", "BudgetType_Id");
            CreateIndex("dbo.ProjectCategories", "ViewKey", unique: true);
            CreateIndex("dbo.ProjectOnHoldStatus", "ViewKey", unique: true);
            CreateIndex("dbo.ProjectPhases", "ViewKey", unique: true);
            CreateIndex("dbo.ProjectRAGStatus", "ViewKey", unique: true);
            AddForeignKey("dbo.Projects", "BudgetType_Id", "dbo.BudgetTypes", "Id");
            AddForeignKey("dbo.Projects", "ProjectSize_Id", "dbo.ProjectSizes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectSize_Id", "dbo.ProjectSizes");
            DropForeignKey("dbo.ProjectProjects", "Project_Id1", "dbo.Projects");
            DropForeignKey("dbo.ProjectProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "BudgetType_Id", "dbo.BudgetTypes");
            DropIndex("dbo.ProjectProjects", new[] { "Project_Id1" });
            DropIndex("dbo.ProjectProjects", new[] { "Project_Id" });
            DropIndex("dbo.ProjectSizes", new[] { "Name" });
            DropIndex("dbo.ProjectSizes", new[] { "ViewKey" });
            DropIndex("dbo.ProjectRAGStatus", new[] { "ViewKey" });
            DropIndex("dbo.ProjectPhases", new[] { "ViewKey" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "ViewKey" });
            DropIndex("dbo.ProjectCategories", new[] { "ViewKey" });
            DropIndex("dbo.Projects", new[] { "BudgetType_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectSize_Id" });
            DropIndex("dbo.BudgetTypes", new[] { "Name" });
            DropIndex("dbo.BudgetTypes", new[] { "ViewKey" });
            DropColumn("dbo.ProjectUpdateItems", "ExpectedCurrentPhaseEnd");
            DropColumn("dbo.ProjectUpdateItems", "Spent");
            DropColumn("dbo.ProjectUpdateItems", "Budget");
            DropColumn("dbo.ProjectUpdateItems", "PercentageComplete");
            DropColumn("dbo.Projects", "Team");
            DropColumn("dbo.Projects", "Criticality");
            DropColumn("dbo.Projects", "Benefits");
            DropColumn("dbo.Projects", "Priorities");
            DropColumn("dbo.Projects", "Confidence");
            DropColumn("dbo.Projects", "Funded");
            DropColumn("dbo.Projects", "BudgetType_Id");
            DropColumn("dbo.Projects", "ProjectSize_Id");
            DropColumn("dbo.Projects", "Directorate");
            DropTable("dbo.ProjectProjects");
            DropTable("dbo.ProjectSizes");
            DropTable("dbo.BudgetTypes");
        }
    }
}
