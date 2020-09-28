namespace FSAPortfolio.Entites
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entites.Users;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class PortfolioContext : DbContext
    {
        public PortfolioContext()
            : base("PortfolioContext")
        {
        }
        public PortfolioContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AccessGroup> AccessGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasRequired(u => u.AccessGroup).WithMany().HasForeignKey(u => u.AccessGroupId);

            modelBuilder.Entity<AccessGroup>().HasKey(u => u.Id);

        }
    }
}
