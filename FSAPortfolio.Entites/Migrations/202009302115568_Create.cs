namespace FSAPortfolio.Entites.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectOnHoldStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectPhases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectRAGStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.String(maxLength: 10),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 1000),
                        Priority = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        ActualStartDate = c.DateTime(),
                        ExpectedEndDate = c.DateTime(),
                        HardEndDate = c.DateTime(),
                        LatestUpdate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectUpdateItems", t => t.LatestUpdate_Id)
                .Index(t => t.LatestUpdate_Id);
            
            CreateTable(
                "dbo.ProjectUpdateItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                        SyncId = c.Int(nullable: false),
                        OnHoldStatus_Id = c.Int(),
                        Person_Id = c.Int(),
                        Phase_Id = c.Int(),
                        RAGStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectOnHoldStatus", t => t.OnHoldStatus_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.ProjectPhases", t => t.Phase_Id)
                .ForeignKey("dbo.ProjectRAGStatus", t => t.RAGStatus_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.OnHoldStatus_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Phase_Id)
                .Index(t => t.RAGStatus_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        UserName = c.String(maxLength: 50),
                        PasswordHash = c.String(maxLength: 300),
                        AccessGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessGroups", t => t.AccessGroupId)
                .Index(t => t.AccessGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.ProjectUpdateItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "LatestUpdate_Id", "dbo.ProjectUpdateItems");
            DropForeignKey("dbo.ProjectUpdateItems", "RAGStatus_Id", "dbo.ProjectRAGStatus");
            DropForeignKey("dbo.ProjectUpdateItems", "Phase_Id", "dbo.ProjectPhases");
            DropForeignKey("dbo.ProjectUpdateItems", "Person_Id", "dbo.People");
            DropForeignKey("dbo.ProjectUpdateItems", "OnHoldStatus_Id", "dbo.ProjectOnHoldStatus");
            DropIndex("dbo.Users", new[] { "AccessGroupId" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "RAGStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Phase_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Person_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "OnHoldStatus_Id" });
            DropIndex("dbo.ProjectUpdateItems", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "LatestUpdate_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.People");
            DropTable("dbo.ProjectUpdateItems");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectRAGStatus");
            DropTable("dbo.ProjectPhases");
            DropTable("dbo.ProjectOnHoldStatus");
            DropTable("dbo.AccessGroups");
        }
    }
}
