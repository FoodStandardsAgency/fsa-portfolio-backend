using FSAPortfolio.Entites;
using FSAPortfolio.Entites.Projects;
using FSAPortfolio.Entites.Users;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class SyncController : ApiController
    {
        Dictionary<string, string> phaseMap = new Dictionary<string, string>()
        {
            { "backlog", "Backlog" },
            { "discovery", "Discovery" },
            { "alpha", "Alpha" },
            { "beta", "Beta" },
            { "live", "Live" },
            { "completed", "Completed" },
        };
        Dictionary<string, string> onholdMap = new Dictionary<string, string>()
        {
            { "n", "No" },
            { "y", "On hold" },
            { "b", "Blocked" },
            { "c", "Covid-19 on hold" }
        };
        Dictionary<string, string> ragMap = new Dictionary<string, string>()
        {
            { "red", "Red" },
            { "amb", "Amber" },
            { "gre", "Green" },
            { "nor", "Undecided" }
        };

        // GET: api/Sync
        public string Get()
        {
            return "Success!";
        }

        // GET: api/Sync/SyncUsers
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncUsers()
        {
            List<string> messages = new List<string>() { "Syncing users..." };

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
                    if(destAccessGroup == null)
                    {
                        destAccessGroup = sourceAccessGroup;
                        dest.AccessGroups.Add(destAccessGroup);
                        accessGroupLookup[sourceAccessGroup.Name] = destAccessGroup;
                        messages.Add($"Added access group {destAccessGroup.Name}");
                    }
                }

                // Sync the users
                foreach(var sourceUser in source.users)
                {
                    var destUser = dest.Users.SingleOrDefault(u => u.UserName == sourceUser.username);
                    if(destUser == null)
                    {
                        destUser = new User() { 
                            UserName = sourceUser.username,
                            PasswordHash = sourceUser.pass_hash,
                            Timestamp = sourceUser.timestamp,
                            AccessGroup = accessGroupLookup[sourceUser.access_group.ToString()]
                        };
                        dest.Users.Add(destUser);
                        messages.Add($"Added user {destUser.UserName}");
                    }
                }

                dest.SaveChanges();
                messages.Add("Sync users complete.");
            }

            return messages;
        }

        // GET: api/Sync/SyncStatuses
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncStatuses()
        {
            List<string> messages = new List<string>() { $"Syncing statuses..." };
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
                        messages.Add($"ragMap doesn't contain key [{sourceRag}]");
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
                        messages.Add($"phaseMap doesn't contain key [{sourcePhase}]");
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
                        messages.Add($"onholdMap doesn't contain key [{sourceOnHold}]");
                    }
                }

                dest.SaveChanges();

            }
            messages.Add($"Syncing statuses complete.");
            return messages;
        }


    // GET: api/Sync/SyncProject/{id}
    [AcceptVerbs("GET")]
        public IEnumerable<string> SyncProject(string id)
        {
            List<string> messages = new List<string>() { $"Syncing project {id}..." };

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceProjectItems = source.projects.Where(p => p.project_id == id);
                messages.Add($"{sourceProjectItems.Count()} project items");

                var projectDetails = from pi in sourceProjectItems
                                     group pi by new { 
                                         pi.project_id, 
                                         pi.project_name
                                     }
                                     into projectDetailGroup
                                     select projectDetailGroup;

                if(projectDetails.Count() != 1)
                {
                    throw new Exception("Cannot sync project: project_name is not consistent.");
                }
                var sourceProjectDetail = projectDetails.Single();

                // First sync the project
                var destProject = dest.Projects.Include(p => p.Updates).SingleOrDefault(p => p.ProjectId == sourceProjectDetail.Key.project_id);
                if (destProject == null)
                {
                    destProject = new Project()
                    {
                        ProjectId = sourceProjectDetail.Key.project_id,
                        Name = sourceProjectDetail.Key.project_name,
                        StartDate = GetPostgresDate(sourceProjectDetail.Max(u => u.start_date)), // Take the latest date
                        Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc, // Take the last description
                        Updates = new List<ProjectUpdateItem>()
                    };
                    dest.Projects.Add(destProject);
                }

                // Now sync the updates
                ProjectUpdateItem lastUpdate = null;
                foreach(var sourceUpdate in sourceProjectDetail.OrderBy(u => u.timestamp))
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
                    if (lastUpdate?.RAGStatus?.Name != ragStatusName) destUpdate.RAGStatus = dest.ProjectRAGStatuses.Single(s => s.Name == ragStatusName);
                    if (lastUpdate?.OnHoldStatus?.Name != onHoldStatusName) destUpdate.OnHoldStatus = dest.ProjectOnHoldStatuses.Single(s => s.Name == onHoldStatusName);
                    if (lastUpdate?.Phase?.Name != phaseName) destUpdate.Phase = dest.ProjectPhases.Single(s => s.Name == phaseName);

                    lastUpdate = destUpdate;
                }

                dest.SaveChanges();
                destProject.LatestUpdate = lastUpdate;
                dest.SaveChanges();
                messages.Add($"Syncing project {id} complete.");
            }

            return messages;
        }

        private DateTime GetPostgresDate(string date)
        {
            return DateTime.ParseExact(date, "dd/mm/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
