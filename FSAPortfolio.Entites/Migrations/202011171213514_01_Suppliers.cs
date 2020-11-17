namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_Suppliers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessGroups", "ViewKey", c => c.String(maxLength: 50));
            AddColumn("dbo.AccessGroups", "Description", c => c.String(maxLength: 50));
            CreateIndex("dbo.Users", "UserName", unique: true);
            DropColumn("dbo.AccessGroups", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccessGroups", "Name", c => c.String(maxLength: 50));
            DropIndex("dbo.Users", new[] { "UserName" });
            DropColumn("dbo.AccessGroups", "Description");
            DropColumn("dbo.AccessGroups", "ViewKey");
        }
    }
}
