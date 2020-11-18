namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03_LeadRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "LeadRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "LeadRole");
        }
    }
}
