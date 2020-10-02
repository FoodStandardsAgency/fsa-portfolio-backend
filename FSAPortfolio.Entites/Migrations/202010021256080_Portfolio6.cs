namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectCategories", "ViewKey", c => c.String(maxLength: 20));
            AddColumn("dbo.ProjectOnHoldStatus", "ViewKey", c => c.String(maxLength: 20));
            AddColumn("dbo.ProjectPhases", "ViewKey", c => c.String(maxLength: 20));
            AddColumn("dbo.ProjectRAGStatus", "ViewKey", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectRAGStatus", "ViewKey");
            DropColumn("dbo.ProjectPhases", "ViewKey");
            DropColumn("dbo.ProjectOnHoldStatus", "ViewKey");
            DropColumn("dbo.ProjectCategories", "ViewKey");
        }
    }
}
