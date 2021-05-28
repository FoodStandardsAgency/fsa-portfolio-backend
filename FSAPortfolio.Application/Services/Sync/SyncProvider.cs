using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.App.Mapping.Organisation;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text.RegularExpressions;
using FSAPortfolio.WebAPI.App.Identity;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class SyncProvider
    {
        private ICollection<string> log;

        private const bool SyncODDOnly = true;

        IMapper mapper;
        internal bool debug;

        public SyncProvider(ICollection<string> log, bool debug = false)
        {
            this.log = log;
            this.debug = debug;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PostgresODDProjectMappingProfile>();
                cfg.AddProfile<PostgresSERDProjectMappingProfile>();
                cfg.AddProfile<ProjectViewModelProfile>();
                cfg.AddProfile<ProjectUpdateModelProfile>();
                cfg.AddProfile<ProjectQueryModelProfile>();
                cfg.AddProfile<ProjectEditOptionsMappingProfile>();
                cfg.AddProfile<PortfolioConfigurationMappingProfile>();
            });
            mapper = config.CreateMapper();

        }

        public void SyncUsers()
        {
            log.Add("Syncing users...");
            using (var source = new MigratePortfolioContext<oddproject>("odd"))
            using (var dest = new PortfolioContext())
            {
                // Ensure we have all access groups
                var agViewKeys = SyncMaps.accessGroupKeyMap.Select(kv => kv.Value).Distinct();
                var accessGroups = agViewKeys.Select(k => new AccessGroup() { ViewKey = k, Description = k }).ToArray();
                dest.AccessGroups.AddOrUpdate(a => a.ViewKey, accessGroups);
                dest.SaveChanges();
                var accessGroupLookup = dest.AccessGroups.Where(ag => ag.ViewKey != null).ToDictionary(ag => ag.ViewKey);

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
                        };
                        dest.Users.Add(destUser);
                        log.Add($"Added user {destUser.UserName}");
                    }
                    destUser.AccessGroup = accessGroupLookup[SyncMaps.accessGroupKeyMap[sourceUser.access_group]];
                    string[] roles;
                    if (SyncMaps.userRoleMap.TryGetValue(sourceUser.username, out roles))
                    {
                        destUser.RoleList = string.Join(",", roles);
                    }
                }

                dest.SaveChanges();
                var toDelete = dest.AccessGroups.Where(a => a.ViewKey == null).ToArray();
                if (toDelete.Length > 0)
                {
                    dest.AccessGroups.RemoveRange(toDelete);
                    dest.SaveChanges();
                }

                log.Add("Sync users complete.");
            }
        }

        public async Task SyncPeople(string viewKey = "odd", bool forceADSync = false)
        {
            log.Add("Syncing people...");

            using (var source = new MigratePortfolioContext<oddproject>("odd"))
            using (var dest = new PortfolioContext())
            {
                var portfolio = await dest.Portfolios.Include(p => p.Teams).SingleAsync(p => p.ViewKey == "odd");
                var usersProvider = new PersonProvider(dest);
                var roleManager = new PortfolioRoleManager(dest);

                // Sync the people
                foreach (var sourcePerson in source.odd_people.AsEnumerable().Where(p => !string.IsNullOrWhiteSpace(p.email)))
                {
                    var destPerson = 
                        dest.People.Local.SingleOrDefault(u => u.Email == sourcePerson.email) ?? 
                        dest.People.Include(p => p.Team).SingleOrDefault(u => u.Email == sourcePerson.email);

                    if (destPerson == null)
                    {
                        destPerson = new Person()
                        {
                            Firstname = sourcePerson.firstname,
                            Surname = sourcePerson.surname,
                            Email = sourcePerson.email,
                            Timestamp = sourcePerson.timestamp
                        };
                        dest.People.Add(destPerson);
                        log.Add($"Added person {destPerson.Firstname} {destPerson.Surname}");
                    }

                    if (SyncMaps.emailMap.ContainsKey(destPerson.Email)) 
                        destPerson.Email = SyncMaps.emailMap[destPerson.Email];

                    // Set the active directory user
                    MicrosoftGraphUserModel adUser = null;
                    if (destPerson.ActiveDirectoryId == null || forceADSync)
                    {
                        if (destPerson.Email != null)
                        {
                            var graph = new MicrosoftGraphUserStore(roleManager);
                            adUser = await graph.GetUserForPrincipalNameAsync(destPerson.Email);
                            if (adUser != null)
                            {
                                destPerson.ActiveDirectoryPrincipalName = adUser.userPrincipalName;
                                destPerson.ActiveDirectoryId = adUser.id;
                                destPerson.ActiveDirectoryDisplayName = adUser.displayName;
                            }
                            else
                            {
                                log.Add($"USER NOT FOUND! {destPerson.Email}");
                            }
                        }
                    }

                    var teamViewKey = sourcePerson.g6team;
                    if (destPerson.Team == null && destPerson.Team?.ViewKey != teamViewKey)
                    {
                        var team = portfolio.Teams.SingleOrDefault(t => t.ViewKey == teamViewKey);
                        if (team == null)
                        {
                            team = dest.Teams.Local.SingleOrDefault(t => t.ViewKey == teamViewKey) ?? await dest.Teams.SingleOrDefaultAsync(t => t.ViewKey == teamViewKey);
                            if (team == null)
                            {
                                team = new Team()
                                {
                                    ViewKey = teamViewKey,
                                    Name = SyncMaps.teamNameMap[teamViewKey].Item1
                                };
                                dest.Teams.Add(team);
                            }
                            portfolio.Teams.Add(team);
                        }
                        if (team.Order == 0) team.Order = SyncMaps.teamNameMap[teamViewKey].Item2;
                        destPerson.Team = team;
                    }
                }

                dest.SaveChanges();
                log.Add("Sync people complete.");
            }
        }

        public void SyncDirectorates()
        {
            using (var context = new PortfolioContext())
            {
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "FSA wide", ViewKey = "fsa", Order = 1 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Communications", ViewKey = "comms", Order = 2 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Incidents & Resilience", ViewKey = "inc", Order = 3 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Field Operations", ViewKey = "field", Order = 4 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Finance & Performance", ViewKey = "finance", Order = 5 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Food Safety Policy", ViewKey = "policy", Order = 6 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "National Food Crime Unit", ViewKey = "nfcu", Order = 7 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Northern Ireland", ViewKey = "ni", Order = 8 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Openness, Data & Digital", ViewKey = "odd", Order = 9 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "People", ViewKey = "people", Order = 10 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Regulatory Compliance", ViewKey = "comp", Order = 11 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Science, Evidence & Research", ViewKey = "science", Order = 12 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Strategy, Legal & Governance", ViewKey = "strategy", Order = 13 });
                context.Directorates.AddOrUpdate(d => d.ViewKey, new Directorate() { Name = "Wales", ViewKey = "wales", Order = 14 });
                context.SaveChanges();
            }
        }

        public void SyncPortfolios()
        {
            using (var context = new PortfolioContext())
            {
                AddPortfolio(context, "Open Data and Digital", "ODD", "odd");
                if (!SyncODDOnly)
                {
                    AddPortfolio(context, "Science, Evidence and Reseach Directorate", "SERD", "serd");
                    AddPortfolio(context, "ABC", "ABC", "abc");
                    AddPortfolio(context, "FHP", "FHP", "fhp");
                    AddPortfolio(context, "OTP", "OTP", "otp");
                    AddPortfolio(context, "Test", "Test", "test");
                    AddPortfolio(context, "Portfolio Development", "DEV", "dev", "Superuser");
                }
                context.SaveChanges();

                foreach (var portfolio in context.Portfolios.Where(p => p.ViewKey == "dev"))
                {
                    portfolio.Configuration.CompletedPhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == $"{ViewKeyPrefix.Phase}5");
                }
                context.SaveChanges();

            }

            using (var context = new PortfolioContext())
            {
                foreach (var config in context.PortfolioConfigurations.IncludeFullConfiguration().ToList())
                {
                    var defaults = new DefaultFieldLabels(config);
                    var defaultLabels = defaults.GetDefaultLabels();

                    var currentLabels = config.Labels.ToArray();
                    foreach (var label in currentLabels)
                    {
                        var defaultLabel = defaultLabels.SingleOrDefault(l => l.FieldName == label.FieldName);

                        // Removed redundant labels
                        if (defaultLabel == null)
                        {
                            config.Labels.Remove(label);
                        }
                        else
                        {
                            // Preserve these existing values (note that non-configurable values are set in the DefaultFieldLabels!)
                            // TODO: move this into the DefaultFieldLabels
                            defaultLabel.AdminOnly = label.AdminOnly;
                            defaultLabel.Included = label.Included;
                            defaultLabel.Label = label.Label ?? defaultLabel.Label;
                            defaultLabel.FieldOptions = label.FieldOptions ?? defaultLabel.FieldOptions;
                            if (defaultLabel.Flags.HasFlag(PortfolioFieldFlags.Filterable))
                            {
                                defaultLabel.Flags = (defaultLabel.Flags & ~PortfolioFieldFlags.FilterProject) | (label.Flags & PortfolioFieldFlags.FilterProject);
                            }
                        }
                    }

                    // Add or update labels
                    context.PortfolioConfigurationLabels.AddOrUpdate(l => new { l.Configuration_Id, l.FieldName },  defaultLabels);
                }

                context.SaveChanges();
            }
        }

        public Portfolio AddPortfolio(PortfolioContext context, string name, string shortName, string viewKey, string requiredRoles = null)
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
                portfolio = new Portfolio() { 
                    ViewKey = viewKey,
                    Configuration = new PortfolioConfiguration()
                    {
                        Phases = new List<ProjectPhase>(),
                        OnHoldStatuses = new List<ProjectOnHoldStatus>(),
                        RAGStatuses = new List<ProjectRAGStatus>(),
                        Categories = new List<ProjectCategory>(),
                        ProjectSizes = new List<ProjectSize>(),
                        BudgetTypes = new List<BudgetType>(),
                        LabelGroups = new List<PortfolioLabelGroup>(),
                        Labels = new List<PortfolioLabelConfig>(),
                        ArchiveAgeDays = PortfolioSettings.DefaultProjectArchiveAgeDays
                    },
                    RequiredRoleData = requiredRoles
                };
                context.Portfolios.Add(portfolio);
            }
            portfolio.Name = name;
            portfolio.ShortName = shortName;
            portfolio.IDPrefix = viewKey.ToUpper();

            Action<int> phaseFactory = (o) => {
                string phaseName;
                string vk = $"{ViewKeyPrefix.Phase}{o}";
                if (!SyncMaps.phaseMap.TryGetValue(new Tuple<string, string>(viewKey, vk), out phaseName))
                {
                    phaseName = vk;
                }
                var phase = portfolio.Configuration.Phases.SingleOrDefault(p => p.ViewKey == vk);
                if (phase == null)
                {
                    phase = new ProjectPhase() { ViewKey = vk, Order = o };
                    portfolio.Configuration.Phases.Add(phase);
                }
                phase.Name = phaseName;
            };
            Func<int, ProjectOnHoldStatus> onHoldFactory = (o) =>
            {
                string k = $"{ViewKeyPrefix.Status}{o}";
                var onhold = portfolio.Configuration.OnHoldStatuses.SingleOrDefault(p => p.ViewKey == k);
                if (onhold == null)
                {
                    onhold = new ProjectOnHoldStatus() { ViewKey = k, Order = o };
                    portfolio.Configuration.OnHoldStatuses.Add(onhold);
                }
                onhold.Name = SyncMaps.onholdMap[k];
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
                rag.Name = SyncMaps.ragMap[k].Item1;
                rag.Order = SyncMaps.ragMap[k].Item2;
                return rag;
            };
            Action<int> categoryFactory = (o) =>
            {
                string k = $"{ViewKeyPrefix.Category}{o}";
                var category = portfolio.Configuration.Categories.SingleOrDefault(p => p.ViewKey == k);
                if (category == null)
                {
                    category = new ProjectCategory() { ViewKey = k };
                    portfolio.Configuration.Categories.Add(category);
                }
                var tk = new Tuple<string, string>(viewKey, k);
                category.Name = SyncMaps.categoryMap.ContainsKey(tk) ? SyncMaps.categoryMap[tk] : SyncMaps.categoryMap[new Tuple<string, string>("odd", k)];
                category.Order = o;
            };
            Func<int, BudgetType> budgetTypeFactory = (o) =>
            {
                string k = $"{ViewKeyPrefix.BudgetType}{o}";
                var budgetType = portfolio.Configuration.BudgetTypes.SingleOrDefault(p => p.ViewKey == k);
                if (budgetType == null)
                {
                    budgetType = new BudgetType() { ViewKey = k, Order = o };
                    portfolio.Configuration.BudgetTypes.Add(budgetType);
                }
                var tk = new Tuple<string, string>(viewKey, k);
                budgetType.Name = SyncMaps.budgetTypeMap.ContainsKey(tk) ? SyncMaps.budgetTypeMap[tk] : SyncMaps.budgetTypeMap[new Tuple<string, string>("odd", k)];
                budgetType.Order = o;
                return budgetType;
            };

            Func<int, ProjectSize> sizeFactory = (o) =>
            {
                string k = $"{ViewKeyPrefix.ProjectSize}{o}";
                var projectSize = portfolio.Configuration.ProjectSizes.SingleOrDefault(p => p.ViewKey == k);
                if (projectSize == null)
                {
                    projectSize = new ProjectSize() { ViewKey = k, Order = o };
                    portfolio.Configuration.ProjectSizes.Add(projectSize);
                }
                projectSize.Name = SyncMaps.sizeMap[k];
                return projectSize;
            };
            Func<string, int, PortfolioLabelGroup> labelGroupFactory = (n, go) =>
            {
                var group = portfolio.Configuration.LabelGroups.SingleOrDefault(p => p.Name == n);
                if (group == null)
                {
                    group = new PortfolioLabelGroup() { Name = n };
                    portfolio.Configuration.LabelGroups.Add(group);
                }
                group.Order = go;
                return group;
            };

            phaseFactory(0);
            phaseFactory(1);
            phaseFactory(2);
            phaseFactory(3);
            phaseFactory(4);
            phaseFactory(5);
            ragFactory(RagConstants.RedViewKey);
            ragFactory(RagConstants.AmberViewKey);
            ragFactory(RagConstants.GreenViewKey);
            ragFactory(RagConstants.NoneViewKey);
            onHoldFactory(0);
            onHoldFactory(1);
            onHoldFactory(2);
            onHoldFactory(3);

            if (SyncMaps.categoryKeyMap.TryGetValue(viewKey, out var categoryLookup))
            {
                for (int i = 0; i < categoryLookup.Keys.Count; i++)
                {
                    categoryFactory(i);
                }
            }
            sizeFactory(0);
            sizeFactory(1);
            sizeFactory(2);
            sizeFactory(3);
            sizeFactory(4);

            // Need to lookup budgettypes based on portfolio view key
            if (SyncMaps.budgetTypeKeyMap.TryGetValue(viewKey, out var budgetTypeLookup))
            {
                for (int i = 0; i < budgetTypeLookup.Keys.Count; i++)
                {
                    budgetTypeFactory(i);
                }
            }
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectIDs, 0);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_AboutTheProject, 1);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectTeam, 2);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectPlan, 3);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProgressIndicators, 4);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Updates, 5);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Prioritisation, 6);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Budget, 7);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_FSAProcesses, 8);

            return portfolio;
        }

        public void SyncAllProjects(string portfolio = "odd")
        {
            IEnumerable<string> projectIds;
            switch (portfolio)
            {
                case "odd":
                    using (var source = new MigratePortfolioContext<oddproject>(portfolio))
                    {
                        projectIds = source.projects.Select(p => p.project_id).Distinct().ToArray();
                    }
                    break;
                case "serd":
                    if (SyncODDOnly) throw new HttpResponseException(HttpStatusCode.Forbidden);
                    using (var source = new MigratePortfolioContext<serdproject>(portfolio))
                    {
                        log.Add("Syncing SERD projects");
                        projectIds = source.projects.Select(p => p.project_id).Distinct().ToArray();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }


            foreach (var id in projectIds)
            {
                try
                {
                    SyncProject(id, portfolio);
                }
                catch (DbEntityValidationException e)
                {
                    log.Add($"FAIL: {id}");
                    foreach(var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            log.Add($"Property: {ve.PropertyName}");
                            log.Add(ve.ErrorMessage);
                        }
                    }
                }
            }
        }

        public bool SyncProject(string projectId, string portfolioViewKey = null)
        {
            bool synched = false;
            logDebug(projectId, $"syncing...");
            if (string.IsNullOrEmpty(portfolioViewKey))
            {
                var m = Regex.Match(projectId, @"\d+");
                if (m.Success && m.Index > 0 && m.Index <= 4 && m.Index < projectId.Length)
                {
                    portfolioViewKey = projectId.Substring(0, m.Index).ToLower();
                    logDebug(projectId, $"Porfolio = {portfolioViewKey}...");
                }
                else
                {
                    logDebug(projectId, $"Porfolio viewkey not found in id {projectId}...");
                    if(!m.Success) logDebug(projectId, "...viewkey not found");
                    else logDebug(projectId, $"...match index = {m.Index}");
                }
            }

            using (var dest = new PortfolioContext())
            {
                switch(portfolioViewKey)
                {
                    case "odd":
                        using (var source = new MigratePortfolioContext<oddproject>(portfolioViewKey))
                        {
                            synched = SyncODDProject(projectId, source, dest);
                        }
                        break;
                    case "serd":
                        using (var source = new MigratePortfolioContext<serdproject>(portfolioViewKey))
                        {
                            synched = SyncSERDProject(projectId, source, dest);
                        }
                        break;
                    default:
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
            }

            return synched;
        }

        private bool SyncSERDProject(string projectId, MigratePortfolioContext<serdproject> source, PortfolioContext dest)
        {
            if (SyncODDOnly) throw new HttpResponseException(HttpStatusCode.Forbidden);
            string portfolioViewKey = "serd";
            var sourceProjectItems = source.projects.Where(p => p.project_id == projectId).ToList();
            bool synched = SyncProject<serdproject>(projectId, dest, portfolioViewKey, sourceProjectItems);
            return synched;
        }

        private bool SyncODDProject(string projectId, MigratePortfolioContext<oddproject> source, PortfolioContext dest)
        {
            string portfolioViewKey = "odd";
            var sourceProjectItems = source.projects.Where(p => p.project_id == projectId).ToList();
            bool synched = SyncProject<oddproject>(projectId, dest, portfolioViewKey, sourceProjectItems);
            return synched;
        }

        private bool SyncProject<T>(string projectId, PortfolioContext dest, string portfolioViewKey, List<T> sourceProjectItems)
            where T : IPostgresProject, new()
        {
            logDebug(projectId, $"source update count = {sourceProjectItems.Count}");
            bool synched = false;
            if (sourceProjectItems.Count() > 0)
            {
                var latestSourceUpdate = sourceProjectItems.OrderByDescending(p => p.timestamp).First();

                // First sync the project
                var destProject = dest.Projects.FullConfigIncludes()
                    .Include(p => p.Portfolios)
                    .Include(p => p.Updates.Select(u => u.OnHoldStatus))
                    .Include(p => p.Updates.Select(u => u.RAGStatus))
                    .Include(p => p.Updates.Select(u => u.Phase))
                    .Include(p => p.Category)
                    .Include(p => p.Subcategories)
                    .Include(p => p.Size)
                    .Include(p => p.BudgetType)
                    .Include(p => p.RelatedProjects)
                    .Include(p => p.DependantProjects)
                    .Include(p => p.Documents)
                    .Include(p => p.Milestones)
                    .Include(p => p.People)
                    .SingleOrDefault(p => p.Reservation.ProjectId == latestSourceUpdate.project_id);

                if (destProject == null)
                {
                    var pid = latestSourceUpdate.project_id.Trim();
                    var yrStart = pid.Length - 7;
                    destProject = new Project()
                    {
                        TeamSettings = new ProjectGenericSettings(),
                        PlanSettings = new ProjectGenericSettings(),
                        ProgressSettings = new ProjectGenericSettings(),
                        BudgetSettings = new ProjectGenericBudgetSettings(),
                        ProcessSettings = new ProjectGenericSettings(),
                        Reservation = new ProjectReservation()
                        {
                            ProjectId = pid,
                            Year = int.Parse(pid.Substring(yrStart, 2)) + 2000,
                            Month = int.Parse(pid.Substring(yrStart + 2, 2)),
                            Index = int.Parse(pid.Substring(yrStart + 4)),
                            ReservedAt = DateTime.Now
                        },
                        Updates = new List<ProjectUpdateItem>(),
                        Portfolios = new List<Portfolio>(),
                        Documents = new List<Document>(),
                        Milestones = new List<Milestone>()
                    };
                    dest.Projects.Add(destProject);
                }

                // Add to the given portfolio if not already in one
                var portfolio = dest.Portfolios.IncludeConfig().Single(p => p.ShortName == portfolioViewKey);
                destProject.Portfolios.Clear();
                destProject.Portfolios.Add(portfolio);
                destProject.Reservation.Portfolio = portfolio;


                destProject = MapProject<T>(dest, sourceProjectItems, destProject, latestSourceUpdate);

                if (destProject != null)
                {
                    if (destProject.LatestUpdate_Id == null)
                    {
                        SyncUpdates(dest, sourceProjectItems, destProject);
                    }
                    log.Add($"{projectId} Ok.");
                    synched = true;
                }
                else
                {
                    logFailure(projectId);
                }
            }
            else
            {
                logFailure(projectId, $"Details count = {sourceProjectItems.Count()}");
            }

            return synched;
        }

        private void SyncUpdates<T>(PortfolioContext dest, IEnumerable<T> sourceProjectDetail, Project destProject)
            where T : IPostgresProject
        {
            // Now sync the updates
            T lastUpdate = default(T);
            foreach (var sourceUpdate in sourceProjectDetail.OrderBy(u => u.timestamp))
            {
                var destUpdate = destProject.Updates.SingleOrDefault(u => u.SyncId == sourceUpdate.id);
                if (lastUpdate == null || !lastUpdate.IsDuplicate(sourceUpdate))
                {
                    if (destUpdate == null)
                    {
                        destUpdate = new ProjectUpdateItem()
                        {
                            SyncId = sourceUpdate.id,
                            Project = destProject
                        };
                        destProject.Updates.Add(destUpdate);
                    }
                    mapper.Map(sourceUpdate, destUpdate, opt =>
                    {
                        opt.Items[nameof(PortfolioContext)] = dest;
                    });
                    if (lastUpdate != null && (lastUpdate.update?.Equals(sourceUpdate.update) ?? false))
                    {
                        destUpdate.Text = null;
                        debugLogDiscardedUpdateText(sourceUpdate);
                    }
                }
                else
                {
                    debugLogDiscardedUpdateText(sourceUpdate);
                    if (destUpdate != null) dest.ProjectUpdates.Remove(destUpdate);
                }
                lastUpdate = sourceUpdate;
            }

            dest.SaveChanges();


            // Set the latest update
            var updates = destProject.Updates.OrderBy(u => u.Timestamp);
            destProject.FirstUpdate = updates.First();
            destProject.LatestUpdate = updates.Last();
            dest.SaveChanges();
        }

        private Project MapProject<T>(PortfolioContext dest, IEnumerable<T> sourceProjectDetail, Project destProject, T latestSourceUpdate)
            where T : IPostgresProject
        {
            var oddSourceUpdate = latestSourceUpdate as oddproject;
            try
            {
                if (oddSourceUpdate != null)
                {
                    if (SyncMaps.directorateKeyMap.ContainsKey(oddSourceUpdate.direct))
                    {
                        oddSourceUpdate.direct = SyncMaps.directorateKeyMap[oddSourceUpdate.direct];
                    }
                    else
                    {
                        oddSourceUpdate.direct = oddSourceUpdate.direct?.ToLower();
                    }
                }
                mapper.Map(latestSourceUpdate, destProject, opt => opt.Items[nameof(PortfolioContext)] = dest);
                destProject.Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc; // Take the last description
                return destProject;
            }
            catch (AutoMapperMappingException ame)
            {
                if (latestSourceUpdate is oddproject && new string[] { "Size", "Directorate" }.Contains(ame.MemberMap.DestinationName))
                {
                    var oddProject = latestSourceUpdate as oddproject;
                    switch(ame.MemberMap.DestinationName)
                    {
                        case "Directorate":
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}, source data = {oddProject.direct}");
                            break;
                        case "Size":
                            log.Add($"MAPPING ERROR: Source project size = {oddProject.project_size}");
                            break;
                    }
                }
                else
                {
                    switch (ame.MemberMap.DestinationName)
                    {
                        case nameof(Project.BudgetType):
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}, source budgettype = {latestSourceUpdate.budgettype}");
                            break;
                        case nameof(Project.Category):
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}, source category = {latestSourceUpdate.category}");
                            break;
                        default:
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}");
                            break;
                    }
                }
            }
            return null;
        }

        private void logFailure(string projectId, string message)
        {
            log.Add($"{projectId} FAIL! {message}"); ;
        }
        private void logFailure(string projectId)
        {
            log.Add($"{projectId} FAIL!"); ;
        }
        private void logDebug(string projectId, string message)
        {
            if (debug) log.Add($"{projectId}: {message}");
        }

        private void debugLogDiscardedUpdateText(IPostgresProject sourceUpdate)
        {
            if (sourceUpdate != null)
            {
                int length = Math.Min(sourceUpdate.update?.Length ?? 0, 50);
                if (length > 0)
                    logDebug(sourceUpdate.project_id, $"discarding duplicate (Update text = '{sourceUpdate.update.Substring(0, length)}')");
                else
                    logDebug(sourceUpdate.project_id, $"discarding duplicate (Update text empty)");
            }
        }

    }
}