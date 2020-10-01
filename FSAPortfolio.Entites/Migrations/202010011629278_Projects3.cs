namespace FSAPortfolio.Entites.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projects3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "FirstUpdate_Id", c => c.Int());
            AddColumn("dbo.Projects", "FirstUpdate_Id1", c => c.Int());
            CreateIndex("dbo.Projects", "FirstUpdate_Id1");
            AddForeignKey("dbo.Projects", "FirstUpdate_Id1", "dbo.ProjectUpdateItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "FirstUpdate_Id1", "dbo.ProjectUpdateItems");
            DropIndex("dbo.Projects", new[] { "FirstUpdate_Id1" });
            DropColumn("dbo.Projects", "FirstUpdate_Id1");
            DropColumn("dbo.Projects", "FirstUpdate_Id");
        }
    }
}
