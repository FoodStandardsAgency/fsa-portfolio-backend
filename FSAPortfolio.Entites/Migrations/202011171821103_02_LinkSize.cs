namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_LinkSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "Link", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "Link", c => c.String(maxLength: 250));
        }
    }
}
