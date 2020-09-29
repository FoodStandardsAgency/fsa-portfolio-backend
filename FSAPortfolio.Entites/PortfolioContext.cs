namespace FSAPortfolio.Entites
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entites.Users;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using FSAPortfolio.Entites.Projects;

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
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectRAGStatus> ProjectRAGStatuses { get; set; }
        public virtual DbSet<ProjectOnHoldStatus> ProjectOnHoldStatuses { get; set; }
        public virtual DbSet<ProjectPhase> ProjectPhases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasRequired(u => u.AccessGroup).WithMany().HasForeignKey(u => u.AccessGroupId);

            modelBuilder.Entity<AccessGroup>().HasKey(u => u.Id);

            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().HasMany(p => p.Updates).WithRequired(u => u.Project).HasForeignKey(u => u.Project_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.LatestUpdate).WithMany().HasForeignKey(p => p.LatestUpdate_Id);

            modelBuilder.Entity<ProjectUpdateItem>().HasKey(u => u.Id);
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.RAGStatus).WithMany();
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.OnHoldStatus).WithMany();
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.Phase).WithMany();
        }
    }
}
