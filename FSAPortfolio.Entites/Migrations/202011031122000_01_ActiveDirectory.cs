namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_ActiveDirectory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "ActiveDirectoryPrincipleName", c => c.String(maxLength: 150));
            AddColumn("dbo.People", "ActiveDirectoryId", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "ActiveDirectoryId");
            DropColumn("dbo.People", "ActiveDirectoryPrincipleName");
        }
    }
}
