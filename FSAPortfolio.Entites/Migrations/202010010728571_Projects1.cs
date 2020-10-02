namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projects1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Lead_Id", c => c.Int());
            AddColumn("dbo.People", "Surname", c => c.String(maxLength: 250));
            AddColumn("dbo.People", "Firstname", c => c.String(maxLength: 250));
            AddColumn("dbo.People", "Email", c => c.String(maxLength: 250));
            AddColumn("dbo.People", "G6team", c => c.String(maxLength: 50));
            CreateIndex("dbo.Projects", "Lead_Id");
            AddForeignKey("dbo.Projects", "Lead_Id", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Lead_Id", "dbo.People");
            DropIndex("dbo.Projects", new[] { "Lead_Id" });
            DropColumn("dbo.People", "G6team");
            DropColumn("dbo.People", "Email");
            DropColumn("dbo.People", "Firstname");
            DropColumn("dbo.People", "Surname");
            DropColumn("dbo.Projects", "Lead_Id");
        }
    }
}
