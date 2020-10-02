namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(maxLength: 20),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ShortName, unique: true);
            
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
            
            CreateIndex("dbo.Projects", "ProjectId", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PortfolioProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.PortfolioProjects", "Portfolio_Id", "dbo.Portfolios");
            DropIndex("dbo.PortfolioProjects", new[] { "Project_Id" });
            DropIndex("dbo.PortfolioProjects", new[] { "Portfolio_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectId" });
            DropIndex("dbo.Portfolios", new[] { "ShortName" });
            DropTable("dbo.PortfolioProjects");
            DropTable("dbo.Portfolios");
        }
    }
}
