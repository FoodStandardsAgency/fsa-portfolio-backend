using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.UI.WebControls;


namespace FSAPortfolio.WebAPI.App.Sync
{
    internal class SyncProvider
    {
        private ICollection<string> log;
        private static readonly Dictionary<string, string> phaseMap = new Dictionary<string, string>()
        {
            { "backlog", PhaseConstants.BacklogName },
            { "discovery", PhaseConstants.DiscoveryName },
            { "alpha", PhaseConstants.AlphaName },
            { "beta", PhaseConstants.BetaName },
            { "live", PhaseConstants.LiveName },
            { "completed", PhaseConstants.CompletedName }
        };
        private static readonly Dictionary<string, string> onholdMap = new Dictionary<string, string>()
        {
            { "n", OnHoldConstants.NoName },
            { "y", OnHoldConstants.OnHoldName },
            { "b", OnHoldConstants.BlockedName },
            { "c", OnHoldConstants.CovidName }
        };
        private static readonly Dictionary<string, string> ragMap = new Dictionary<string, string>()
        {
            { "red", RagConstants.RedName },
            { "amb", RagConstants.AmberName },
            { "gre", RagConstants.GreenName },
            { "nor", RagConstants.NoneName }
        };
        internal static readonly Dictionary<string, string> categoryMap = new Dictionary<string, string>()
        {
            { "cap", CategoryConstants.CapabilityName },
            { "data", CategoryConstants.DataName },
            { "sm", CategoryConstants.ServiceMgmtName },
            { "ser", CategoryConstants.SupportName },
            { "it", CategoryConstants.ITName },
            { "res", CategoryConstants.ResilienceName }
        };
        private static readonly Dictionary<string, string> sizeMap = new Dictionary<string, string>()
        {
            { "s", ProjectSizeConstants.SmallName },
            { "m", ProjectSizeConstants.MediumName },
            { "l", ProjectSizeConstants.LargeName },
            { "x", ProjectSizeConstants.ExtraLargeName }
        };
        private static readonly Dictionary<string, string> budgetTypeMap = new Dictionary<string, string>()
        {
            { "admin", BudgetTypeConstants.AdminName },
            { "progr", BudgetTypeConstants.ProgrammeName },
            { "capit", BudgetTypeConstants.CapitalName }
        };

        internal SyncProvider(ICollection<string> log)
        {
            this.log = log;
        }
        internal void SyncUsers()
        {
            log.Add("Syncing users...");
            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceAccessGroups = source.users.Select(u => u.access_group)
                    .Distinct()
                    .Select(ag => new AccessGroup() { Name = ag.ToString() })
                    .ToList();

                // Ensure we have all access groups
                var accessGroupLookup = dest.AccessGroups.ToDictionary(ag => ag.Name);
                foreach (var sourceAccessGroup in sourceAccessGroups)
                {
                    var destAccessGroup = dest.AccessGroups.SingleOrDefault(dag => dag.Name == sourceAccessGroup.Name);
                    if (destAccessGroup == null)
                    {
                        destAccessGroup = sourceAccessGroup;
                        dest.AccessGroups.Add(destAccessGroup);
                        accessGroupLookup[sourceAccessGroup.Name] = destAccessGroup;
                        log.Add($"Added access group {destAccessGroup.Name}");
                    }
                }

                // Sync the users
                foreach (var sourceUser in source.users)
                {
                    var destUser = dest.Users.SingleOrDefault(u => u.UserName == sourceUser.username);
                    if (destUser == null)
                    {
                        destUser = new User()
                        {
                            UserName = sourceUser.username,
                            PasswordHash = sourceUser.pass_hash,
                            Timestamp = sourceUser.timestamp,
                            AccessGroup = accessGroupLookup[sourceUser.access_group.ToString()]
                        };
                        dest.Users.Add(destUser);
                        log.Add($"Added user {destUser.UserName}");
                    }
                }

                dest.SaveChanges();
                log.Add("Sync users complete.");
            }
        }

        internal void SyncPeople()
        {
            log.Add("Syncing people...");

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                // Sync the people
                foreach (var sourcePerson in source.odd_people)
                {
                    var destPerson = dest.People.SingleOrDefault(u => u.Email == sourcePerson.email);
                    if (destPerson == null)
                    {
                        destPerson = new Person()
                        {
                            Firstname = sourcePerson.firstname,
                            Surname = sourcePerson.surname,
                            Email = sourcePerson.email,
                            G6team = sourcePerson.g6team,
                            Timestamp = sourcePerson.timestamp
                        };
                        dest.People.Add(destPerson);
                        log.Add($"Added person {destPerson.Firstname} {destPerson.Surname}");
                    }
                }

                dest.SaveChanges();
                log.Add("Sync people complete.");
            }
        }

        internal void SyncPortfolios()
        {
            using (var context = new PortfolioContext())
            {
                AddPortfolio(context, "Open Data and Digital", "ODD", "odd");
                AddPortfolio(context, "SERD", "SERD", "serd");
                AddPortfolio(context, "ABC", "ABC", "abc");
                AddPortfolio(context, "Test1", "Test1", "test1");
                AddPortfolio(context, "Test2", "Test2", "test2");
                AddPortfolio(context, "Test3", "Test3", "test3");
                AddPortfolio(context, "Test4", "Test4", "test4");
                context.SaveChanges();

                foreach (var portfolio in context.Portfolios)
                {
                    portfolio.Configuration.CompletedPhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == "completed");
                }
                context.SaveChanges();

            }
            using (var context = new PortfolioContext())
            {
                foreach (var config in context.PortfolioConfigurations.Include(c => c.LabelGroups).ToList())
                {
                    var defaults = new DefaultFieldLabels(config);
                    context.PortfolioConfigurationLabels.AddOrUpdate(l => new { l.Configuration_Id, l.FieldName },  defaults.GetDefaultLabels());
                }
                context.SaveChanges();
            }
        }

        private void AddPortfolio(PortfolioContext context, string name, string shortName, string viewKey)
        {
            var portfolio = context.Portfolios
                .Include(p => p.Configuration.Phases)
                .Include(p => p.Configuration.OnHoldStatuses)
                .Include(p => p.Configuration.RAGStatuses)
                .Include(p => p.Configuration.Categories)
                .Include(p => p.Configuration.ProjectSizes)
                .Include(p => p.Configuration.BudgetTypes)
                .Include(p => p.Configuration.LabelGroups)
                .Include(p => p.Configuration.Labels)
                .SingleOrDefault(p => p.ViewKey == viewKey);

            if(portfolio == null)
            {
                portfolio = new Portfolio() { ViewKey = viewKey };
                context.Portfolios.Add(portfolio);
            }
            portfolio.Name = name;
            portfolio.ShortName = shortName;

            Func<string, ProjectPhase> phaseFactory = (k) => {
                var phase = portfolio.Configuration.Phases.SingleOrDefault(p => p.ViewKey == k);
                if (phase == null)
                {
                    phase = new ProjectPhase() { ViewKey = k };
                    portfolio.Configuration.Phases.Add(phase);
                }
                phase.Name = phaseMap[k];
                return phase;
            };
            Func<string, ProjectOnHoldStatus> onHoldFactory = (k) =>
            {
                var onhold = portfolio.Configuration.OnHoldStatuses.SingleOrDefault(p => p.ViewKey == k);
                if (onhold == null)
                {
                    onhold = new ProjectOnHoldStatus() { ViewKey = k };
                    portfolio.Configuration.OnHoldStatuses.Add(onhold);
                }
                onhold.Name = onholdMap[k];
                return onhold;
            };
            Func<string, ProjectRAGStatus> ragFactory = (k) =>
            {
                var rag = portfolio.Configuration.RAGStatuses.SingleOrDefault(p => p.ViewKey == k);
                if (rag == null)
                {
                    rag = new ProjectRAGStatus() { ViewKey = k };
                    portfolio.Configuration.RAGStatuses.Add(rag);
                }
                rag.Name = ragMap[k];
                return rag;
            };
            Func<string, ProjectCategory> categoryFactory = (k) =>
            {
                var category = portfolio.Configuration.Categories.SingleOrDefault(p => p.ViewKey == k);
                if (category == null)
                {
                    category = new ProjectCategory() { ViewKey = k };
                    portfolio.Configuration.Categories.Add(category);
                }
                category.Name = categoryMap[k];
                return category;
            };
            Func<string, ProjectSize> sizeFactory = (k) =>
            {
                var projectSize = portfolio.Configuration.ProjectSizes.SingleOrDefault(p => p.ViewKey == k);
                if (projectSize == null)
                {
                    projectSize = new ProjectSize() { ViewKey = k };
                    portfolio.Configuration.ProjectSizes.Add(projectSize);
                }
                projectSize.Name = sizeMap[k];
                return projectSize;
            };
            Func<string, BudgetType> budgetTypeFactory = (k) =>
            {
                var budgetType = portfolio.Configuration.BudgetTypes.SingleOrDefault(p => p.ViewKey == k);
                if (budgetType == null)
                {
                    budgetType = new BudgetType() { ViewKey = k };
                    portfolio.Configuration.BudgetTypes.Add(budgetType);
                }
                budgetType.Name = budgetTypeMap[k];
                return budgetType;
            };
            Func<string, int, PortfolioLabelGroup> labelGroupFactory = (n, o) =>
            {
                var group = portfolio.Configuration.LabelGroups.SingleOrDefault(p => p.Name == n);
                if (group == null)
                {
                    group = new PortfolioLabelGroup() { Name = n };
                    portfolio.Configuration.LabelGroups.Add(group);
                }
                group.Order = o;
                return group;
            };

            phaseFactory("backlog");
            phaseFactory("discovery");
            phaseFactory("alpha");
            phaseFactory("beta");
            phaseFactory("live");
            phaseFactory("completed");
            ragFactory("red");
            ragFactory("amb");
            ragFactory("gre");
            ragFactory("nor");
            onHoldFactory("n");
            onHoldFactory("y");
            onHoldFactory("b");
            onHoldFactory("c");
            categoryFactory("cap");
            categoryFactory("data");
            categoryFactory("sm");
            categoryFactory("ser");
            categoryFactory("it");
            categoryFactory("res");
            sizeFactory("s");
            sizeFactory("m");
            sizeFactory("l");
            sizeFactory("x");
            budgetTypeFactory("admin");
            budgetTypeFactory("progr");
            budgetTypeFactory("capit");
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_ProjectIDs, 0);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_AboutTheProject, 1);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_ProjectTeam, 2);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_ProjectPlan, 3);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_ProgressIndicators, 4);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_Updates, 5);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_Prioritisation, 6);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_Budget, 7);
            labelGroupFactory(DefaultFieldLabels.FieldGroupName_FSAProcesses, 8);
        }

        internal void SyncAllProjects()
        {
            IEnumerable<string> projectIds;
            IEnumerator<string> portfolios;
            using (var source = new MigratePortfolioContext())
            {
                projectIds = source.projects.Select(p => p.project_id).Distinct().ToArray();
            }
            using (var source = new PortfolioContext())
            {
                portfolios = source.Portfolios.Select(p => p.ShortName).ToList().GetEnumerator();
            }

            foreach (var id in projectIds)
            {
                try
                {
                    if(!portfolios.MoveNext())
                    {
                        portfolios.Reset();
                        portfolios.MoveNext();
                    }
                    SyncProject(id, portfolios.Current);
                }
                catch(Exception e)
                {
                    log.Add($"Project {id} failed to sync: {e.Message}");
                    throw e;
                }
            }
        }
        internal bool SyncProject(string projectId, string portfolioShortName = null)
        {
            log.Add($"Syncing project {projectId}...");
            bool synched = false;

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceProjectItems = source.projects.Where(p => p.project_id == projectId);
                log.Add($"{sourceProjectItems.Count()} project items");

                var projectDetails = from pi in sourceProjectItems
                                     group pi by new
                                     {
                                         pi.project_id,
                                         pi.project_name
                                     }
                                     into projectDetailGroup
                                     select projectDetailGroup;

                // Only proceed if have a project with a single name throughout its history
                if (projectDetails.Count() == 1)
                {
                    synched = true;
                    var sourceProjectDetail = projectDetails.Single();

                    // First sync the project
                    var destProject = dest.Projects
                        .Include(p => p.Portfolios)
                        .Include(p => p.Updates.Select(u => u.OnHoldStatus))
                        .Include(p => p.Updates.Select(u => u.RAGStatus))
                        .Include(p => p.Updates.Select(u => u.Phase))
                        .Include(p => p.Category)
                        .Include(p => p.Size)
                        .Include(p => p.BudgetType)
                        .Include(p => p.RelatedProjects)
                        .Include(p => p.DependantProjects)
                        .SingleOrDefault(p => p.ProjectId == sourceProjectDetail.Key.project_id);

                    var latestSourceUpdate = sourceProjectDetail.OrderBy(d => d.timestamp).Last();
                    if (destProject == null)
                    {
                        destProject = new Project()
                        {
                            ProjectId = sourceProjectDetail.Key.project_id,
                            Updates = new List<ProjectUpdateItem>(),
                            Portfolios = new List<Portfolio>()
                        };
                        dest.Projects.Add(destProject);
                    }

                    // Add to the given portfolio if not already in one
                    if (!string.IsNullOrEmpty(portfolioShortName) && destProject.Portfolios.Count == 0)
                    {
                        var portfolio = dest.Portfolios.Single(p => p.ShortName == portfolioShortName);
                        destProject.OwningPortfolio = portfolio;
                        destProject.Portfolios.Add(portfolio);
                    }


                    PortfolioMapper.Mapper.Map(latestSourceUpdate, destProject, opt => opt.Items[ProjectMappingProfile.PortfolioContextKey] = dest);

                    destProject.Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc; // Take the last description


                    // Now sync the updates
                    foreach (var sourceUpdate in sourceProjectDetail.OrderBy(u => u.timestamp))
                    {
                        var destUpdate = destProject.Updates.SingleOrDefault(u => u.SyncId == sourceUpdate.id);
                        if (destUpdate == null)
                        {
                            destUpdate = new ProjectUpdateItem()
                            {
                                SyncId = sourceUpdate.id,
                                Project = destProject
                            };
                            destProject.Updates.Add(destUpdate);
                        }
                        PortfolioMapper.Mapper.Map(sourceUpdate, destUpdate, opt => opt.Items[ProjectMappingProfile.PortfolioContextKey] = dest);
                    }

                    dest.SaveChanges();

                    // Set the latest update
                    var updates = destProject.Updates.OrderBy(u => u.Timestamp);
                    destProject.FirstUpdate = updates.First();
                    destProject.LatestUpdate = updates.Last();
                    dest.SaveChanges();
                    log.Add($"Syncing project {projectId} complete.");
                }
            }
            return synched;
        }

    }
}