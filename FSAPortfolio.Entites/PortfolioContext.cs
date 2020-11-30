namespace FSAPortfolio.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FSAPortfolio.Entities.Users;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using FSAPortfolio.Entities.Projects;
    using System.Configuration;
    using FSAPortfolio.Entities.Organisation;

    public partial class PortfolioContext : DbContext
    {
        private const bool LazyLoad = false;
        public PortfolioContext()
            : base("name=PortfolioContext")
        {
            Configuration.LazyLoadingEnabled = LazyLoad;
        }

        public PortfolioContext(ConnectionStringSettings cs)
            : base(cs.ConnectionString)
        {
            Configuration.LazyLoadingEnabled = LazyLoad;
        }

        public static PortfolioContext Create() => new PortfolioContext();

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<AccessGroup> AccessGroups { get; set; }
        public virtual DbSet<Directorate> Directorates { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<PortfolioConfiguration> PortfolioConfigurations { get; set; }
        public virtual DbSet<PortfolioLabelConfig> PortfolioConfigurationLabels { get; set; }

        public virtual DbSet<ProjectReservation> ProjectReservations { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectUpdateItem> ProjectUpdates { get; set; }
        public virtual DbSet<ProjectDataItem> ProjectDataItems { get; set; }
        public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }
        public virtual DbSet<ProjectSize> ProjectSizes { get; set; }
        public virtual DbSet<BudgetType> BudgetTypes { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }

        public virtual DbSet<ProjectRAGStatus> ProjectRAGStatuses { get; set; }
        public virtual DbSet<ProjectOnHoldStatus> ProjectOnHoldStatuses { get; set; }
        public virtual DbSet<ProjectPhase> ProjectPhases { get; set; }

        public virtual DbSet<ProjectAuditLog> ProjectAuditLogs { get; set; }
        public virtual DbSet<PortfolioConfigAuditLog> PortfolioConfigAuditLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PortfolioContext>(null);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(p => p.UserName).IsUnique();
            modelBuilder.Entity<User>().HasRequired(u => u.AccessGroup).WithMany().HasForeignKey(u => u.AccessGroupId);

            modelBuilder.Entity<Person>().HasKey(u => u.Id);
            modelBuilder.Entity<Person>().HasOptional(u => u.Team).WithMany().HasForeignKey(p => p.Team_Id);

            modelBuilder.Entity<Team>().HasKey(u => u.Id);

            modelBuilder.Entity<AccessGroup>().HasKey(u => u.Id);
            modelBuilder.Entity<AccessGroup>().HasIndex(p => p.ViewKey).IsUnique();
            modelBuilder.Entity<AccessGroup>().HasIndex(p => p.Description).IsUnique();

            modelBuilder.Entity<Directorate>().HasKey(p => p.Id);
            modelBuilder.Entity<Directorate>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Directorate>().HasIndex(p => p.ViewKey).IsUnique();

            modelBuilder.Entity<Portfolio>().HasKey(p => p.Id);
            modelBuilder.Entity<Portfolio>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Portfolio>().HasIndex(p => p.ShortName).IsUnique();
            modelBuilder.Entity<Portfolio>().HasIndex(p => p.ViewKey).IsUnique();
            modelBuilder.Entity<Portfolio>().HasIndex(p => p.IDPrefix).IsUnique();
            modelBuilder.Entity<Portfolio>().HasMany(p => p.Projects).WithMany(p => p.Portfolios);
            modelBuilder.Entity<Portfolio>().HasMany(p => p.Teams).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Portfolio_Id");
                mc.MapRightKey("Team_Id");
                mc.ToTable("PortfolioTeams");
            }); ;
            modelBuilder.Entity<Portfolio>().HasRequired(p => p.Configuration).WithRequiredPrincipal(c => c.Portfolio);

            modelBuilder.Entity<PortfolioConfiguration>().HasKey(p => p.Portfolio_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.AuditLogs).WithRequired(l => l.PortfolioConfiguration).HasForeignKey(u => u.PortfolioConfiguration_Id);

            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.Phases).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.RAGStatuses).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.OnHoldStatuses).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);

            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.Categories).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.ProjectSizes).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.BudgetTypes).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);

            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.Labels).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasMany(p => p.LabelGroups).WithRequired(p => p.Configuration).HasForeignKey(p => p.Configuration_Id);
            modelBuilder.Entity<PortfolioConfiguration>().HasOptional(p => p.CompletedPhase);

            modelBuilder.Entity<PortfolioLabelConfig>().HasKey(l => l.Id);
            modelBuilder.Entity<PortfolioLabelConfig>().HasIndex(l => new { l.Configuration_Id, l.FieldName }).IsUnique();
            modelBuilder.Entity<PortfolioLabelConfig>().HasIndex(l => new { l.Configuration_Id, l.FieldTitle }).IsUnique();
            modelBuilder.Entity<PortfolioLabelConfig>().HasOptional(l => l.MasterLabel).WithMany().HasForeignKey(l => l.MasterLabel_Id);

            modelBuilder.Entity<PortfolioLabelGroup>().HasMany(g => g.Labels).WithOptional(l => l.Group);

            modelBuilder.Entity<ProjectReservation>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectReservation>().HasIndex(p => p.ProjectId).IsUnique();
            modelBuilder.Entity<ProjectReservation>().HasRequired(p => p.Portfolio).WithMany().HasForeignKey(p => p.Portfolio_Id);
            modelBuilder.Entity<ProjectReservation>().HasOptional(p => p.Project).WithRequired(p => p.Reservation);
            modelBuilder.Entity<ProjectReservation>().HasIndex(l => new { l.Portfolio_Id, l.Year, l.Month, l.Index }).IsUnique();

            modelBuilder.Entity<Project>().HasKey(p => p.ProjectReservation_Id);
            modelBuilder.Entity<Project>().HasMany(p => p.Updates).WithRequired(u => u.Project).HasForeignKey(u => u.Project_Id);
            modelBuilder.Entity<Project>().HasMany(p => p.RelatedProjects).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Project_Id");
                mc.MapRightKey("RelatedProject_Id");
                mc.ToTable("RelatedProjects");
            });
            modelBuilder.Entity<Project>().HasMany(p => p.DependantProjects).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Project_Id");
                mc.MapRightKey("DependantProject_Id");
                mc.ToTable("DependantProjects");
            });
            modelBuilder.Entity<Project>().HasMany(p => p.Subcategories).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Project_Id");
                mc.MapRightKey("Subcategory_Id");
                mc.ToTable("ProjectSubcategories");
            });
            modelBuilder.Entity<Project>().HasMany(p => p.Documents).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Project_Id");
                mc.MapRightKey("Document_Id");
                mc.ToTable("ProjectDocuments");
            });
            modelBuilder.Entity<Project>().HasMany(p => p.Milestones).WithRequired(m => m.Project).HasForeignKey(m => m.Project_ProjectReservation_Id);
            modelBuilder.Entity<Project>().HasMany(p => p.AuditLogs).WithRequired(l => l.Project).HasForeignKey(u => u.Project_Id);
            modelBuilder.Entity<Project>().HasMany(p => p.ProjectData).WithRequired(l => l.Project).HasForeignKey(u => u.Project_Id);
            modelBuilder.Entity<Project>().HasMany(p => p.People).WithMany().Map(mc =>
            {
                mc.MapLeftKey("Project_Id");
                mc.MapRightKey("Person_Id");
                mc.ToTable("ProjectTeamMembers");
            });
            modelBuilder.Entity<Project>().HasOptional(p => p.LatestUpdate).WithMany().HasForeignKey(p => p.LatestUpdate_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.FirstUpdate).WithMany().HasForeignKey(p => p.FirstUpdate_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.Lead).WithMany().HasForeignKey(p => p.Lead_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.ServiceLead).WithMany().HasForeignKey(p => p.ServiceLead_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.Category).WithMany().HasForeignKey(p => p.ProjectCategory_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.Size).WithMany().HasForeignKey(p => p.ProjectSize_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.BudgetType).WithMany().HasForeignKey(p => p.BudgetType_Id);
            modelBuilder.Entity<Project>().HasOptional(p => p.KeyContact1).WithMany();
            modelBuilder.Entity<Project>().HasOptional(p => p.KeyContact2).WithMany();
            modelBuilder.Entity<Project>().HasOptional(p => p.KeyContact3).WithMany();
            modelBuilder.Entity<Project>().HasOptional(p => p.Directorate).WithMany();

            modelBuilder.ComplexType<ProjectDate>();
            modelBuilder.ComplexType<ProjectLink>();

            modelBuilder.Entity<Document>().HasKey(p => p.Id);
            modelBuilder.Entity<Milestone>().HasKey(p => p.Id);


            modelBuilder.Entity<ProjectDataItem>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectDataItem>().HasRequired(p => p.Label).WithMany().HasForeignKey(p => p.Label_Id);

            modelBuilder.Entity<ProjectAuditLog>().HasKey(p => p.Id);

            modelBuilder.Entity<ProjectCategory>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectCategory>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<ProjectCategory>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();

            modelBuilder.Entity<ProjectSize>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectSize>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<ProjectSize>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();

            modelBuilder.Entity<BudgetType>().HasKey(p => p.Id);
            modelBuilder.Entity<BudgetType>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<BudgetType>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();


            // Status updates
            modelBuilder.Entity<ProjectRAGStatus>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectRAGStatus>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<ProjectRAGStatus>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();

            modelBuilder.Entity<ProjectOnHoldStatus>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectOnHoldStatus>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<ProjectOnHoldStatus>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();

            modelBuilder.Entity<ProjectPhase>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectPhase>().HasIndex(p => new { p.Configuration_Id, p.Name }).IsUnique();
            modelBuilder.Entity<ProjectPhase>().HasIndex(p => new { p.Configuration_Id, p.ViewKey }).IsUnique();


            modelBuilder.Entity<ProjectUpdateItem>().HasKey(u => u.Id);
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.RAGStatus).WithMany();
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.OnHoldStatus).WithMany();
            modelBuilder.Entity<ProjectUpdateItem>().HasOptional(u => u.Phase).WithMany();



        }
    }
}
