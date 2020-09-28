namespace FSAPortfolio.Entites.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
            DropIndex("dbo.Users", new[] { "AccessGroupId" });
            DropTable("dbo.Users");
            DropTable("dbo.AccessGroups");
        }
    }
}
