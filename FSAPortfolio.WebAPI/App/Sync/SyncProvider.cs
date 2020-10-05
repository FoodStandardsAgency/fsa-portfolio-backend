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
                foreach (var config in context.PortfolioConfigurations)
                {
                    context.PortfolioConfigurationLabels.AddOrUpdate(l => new { l.Configuration_Id, l.FieldName },  DefaultFieldLabels.GetDefaultLabels(config.Id).ToArray());
                }
                context.SaveChanges();
            }
        }

        private void AddPortfolio(PortfolioContext context, string name, string shortName, string viewKey)
        {
            if (!context.Portfolios.Any(p => p.ViewKey == viewKey))
            {
                Func<string, ProjectPhase> phaseFactory = (k) => new ProjectPhase() { Name = phaseMap[k], ViewKey = k };
                Func<string, ProjectOnHoldStatus> onHoldFactory = (k) => new ProjectOnHoldStatus() { Name = onholdMap[k], ViewKey = k };
                Func<string, ProjectRAGStatus> ragFactory = (k) => new ProjectRAGStatus() { Name = ragMap[k], ViewKey = k };
                Func<string, ProjectCategory> categoryFactory = (k) => new ProjectCategory() { Name = categoryMap[k], ViewKey = k };
                Func<string, ProjectSize> sizeFactory = (k) => new ProjectSize() { Name = sizeMap[k], ViewKey = k };
                Func<string, BudgetType> budgetTypeFactory = (k) => new BudgetType() { Name = budgetTypeMap[k], ViewKey = k };

                var portfolio = new Portfolio()
                {
                    Name = name,
                    ShortName = shortName,
                    ViewKey = viewKey,
                    Configuration = new PortfolioConfiguration()
                    {
                        Phases = new List<ProjectPhase>()
                        {
                            phaseFactory("backlog"),
                            phaseFactory("discovery"),
                            phaseFactory("alpha"),
                            phaseFactory("beta"),
                            phaseFactory("live"),
                            phaseFactory("completed")
                        },
                        RAGStatuses = new List<ProjectRAGStatus>()
                        {
                            ragFactory("red"),
                            ragFactory("amb"),
                            ragFactory("gre"),
                            ragFactory("nor")
                        },
                        OnHoldStatuses = new List<ProjectOnHoldStatus>()
                        {
                            onHoldFactory("n"),
                            onHoldFactory("y"),
                            onHoldFactory("b"),
                            onHoldFactory("c")
                        },
                        Categories = new List<ProjectCategory>()
                        {
                            categoryFactory("cap"),
                            categoryFactory("data"),
                            categoryFactory("sm"),
                            categoryFactory("ser"),
                            categoryFactory("it"),
                            categoryFactory("res"),
                            new ProjectCategory() { Name = CategoryConstants.NotSetName }
                        },
                        ProjectSizes = new List<ProjectSize>()
                        {
                            sizeFactory("s"),
                            sizeFactory("m"),
                            sizeFactory("l"),
                            sizeFactory("x"),
                            new ProjectSize() { Name = ProjectSizeConstants.NotSetName }
                        },
                        BudgetTypes = new List<BudgetType>()
                        {
                            budgetTypeFactory("admin"),
                            budgetTypeFactory("progr"),
                            budgetTypeFactory("capit"),
                            new BudgetType() { Name = BudgetTypeConstants.NotSetName }
                        }
                    }
                };
                context.Portfolios.Add(portfolio);
            }
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