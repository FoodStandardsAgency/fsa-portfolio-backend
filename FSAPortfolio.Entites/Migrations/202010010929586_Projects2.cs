namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projects2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Timestamp");
        }
    }
}
