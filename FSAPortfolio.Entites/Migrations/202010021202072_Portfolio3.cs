namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            AddColumn("dbo.Portfolios", "Route", c => c.String(maxLength: 10));
            AddColumn("dbo.Projects", "ProjectCategoryId_Id", c => c.Int());
            AddColumn("dbo.ProjectOnHoldStatus", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectPhases", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectRAGStatus", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectOnHoldStatus", "Name", c => c.String(maxLength: 20));
            AlterColumn("dbo.ProjectPhases", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.ProjectRAGStatus", "Name", c => c.String(maxLength: 20));
            CreateIndex("dbo.Projects", "ProjectCategoryId_Id");
            CreateIndex("dbo.ProjectOnHoldStatus", "Name", unique: true);
            CreateIndex("dbo.ProjectPhases", "Name", unique: true);
            CreateIndex("dbo.ProjectRAGStatus", "Name", unique: true);
            AddForeignKey("dbo.Projects", "ProjectCategoryId_Id", "dbo.ProjectCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectCategoryId_Id", "dbo.ProjectCategories");
            DropIndex("dbo.ProjectRAGStatus", new[] { "Name" });
            DropIndex("dbo.ProjectPhases", new[] { "Name" });
            DropIndex("dbo.ProjectOnHoldStatus", new[] { "Name" });
            DropIndex("dbo.ProjectCategories", new[] { "Name" });
            DropIndex("dbo.Projects", new[] { "ProjectCategoryId_Id" });
            AlterColumn("dbo.ProjectRAGStatus", "Name", c => c.String());
            AlterColumn("dbo.ProjectPhases", "Name", c => c.String());
            AlterColumn("dbo.ProjectOnHoldStatus", "Name", c => c.String());
            DropColumn("dbo.ProjectRAGStatus", "Order");
            DropColumn("dbo.ProjectPhases", "Order");
            DropColumn("dbo.ProjectOnHoldStatus", "Order");
            DropColumn("dbo.Projects", "ProjectCategoryId_Id");
            DropColumn("dbo.Portfolios", "Route");
            DropTable("dbo.ProjectCategories");
        }
    }
}
