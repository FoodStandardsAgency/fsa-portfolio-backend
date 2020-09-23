namespace FSAPortfolio.PostgreSQL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entites.Projects;
    using FSAPortfolio.Entites.Users;
    using FSAPortfolio.Entites;

    public partial class MigratePortfolioContext : DbContext
    {
        public MigratePortfolioContext()
            : base("name=MigratePortfolioContext")
        {
        }

        public virtual DbSet<odd_people> odd_people { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<odd_people>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<odd_people>()
                .Property(e => e.firstname)
                .IsUnicode(false);

            modelBuilder.Entity<odd_people>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<odd_people>()
                .Property(e => e.g6team)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.project_id)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.project_name)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.start_date)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.short_desc)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.phase)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.subcat)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.rag)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.oddlead)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.oddlead_email)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.servicelead)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.servicelead_email)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.priority_main)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.funded)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.confidence)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.priorities)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.benefits)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.criticality)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.budget)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.spent)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.documents)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.pgroup)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.toupdate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.rels)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.team)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.onhold)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.expend)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.hardend)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.actstart)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.dependencies)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.project_size)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.oddlead_role)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.budgettype)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.direct)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.expendp)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.pass_hash)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.access_group)
                .IsUnicode(false);
        }
    }
}
