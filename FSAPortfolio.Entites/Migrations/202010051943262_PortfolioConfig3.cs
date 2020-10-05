namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PortfolioConfig3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PortfolioConfigAuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PortfolioConfiguration_Id = c.Int(nullable: false),
                        AuditType = c.String(maxLength: 50),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.PortfolioConfiguration_Id)
                .Index(t => t.PortfolioConfiguration_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PortfolioConfigAuditLogs", "PortfolioConfiguration_Id", "dbo.PortfolioConfigurations");
            DropIndex("dbo.PortfolioConfigAuditLogs", new[] { "PortfolioConfiguration_Id" });
            DropTable("dbo.PortfolioConfigAuditLogs");
        }
    }
}
