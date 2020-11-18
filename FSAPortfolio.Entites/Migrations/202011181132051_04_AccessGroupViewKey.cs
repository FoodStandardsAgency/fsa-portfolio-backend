namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_AccessGroupViewKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AccessGroups", "ViewKey", unique: true);
            CreateIndex("dbo.AccessGroups", "Description", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AccessGroups", new[] { "Description" });
            DropIndex("dbo.AccessGroups", new[] { "ViewKey" });
        }
    }
}
