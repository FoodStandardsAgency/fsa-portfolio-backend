namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06_AssuranceGateCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "AssuranceGateCompletedDate_Date", c => c.DateTime());
            AddColumn("dbo.Projects", "AssuranceGateCompletedDate_Flags", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "AssuranceGateCompletedDate_Flags");
            DropColumn("dbo.Projects", "AssuranceGateCompletedDate_Date");
        }
    }
}
