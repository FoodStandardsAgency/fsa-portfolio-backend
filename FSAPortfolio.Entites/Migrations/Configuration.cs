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

            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Backlog" });
            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Discovery" });
            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Alpha" });
            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Beta" });
            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Live" });
            context.ProjectPhases.AddOrUpdate(p => p.Name, new ProjectPhase() { Name = "Completed" });

            context.ProjectRAGStatuses.AddOrUpdate(p => p.Name, new ProjectRAGStatus() { Name = "Red" });
            context.ProjectRAGStatuses.AddOrUpdate(p => p.Name, new ProjectRAGStatus() { Name = "Amber" });
            context.ProjectRAGStatuses.AddOrUpdate(p => p.Name, new ProjectRAGStatus() { Name = "Green" });
            context.ProjectRAGStatuses.AddOrUpdate(p => p.Name, new ProjectRAGStatus() { Name = "Undecided" });

            context.ProjectOnHoldStatuses.AddOrUpdate(p => p.Name, new ProjectOnHoldStatus() { Name = "No" });
            context.ProjectOnHoldStatuses.AddOrUpdate(p => p.Name, new ProjectOnHoldStatus() { Name = "On hold" });
            context.ProjectOnHoldStatuses.AddOrUpdate(p => p.Name, new ProjectOnHoldStatus() { Name = "Blocked" });
            context.ProjectOnHoldStatuses.AddOrUpdate(p => p.Name, new ProjectOnHoldStatus() { Name = "Covid-19 on hold" });
        }
    }
}
