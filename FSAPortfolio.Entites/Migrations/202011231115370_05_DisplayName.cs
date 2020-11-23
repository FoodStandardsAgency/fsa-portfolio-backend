namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05_DisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "ActiveDirectoryDisplayName", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "ActiveDirectoryDisplayName");
        }
    }
}
