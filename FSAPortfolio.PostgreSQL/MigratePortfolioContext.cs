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
        string portfolio = "odd";
        public MigratePortfolioContext()
            : this(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Migration.ConnectionString"] ?? "MigratePortfolioContext"])
        {
        }
        public MigratePortfolioContext(string portfolio)
            : this(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[$"{portfolio.ToUpper()}.Migration.ConnectionString"] ?? "MigratePortfolioContext"])
        {
            this.portfolio = portfolio;
        }

        public MigratePortfolioContext(ConnectionStringSettings cs)
            : base(cs.ConnectionString)
        {
        }

        public virtual DbSet<odd_people> odd_people { get; set; }
        public virtual DbSet<oddproject> projects { get; set; }
        public virtual DbSet<serdproject> serdprojects { get; set; }
        public virtual DbSet<user> users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            if(portfolio == "odd")
                configure_odd_people(modelBuilder);

            configure_project(modelBuilder);
            configure_user(modelBuilder);
        }

        private void configure_project(DbModelBuilder modelBuilder)
        {
            switch (portfolio)
            {
                case "odd":
                    configure_odd_project(modelBuilder);
                    break;
                case "serd":
                    configure_serd_project(modelBuilder);
                    break;
                default:
                    throw new NotImplementedException();
            }

            configure_odd_project(modelBuilder);
        }

        private static void configure_odd_project(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<oddproject>().ToTable("projects");
            modelBuilder.Entity<oddproject>()
                .Property(e => e.project_id)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.project_name)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.start_date)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.short_desc)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.phase)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.subcat)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.rag)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.oddlead)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.oddlead_email)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.servicelead)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.servicelead_email)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.priority_main)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.funded)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.confidence)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.priorities)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.benefits)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.criticality)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.budget)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.spent)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.documents)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.pgroup)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.toupdate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.rels)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.team)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.onhold)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.expend)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.hardend)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.actstart)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.dependencies)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.project_size)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.oddlead_role)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.budgettype)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.direct)
                .IsUnicode(false);

            modelBuilder.Entity<oddproject>()
                .Property(e => e.expendp)
                .IsUnicode(false);
        }

        private static void configure_serd_project(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<serdproject>().ToTable("projects");
            modelBuilder.Entity<serdproject>()
                .Property(e => e.project_id)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.project_name)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.start_date)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.short_desc)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.phase)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.subcat)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.rag)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.lead)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.lead_email)
                .IsUnicode(false);


            modelBuilder.Entity<serdproject>()
                .Property(e => e.priority_main)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.funded)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.confidence)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.priorities)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.benefits)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.criticality)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.budget)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.spent)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.documents)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.pgroup)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.rels)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.team)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.onhold)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.expend)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.hardend)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.actstart)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.dependencies)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.project_type)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.lead_team)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.budgettype)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.fsno)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.ibbcno)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.supplier)
                .IsUnicode(false);

            modelBuilder.Entity<serdproject>()
                .Property(e => e.rd)
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

    }
}
