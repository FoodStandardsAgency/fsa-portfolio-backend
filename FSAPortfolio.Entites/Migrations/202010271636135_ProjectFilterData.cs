namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectFilterData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Theme", c => c.String(maxLength: 50));
            AddColumn("dbo.Projects", "ProjectType", c => c.String(maxLength: 50));
            AddColumn("dbo.Projects", "StrategicObjectives", c => c.String(maxLength: 50));
            AddColumn("dbo.Projects", "Programme", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Programme");
            DropColumn("dbo.Projects", "StrategicObjectives");
            DropColumn("dbo.Projects", "ProjectType");
            DropColumn("dbo.Projects", "Theme");
        }
    }
}
