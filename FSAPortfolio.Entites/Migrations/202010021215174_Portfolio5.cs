namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Portfolio5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "ProjectCategoryId_Id", newName: "ProjectCategory_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectCategoryId_Id", newName: "IX_ProjectCategory_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectCategory_Id", newName: "IX_ProjectCategoryId_Id");
            RenameColumn(table: "dbo.Projects", name: "ProjectCategory_Id", newName: "ProjectCategoryId_Id");
        }
    }
}
