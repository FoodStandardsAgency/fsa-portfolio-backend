using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL;
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

        internal void SyncStatuses()
        {
            log.Add("Syncing statuses...");
            using (var dest = new PortfolioContext())
            {
                Func<string, ProjectPhase> phaseFactory = (k) => new ProjectPhase() { Name = phaseMap[k], ViewKey = k };
                Func<string, ProjectOnHoldStatus> onHoldFactory = (k) => new ProjectOnHoldStatus() { Name = onholdMap[k], ViewKey = k };
                Func<string, ProjectRAGStatus> ragFactory = (k) => new ProjectRAGStatus() { Name = ragMap[k], ViewKey = k };
                Func<string, ProjectCategory> categoryFactory = (k) => new ProjectCategory() { Name = categoryMap[k], ViewKey = k };
                dest.ProjectPhases.AddOrUpdate(p => p.Name,
                    phaseFactory("backlog"),
                    phaseFactory("discovery"),
                    phaseFactory("alpha"),
                    phaseFactory("beta"),
                    phaseFactory("live"),
                    phaseFactory("completed")
                    );
                dest.ProjectOnHoldStatuses.AddOrUpdate(p => p.Name,
                    onHoldFactory("n"),
                    onHoldFactory("y"),
                    onHoldFactory("b"),
                    onHoldFactory("c")
                    );
                dest.ProjectRAGStatuses.AddOrUpdate(p => p.Name,
                    ragFactory("red"),
                    ragFactory("amb"),
                    ragFactory("gre"),
                    ragFactory("nor")
                    );
                dest.ProjectCategories.AddOrUpdate(p => p.Name,
                    categoryFactory("cap"),
                    categoryFactory("data"),
                    categoryFactory("sm"),
                    categoryFactory("ser"),
                    categoryFactory("it"),
                    categoryFactory("res"),
                    new ProjectCategory() { Name = CategoryConstants.NotSetName }
                    );
                dest.SaveChanges();
            }

            log.Add($"Syncing statuses complete.");
        }

        internal void SyncPortfolios()
        {
            using (var dest = new PortfolioContext())
            {
                dest.Portfolios.AddOrUpdate(p => p.ShortName,
                    new Portfolio() { Name = "Open Data and Digital", ShortName = "ODD", Route = "odd" },
                    new Portfolio() { Name = "SERD", ShortName = "SERD", Route = "serd" },
                    new Portfolio() { Name = "ABC", ShortName = "ABC", Route = "abc" },
                    new Portfolio() { Name = "Test1", ShortName = "Test1", Route = "test1" },
                    new Portfolio() { Name = "Test2", ShortName = "Test2", Route = "test2" },
                    new Portfolio() { Name = "Test3", ShortName = "Test3", Route = "test3" },
                    new Portfolio() { Name = "Test4", ShortName = "Test4", Route = "test4" }
                    );
                dest.SaveChanges();
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
                        .SingleOrDefault(p => p.ProjectId == sourceProjectDetail.Key.project_id);

                    var latestSourceUpdate = sourceProjectDetail.OrderBy(d => d.timestamp).Last();
                    if (destProject == null)
                    {
                        destProject = new Project()
                        {
                            ProjectId = sourceProjectDetail.Key.project_id,
                            Name = sourceProjectDetail.Key.project_name,
                            Updates = new List<ProjectUpdateItem>(),
                            Portfolios = new List<Portfolio>()
                        };
                        dest.Projects.Add(destProject);
                    }
                    destProject.StartDate = GetPostgresDate(latestSourceUpdate.start_date); // Take the latest date
                    destProject.Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc; // Take the last description
                    destProject.Priority = int.Parse(latestSourceUpdate.priority_main);
                    destProject.Category = dest.ProjectCategories.SingleOrDefault(c => c.ViewKey == latestSourceUpdate.category);

                    // Sync the portfolio
                    if(!string.IsNullOrEmpty(portfolioShortName) && destProject.Portfolios.Count == 0)
                    {
                        destProject.Portfolios.Add(dest.Portfolios.Single(p => p.ShortName == portfolioShortName));
                    }

                    // Sync the lead
                    if (!string.IsNullOrWhiteSpace(latestSourceUpdate.oddlead_email))
                    {
                        destProject.Lead = dest.People.SingleOrDefault(p => p.Email == latestSourceUpdate.oddlead_email);
                    }
                    else
                    {
                        destProject.Lead = null;
                    }

                    // Now sync the updates
                    ProjectUpdateItem firstUpdate = null;
                    ProjectUpdateItem lastUpdate = null;
                    foreach (var sourceUpdate in sourceProjectDetail.OrderBy(u => u.timestamp))
                    {
                        var destUpdate = destProject.Updates.SingleOrDefault(u => u.SyncId == sourceUpdate.id);
                        if (destUpdate == null)
                        {
                            destUpdate = new ProjectUpdateItem()
                            {
                                SyncId = sourceUpdate.id,
                                Timestamp = sourceUpdate.timestamp
                            };
                            destProject.Updates.Add(destUpdate);
                        }

                        // Apply changes
                        destUpdate.RAGStatus = dest.ProjectRAGStatuses.SingleOrDefault(s => s.ViewKey == sourceUpdate.rag);
                        destUpdate.OnHoldStatus = dest.ProjectOnHoldStatuses.SingleOrDefault(s => s.ViewKey == sourceUpdate.onhold);
                        destUpdate.Phase = dest.ProjectPhases.SingleOrDefault(s => s.ViewKey == sourceUpdate.phase);

                        if (firstUpdate == null) firstUpdate = destUpdate;
                         lastUpdate = destUpdate;
                    }

                    dest.SaveChanges();

                    // Set the latest update
                    destProject.LatestUpdate = lastUpdate;
                    destProject.FirstUpdate = firstUpdate;
                    dest.SaveChanges();
                    log.Add($"Syncing project {projectId} complete.");
                }
            }
            return synched;
        }

        private DateTime? GetPostgresDate(string date)
        {
            DateTime result;
            if (DateTime.TryParseExact(date, "dd/mm/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }
}