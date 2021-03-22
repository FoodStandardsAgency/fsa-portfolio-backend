namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_NoDescriptionSizeLimit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Description", c => c.String(maxLength: 1000));
        }
    }
}
