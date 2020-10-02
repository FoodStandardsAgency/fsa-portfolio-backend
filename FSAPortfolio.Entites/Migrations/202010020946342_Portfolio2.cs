namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ServiceLead_Id", c => c.Int());
            CreateIndex("dbo.Projects", "ServiceLead_Id");
            AddForeignKey("dbo.Projects", "ServiceLead_Id", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ServiceLead_Id", "dbo.People");
            DropIndex("dbo.Projects", new[] { "ServiceLead_Id" });
            DropColumn("dbo.Projects", "ServiceLead_Id");
        }
    }
}
