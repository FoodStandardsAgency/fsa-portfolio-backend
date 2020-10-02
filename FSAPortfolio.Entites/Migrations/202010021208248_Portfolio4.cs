namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "FirstUpdate_Id");
            RenameColumn(table: "dbo.Projects", name: "FirstUpdate_Id1", newName: "FirstUpdate_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_FirstUpdate_Id1", newName: "IX_FirstUpdate_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Projects", name: "IX_FirstUpdate_Id", newName: "IX_FirstUpdate_Id1");
            RenameColumn(table: "dbo.Projects", name: "FirstUpdate_Id", newName: "FirstUpdate_Id1");
            AddColumn("dbo.Projects", "FirstUpdate_Id", c => c.Int());
        }
    }
}
