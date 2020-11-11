namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_Teams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        Portfolio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Portfolios", t => t.Portfolio_Id)
                .Index(t => t.Portfolio_Id);
            
            AddColumn("dbo.Projects", "Directorate_Id", c => c.Int());
            AddColumn("dbo.People", "Team_Id", c => c.Int());
            CreateIndex("dbo.Projects", "Directorate_Id");
            CreateIndex("dbo.People", "Team_Id");
            AddForeignKey("dbo.Projects", "Directorate_Id", "dbo.Directorates", "Id");
            AddForeignKey("dbo.People", "Team_Id", "dbo.Teams", "Id");
            DropColumn("dbo.Projects", "Directorate");
            DropColumn("dbo.People", "G6team");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "G6team", c => c.String(maxLength: 50));
            AddColumn("dbo.Projects", "Directorate", c => c.String(maxLength: 150));
            DropForeignKey("dbo.Teams", "Portfolio_Id", "dbo.Portfolios");
            DropForeignKey("dbo.People", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Projects", "Directorate_Id", "dbo.Directorates");
            DropIndex("dbo.Teams", new[] { "Portfolio_Id" });
            DropIndex("dbo.People", new[] { "Team_Id" });
            DropIndex("dbo.Projects", new[] { "Directorate_Id" });
            DropColumn("dbo.People", "Team_Id");
            DropColumn("dbo.Projects", "Directorate_Id");
            DropTable("dbo.Teams");
        }
    }
}
