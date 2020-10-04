namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRelatedProjects : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProjectProjects", newName: "DependantProjects");
            RenameTable(name: "dbo.ProjectProject1", newName: "RelatedProjects");
            RenameColumn(table: "dbo.DependantProjects", name: "Project_Id1", newName: "DependantProject_Id");
            RenameColumn(table: "dbo.RelatedProjects", name: "Project_Id1", newName: "RelatedProject_Id");
            RenameIndex(table: "dbo.DependantProjects", name: "IX_Project_Id1", newName: "IX_DependantProject_Id");
            RenameIndex(table: "dbo.RelatedProjects", name: "IX_Project_Id1", newName: "IX_RelatedProject_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.RelatedProjects", name: "IX_RelatedProject_Id", newName: "IX_Project_Id1");
            RenameIndex(table: "dbo.DependantProjects", name: "IX_DependantProject_Id", newName: "IX_Project_Id1");
            RenameColumn(table: "dbo.RelatedProjects", name: "RelatedProject_Id", newName: "Project_Id1");
            RenameColumn(table: "dbo.DependantProjects", name: "DependantProject_Id", newName: "Project_Id1");
            RenameTable(name: "dbo.RelatedProjects", newName: "ProjectProject1");
            RenameTable(name: "dbo.DependantProjects", newName: "ProjectProjects");
        }
    }
}
