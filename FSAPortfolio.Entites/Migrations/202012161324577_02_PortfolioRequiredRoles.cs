namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_PortfolioRequiredRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portfolios", "RequiredRoleData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Portfolios", "RequiredRoleData");
        }
    }
}
