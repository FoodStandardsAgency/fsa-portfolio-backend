namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07_Roles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RoleList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RoleList");
        }
    }
}
