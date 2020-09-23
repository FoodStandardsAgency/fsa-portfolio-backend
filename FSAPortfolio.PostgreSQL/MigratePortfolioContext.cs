namespace FSAPortfolio.PostgreSQL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entites.Projects;
    using FSAPortfolio.Entites.Users;
    using FSAPortfolio.Entites;
    using System.Configuration;

    public partial class MigratePortfolioContext : PortfolioContext
    {
        public MigratePortfolioContext()
            : this(ConfigurationManager.ConnectionStrings["MigratePortfolioContext"])
        {
        }

        public MigratePortfolioContext(ConnectionStringSettings cs)
            : base(cs.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

    }
}
