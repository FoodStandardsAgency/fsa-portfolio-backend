namespace FSAPortfolio.Entites
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entites.Projects;

    public partial class PortfolioViewContext : DbContext
    {
        public PortfolioViewContext()
            : base("name=PortfolioViewContext")
        {
        }

        public virtual DbSet<latest_projects> latest_projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<latest_projects>()
                .Property(e => e.project_id)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.project_name)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.start_date)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.short_desc)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.phase)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.subcat)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.rag)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.oddlead)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.oddlead_email)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.servicelead)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.servicelead_email)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.priority_main)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.funded)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.confidence)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.priorities)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.benefits)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.criticality)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.budget)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.spent)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.documents)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.pgroup)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.toupdate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.rels)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.team)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.onhold)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.expend)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.hardend)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.actstart)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.dependencies)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.project_size)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.oddlead_role)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.budgettype)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.direct)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.expendp)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.g6team)
                .IsUnicode(false);

            modelBuilder.Entity<latest_projects>()
                .Property(e => e.new_flag)
                .IsUnicode(false);
        }
    }
}
