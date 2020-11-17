﻿using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Mapping.Organisation;
using FSAPortfolio.WebAPI.Mapping.Projects;
using FSAPortfolio.WebAPI.Models;
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
using System.Web.UI.WebControls;


namespace FSAPortfolio.WebAPI.App.Sync
{
    internal class SyncProvider
    {
        private ICollection<string> log;
        private const bool randomiseProjectPortfolios = false;


        IMapper mapper;

        internal SyncProvider(ICollection<string> log)
        {
            this.log = log;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PostgresProjectMappingProfile>();
                cfg.AddProfile<ProjectViewModelProfile>();
                cfg.AddProfile<ProjectUpdateModelProfile>();
                cfg.AddProfile<ProjectQueryModelProfile>();
                cfg.AddProfile<ProjectEditOptionsMappingProfile>();
                cfg.AddProfile<PortfolioConfigurationMappingProfile>();
            });
            mapper = config.CreateMapper();

        }

        internal void SyncUsers()
        {
            log.Add("Syncing users...");
            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                // Ensure we have all access groups
                var agViewKeys = SyncMaps.accessGroupKeyMap.Select(kv => kv.Value).Distinct();
                var accessGroupLookup = dest.AccessGroups.Where(ag => ag.ViewKey != null).ToDictionary(ag => ag.ViewKey);
                foreach(var agvk in agViewKeys)
                {
                    if (!accessGroupLookup.ContainsKey(agvk))
                    {
                        AccessGroup accessGroup = new AccessGroup() { ViewKey = agvk, Description = agvk };
                        dest.AccessGroups.Add(accessGroup);
                        accessGroupLookup[agvk] = accessGroup;
                    }
                }
                dest.SaveChanges();
                accessGroupLookup = dest.AccessGroups.Where(ag => ag.ViewKey != null).ToDictionary(ag => ag.ViewKey);

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

        internal async Task SyncPeople(string viewKey = "odd")
        {
            log.Add("Syncing people...");

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var portfolio = await dest.Portfolios.Include(p => p.Teams).SingleAsync(p => p.ViewKey == "odd");
                var usersProvider = new UsersProvider(dest);

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

                    // Set the active directory user
                    MicrosoftGraphUserModel adUser = null;
                    if (destPerson.ActiveDirectoryId == null)
                    {
                        if (destPerson.Email != null)
                        {
                            adUser = await usersProvider.GetUserForPrincipalNameAsync(destPerson.Email);
                            if (adUser != null)
                            {
                                destPerson.ActiveDirectoryPrincipalName = adUser.userPrincipalName;
                                destPerson.ActiveDirectoryId = adUser.id;
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

        internal void SyncDirectorates()
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

        internal void SyncPortfolios()
        {
            using (var context = new PortfolioContext())
            {
                AddPortfolio(context, "Open Data and Digital", "ODD", "odd");
                AddPortfolio(context, "Science, Evidence and Reseach Directorate", "SERD", "serd");
                AddPortfolio(context, "ABC", "ABC", "abc");
                AddPortfolio(context, "FHP", "FHP", "fhp");
                AddPortfolio(context, "OTP", "OTP", "otp");
                AddPortfolio(context, "Test", "Test", "test");
                context.SaveChanges();

                foreach (var portfolio in context.Portfolios)
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

                    // Removed redundant labels
                    var currentLabels = config.Labels.ToArray();
                    foreach (var label in currentLabels)
                    {
                        if(!defaultLabels.Any(l => l.FieldName == label.FieldName))
                        {
                            config.Labels.Remove(label);
                        }
                    }

                    // Add or update labels
                    context.PortfolioConfigurationLabels.AddOrUpdate(l => new { l.Configuration_Id, l.FieldName },  defaultLabels);
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
                        Labels = new List<PortfolioLabelConfig>()
                    }
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
                phaseName = SyncMaps.phaseMap.First().Value;
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
            Func<int, BudgetType> budgetTypeFactory = (o) =>
            {
                string k = $"{ViewKeyPrefix.BudgetType}{o}";
                var budgetType = portfolio.Configuration.BudgetTypes.SingleOrDefault(p => p.ViewKey == k);
                if (budgetType == null)
                {
                    budgetType = new BudgetType() { ViewKey = k, Order = o };
                    portfolio.Configuration.BudgetTypes.Add(budgetType);
                }
                budgetType.Name = SyncMaps.budgetTypeMap[k];
                return budgetType;
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
            categoryFactory(0);
            categoryFactory(1);
            categoryFactory(2);
            categoryFactory(3);
            categoryFactory(4);
            categoryFactory(5);
            sizeFactory(0);
            sizeFactory(1);
            sizeFactory(2);
            sizeFactory(3);
            sizeFactory(4);
            budgetTypeFactory(0);
            budgetTypeFactory(1);
            budgetTypeFactory(2);
            budgetTypeFactory(3);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectIDs, 0);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_AboutTheProject, 1);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectTeam, 2);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProjectPlan, 3);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_ProgressIndicators, 4);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Updates, 5);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Prioritisation, 6);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_Budget, 7);
            labelGroupFactory(FieldGroupConstants.FieldGroupName_FSAProcesses, 8);
        }

        internal void SyncAllProjects()
        {
            IEnumerable<string> projectIds;
            IEnumerator<string> portfolios = null;
            using (var source = new MigratePortfolioContext())
            {
                projectIds = source.projects.Select(p => p.project_id).Distinct().ToArray();
            }
            if (randomiseProjectPortfolios)
            {
                using (var source = new PortfolioContext())
                {
                    portfolios = source.Portfolios.Select(p => p.ShortName).ToList().GetEnumerator();
                }
            }


            foreach (var id in projectIds)
            {
                    string portfolio = "odd";

                if (randomiseProjectPortfolios && portfolios != null)
                {
                    if (!portfolios.MoveNext())
                    {
                        portfolios.Reset();
                        portfolios.MoveNext();
                    }
                    portfolio = portfolios.Current;
                }
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

        internal bool SyncProject(string projectId, string portfolioViewKey = null)
        {
            bool synched = false;

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceProjectItems = source.projects.Where(p => p.project_id == projectId).ToList();

                // Only proceed if have a project with a single name throughout its history
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
                        .Include(p => p.Size)
                        .Include(p => p.BudgetType)
                        .Include(p => p.RelatedProjects)
                        .Include(p => p.DependantProjects)
                        .Include(p => p.Documents)
                        .Include(p => p.People)
                        .SingleOrDefault(p => p.Reservation.ProjectId == latestSourceUpdate.project_id);

                    if (destProject == null)
                    {
                        var pid = latestSourceUpdate.project_id.Trim();
                        var yrStart = pid.Length - 7;
                        destProject = new Project()
                        {
                            Reservation = new ProjectReservation()
                            {
                                ProjectId = pid,
                                Year = int.Parse(pid.Substring(yrStart, 2)) + 2000,
                                Month = int.Parse(pid.Substring(yrStart + 2, 2)),
                                Index = int.Parse(pid.Substring(yrStart + 4)),
                                ReservedAt = DateTime.Now
                            },
                            Updates = new List<ProjectUpdateItem>(),
                            Portfolios = new List<Portfolio>()
                        };
                        dest.Projects.Add(destProject);
                    }

                    // Add to the given portfolio if not already in one
                    if (!string.IsNullOrEmpty(portfolioViewKey))
                    {
                        var portfolio = dest.Portfolios.IncludeConfig().Single(p => p.ShortName == portfolioViewKey);
                        destProject.Portfolios.Clear();
                        destProject.Portfolios.Add(portfolio);
                        destProject.Reservation.Portfolio = portfolio;
                    }


                    destProject = MapProject(dest, sourceProjectItems, destProject, latestSourceUpdate);

                    if (destProject != null)
                    {
                        SyncUpdates(dest, sourceProjectItems, destProject);
                        log.Add($"{projectId} Ok.");
                        synched = true;
                    }
                    else
                    {
                        logFailure(projectId, "Destination project is null!");
                    }
                }
                else
                {
                    logFailure(projectId, $"Details count = {sourceProjectItems.Count()}");
                }
            }

            return synched;
        }

        private void logFailure(string projectId, string message)
        {
            log.Add($"{projectId} FAIL! {message}"); ;
        }

        private void SyncUpdates(PortfolioContext dest, IEnumerable<project> sourceProjectDetail, Project destProject)
        {
            // Now sync the updates
            project lastUpdate = null;
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
                    }
                }
                else
                {
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

        private Project MapProject(PortfolioContext dest, IEnumerable<project> sourceProjectDetail, Project destProject, project latestSourceUpdate)
        {
            try
            {
                if (SyncMaps.directorateKeyMap.ContainsKey(latestSourceUpdate.direct))
                {
                    latestSourceUpdate.direct = SyncMaps.directorateKeyMap[latestSourceUpdate.direct];
                }
                else
                {
                    latestSourceUpdate.direct = latestSourceUpdate.direct?.ToLower();
                }
                mapper.Map(latestSourceUpdate, destProject, opt => opt.Items[nameof(PortfolioContext)] = dest);
                destProject.Description = sourceProjectDetail.Where(u => !string.IsNullOrEmpty(u.short_desc)).OrderBy(u => u.timestamp).LastOrDefault()?.short_desc; // Take the last description
                return destProject;
            }
            catch (AutoMapperMappingException ame)
            {
                if (ame.MemberMap.DestinationName == "Size")
                {
                    log.Add($"MAPPING ERROR: Source project size = {latestSourceUpdate.project_size}");
                }
                else
                {
                    switch(ame.MemberMap.DestinationName)
                    {
                        case "Team":
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}, source team = {latestSourceUpdate.team}");
                            break;
                        case "Directorate":
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}, source data = {latestSourceUpdate.direct}");
                            break;
                        default:
                            log.Add($"MAPPING ERROR: Destination member = {ame.MemberMap.DestinationName}");
                            break;
                    }
                }
            }
            return null;
        }
    }
}