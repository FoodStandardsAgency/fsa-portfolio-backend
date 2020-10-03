namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectAuditLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            AlterColumn("dbo.Projects", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectAuditLogs", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectAuditLogs", new[] { "Project_Id" });
            AlterColumn("dbo.Projects", "Priority", c => c.Int(nullable: false));
            DropTable("dbo.ProjectAuditLogs");
        }
    }
}
