namespace FSAPortfolio.Entities.Migrations
{
    using FSAPortfolio.Entities.Projects;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PortfolioContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PortfolioContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
