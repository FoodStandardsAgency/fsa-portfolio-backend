namespace FSAPortfolio.Entities.Migrations
{
    using FSAPortfolio.Entities.Projects;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FSAPortfolio.Entities.PortfolioContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FSAPortfolio.Entities.PortfolioContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
