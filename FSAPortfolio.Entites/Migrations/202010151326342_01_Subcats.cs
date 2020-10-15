namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_Subcats : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectSubcategories", "Subcategory_Id", "dbo.ProjectCategories");
            DropForeignKey("dbo.ProjectSubcategories", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectSubcategories", new[] { "Subcategory_Id" });
            DropIndex("dbo.ProjectSubcategories", new[] { "Project_Id" });
            DropTable("dbo.ProjectSubcategories");
        }
    }
}
