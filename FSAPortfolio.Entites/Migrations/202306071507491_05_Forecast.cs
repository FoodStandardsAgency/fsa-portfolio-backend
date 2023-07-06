namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05_Forecast : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forecasts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        Amount = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectForecasts",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Forecast_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Forecast_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Forecasts", t => t.Forecast_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Forecast_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectForecasts", "Forecast_Id", "dbo.Forecasts");
            DropForeignKey("dbo.ProjectForecasts", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectForecasts", new[] { "Forecast_Id" });
            DropIndex("dbo.ProjectForecasts", new[] { "Project_Id" });
            DropTable("dbo.ProjectForecasts");
            DropTable("dbo.Forecasts");
        }
    }
}
