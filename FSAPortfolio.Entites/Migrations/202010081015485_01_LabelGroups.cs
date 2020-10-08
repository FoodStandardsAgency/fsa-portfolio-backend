namespace FSAPortfolio.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_LabelGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PortfolioLabelGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Configuration_Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortfolioConfigurations", t => t.Configuration_Id)
                .Index(t => t.Configuration_Id);
            
            AddColumn("dbo.PortfolioLabelConfigs", "Group_Id", c => c.Int());
            CreateIndex("dbo.PortfolioLabelConfigs", "Group_Id");
            AddForeignKey("dbo.PortfolioLabelConfigs", "Group_Id", "dbo.PortfolioLabelGroups", "Id");
            DropColumn("dbo.PortfolioLabelConfigs", "FieldGroup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PortfolioLabelConfigs", "FieldGroup", c => c.String(maxLength: 50));
            DropForeignKey("dbo.PortfolioLabelGroups", "Configuration_Id", "dbo.PortfolioConfigurations");
            DropForeignKey("dbo.PortfolioLabelConfigs", "Group_Id", "dbo.PortfolioLabelGroups");
            DropIndex("dbo.PortfolioLabelConfigs", new[] { "Group_Id" });
            DropIndex("dbo.PortfolioLabelGroups", new[] { "Configuration_Id" });
            DropColumn("dbo.PortfolioLabelConfigs", "Group_Id");
            DropTable("dbo.PortfolioLabelGroups");
        }
    }
}
