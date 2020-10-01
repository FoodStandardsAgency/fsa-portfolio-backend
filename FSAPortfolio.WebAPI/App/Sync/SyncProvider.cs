using FSAPortfolio.Entites;
using FSAPortfolio.Entites.Projects;
using FSAPortfolio.Entites.Users;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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


        public void SyncPeople()
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

        public void SyncStatuses()
        {
            log.Add("Syncing statuses...");
            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceRAGStatuses = source.projects.Select(p => p.rag).Distinct().ToList();
                var sourcePhases = source.projects.Select(p => p.phase).Distinct().ToList();
                var sourceOnholdStatuses = source.projects.Select(p => p.onhold).Distinct().ToList();

                foreach (var sourceRag in sourceRAGStatuses)
                {
                    if (ragMap.ContainsKey(sourceRag))
                    {
                        var destRagName = ragMap[sourceRag];
                        var destRag = dest.ProjectRAGStatuses.FirstOrDefault(s => s.Name == destRagName);
                        if (destRag == null)
                        {
                            destRag = new ProjectRAGStatus() { Name = destRagName };
                            dest.ProjectRAGStatuses.Add(destRag);
                        }
                    }
                    else
                    {
                        log.Add($"ragMap doesn't contain key [{sourceRag}]");
                    }
                }
                foreach (var sourcePhase in sourcePhases)
                {
                    if (phaseMap.ContainsKey(sourcePhase))
                    {
                        var destPhaseName = phaseMap[sourcePhase];
                        var destPhase = dest.ProjectPhases.FirstOrDefault(s => s.Name == destPhaseName);
                        if (destPhase == null)
                        {
                            destPhase = new ProjectPhase() { Name = destPhaseName };
                            dest.ProjectPhases.Add(destPhase);
                        }
                    }
                    else
                    {
                        log.Add($"phaseMap doesn't contain key [{sourcePhase}]");
                    }
                }
                foreach (var sourceOnHold in sourceOnholdStatuses)
                {
                    if (onholdMap.ContainsKey(sourceOnHold))
                    {
                        var destOnHoldName = onholdMap[sourceOnHold];
                        var destOnHold = dest.ProjectOnHoldStatuses.FirstOrDefault(s => s.Name == destOnHoldName);
                        if (destOnHold == null)
                        {
                            destOnHold = new ProjectOnHoldStatus() { Name = destOnHoldName };
                            dest.ProjectOnHoldStatuses.Add(destOnHold);
                        }
                    }
                    else
                    {
                        log.Add($"onholdMap doesn't contain key [{sourceOnHold}]");
                    }
                }

                dest.SaveChanges();

            }
            log.Add($"Syncing statuses complete.");
        }

        public void SyncProject(string projectId)
        {
            log.Add("Syncing project {syncRequest.ProjectId}...");

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

                if (projectDetails.Count() == 1)
                {
                    var sourceProjectDetail = projectDetails.Single();

                    // First sync the project
                    var destProject = dest.Projects
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
                            Updates = new List<ProjectUpdateItem>()
                        };
                        dest.Projects.Add(destProject);
                    }
                    destProject.StartDate = GetPostgresDate(latestSourceUpdate.start_date); // Take the latest date
                    destProject.Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc; // Take the last description
                    destProject.Priority = int.Parse(latestSourceUpdate.priority_main);


                    // Now sync the updates
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

                        // Translate field lookups
                        var ragStatusName = ragMap[sourceUpdate.rag];
                        var onHoldStatusName = onholdMap[sourceUpdate.onhold];
                        var phaseName = phaseMap[sourceUpdate.phase];

                        // Apply changes
                        destUpdate.RAGStatus = dest.ProjectRAGStatuses.Single(s => s.Name == ragStatusName);
                        destUpdate.OnHoldStatus = dest.ProjectOnHoldStatuses.Single(s => s.Name == onHoldStatusName);
                        destUpdate.Phase = dest.ProjectPhases.Single(s => s.Name == phaseName);

                        lastUpdate = destUpdate;
                    }

                    dest.SaveChanges();
                    destProject.LatestUpdate = lastUpdate;
                    dest.SaveChanges();
                    log.Add($"Syncing project {projectId} complete.");
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
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