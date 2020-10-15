namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_Directorates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Directorates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewKey = c.String(maxLength: 10),
                        Name = c.String(maxLength: 250),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ViewKey, unique: true)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Directorates", new[] { "Name" });
            DropIndex("dbo.Directorates", new[] { "ViewKey" });
            DropTable("dbo.Directorates");
        }
    }
}
