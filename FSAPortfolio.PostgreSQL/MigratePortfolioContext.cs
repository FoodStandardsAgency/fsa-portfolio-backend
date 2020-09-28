namespace FSAPortfolio.PostgreSQL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.PostgreSQL.Projects;
    using FSAPortfolio.PostgreSQL.Users;
    using System.Configuration;

    public partial class MigratePortfolioContext : DbContext
    {
        public MigratePortfolioContext()
            : this(ConfigurationManager.ConnectionStrings["MigratePortfolioContext"])
        {
        }

        public MigratePortfolioContext(ConnectionStringSettings cs)
            : base(cs.ConnectionString)
        {
        }

        public virtual DbSet<odd_people> odd_people { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<user> users { get; set; }

        public virtual DbSet<latest_projects> latest_projects { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            configure_odd_people(modelBuilder);
            configure_project(modelBuilder);
            configure_user(modelBuilder);
            configure_latest_projects(modelBuilder);
        }

        private static void configure_project(DbModelBuilder modelBuilder)
        {
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
        }

        private static void configure_odd_people(DbModelBuilder modelBuilder)
        {
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
        }

        private static void configure_user(DbModelBuilder modelBuilder)
        {
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

        private static void configure_latest_projects(DbModelBuilder modelBuilder)
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
