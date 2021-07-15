using FSAPortfolio.Application.Services;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net;
using LinqKit;
using System.Linq.Expressions;
using FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries;
using FSAPortfolio.Common.Logging;

namespace FSAPortfolio.Application.Services.Projects
{
    public class PortfolioService : BaseService, IPortfolioService
    {
        public PortfolioService(IServiceContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PortfolioModel>> GetPortfoliosAsync()
        {
            var context = ServiceContext.PortfolioContext;
            var portfolios = await context.Portfolios.ToListAsync();
            List<Portfolio> validPortfolios = null;
            validPortfolios = new List<Portfolio>();
            foreach (var portfolio in portfolios)
            {
                if (ServiceContext.HasPermission(portfolio))
                {
                    validPortfolios.Add(portfolio);
                }
            }
            var result = PortfolioMapper.ConfigMapper.Map<IEnumerable<PortfolioModel>>(validPortfolios);
            return result;
        }

        public async Task<PortfolioSummaryModel> GetSummaryAsync(string viewKey, string summaryType, string userFilter, string projectTypeFilter, bool includeKeyData = false)
        {
            PortfolioSummaryModel result = null;
            var context = ServiceContext.PortfolioContext;
            var portfolio = await context.Portfolios
                .Include(p => p.Teams)
                .IncludeConfig()
                .SingleOrDefaultAsync(p => p.ViewKey == viewKey);

            if (portfolio == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (ServiceContext.HasPermission(portfolio))
            {
                if (!string.IsNullOrEmpty(userFilter)) TraceSummaryQuery(viewKey, summaryType, userFilter, projectTypeFilter);

                var projectFilter = BuildProjectFilter(userFilter, projectTypeFilter);

                await context.LoadProjectsIntoPortfolioAsync(portfolio, projectFilter);

                result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(
                    portfolio,
                    opt =>
                    {
                        opt.Items[ProjectIndexDateResolver.OptionKey] = includeKeyData;
                        opt.Items[ProjectIndexPriorityResolver.OptionKey] = includeKeyData;
                        opt.Items[nameof(PortfolioContext)] = context;
                        opt.Items[PortfolioPersonResolver.PersonKey] = userFilter;
                        opt.Items[nameof(PortfolioConfiguration)] = portfolio.Configuration;
                        opt.Items[PortfolioSummaryResolver.SummaryTypeKey] = summaryType;
                    });
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            return result;
        }

        public async Task<PortfolioSummaryModel> GetSummaryLabelsAsync(string viewKey)
        {
            PortfolioSummaryModel result = null;
            var context = ServiceContext.PortfolioContext;
            var portfolio = await context.Portfolios
                .IncludeConfig()
                .SingleOrDefaultAsync(p => p.ViewKey == viewKey);

            if (portfolio == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (ServiceContext.HasPermission(portfolio))
            {
                result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(portfolio, opt => { });
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            return result;
        }


        private void TraceSummaryQuery(string viewKey, string summaryType, string userFilter, string projectTypeFilter)
        {
            AppLog.TraceWarning($"Project Summary Query: Portfolio={viewKey}, SummaryType={summaryType}, UserFilter={userFilter}, ProjectTypeFilter={projectTypeFilter}, CurrentUser={ServiceContext.CurrentUserName}");
        }

        private static Expression<Func<Project, bool>> BuildProjectFilter(string user, string projectType)
        {

            Expression<Func<Project, bool>> projectFilter = null;

            if (!(string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(projectType)))
            {
                var projectTypePredicate = PredicateBuilder.New<Project>();
                var userPredicate = PredicateBuilder.New<Project>();

                // User predicates
                if (!string.IsNullOrWhiteSpace(user))
                {
                    userPredicate = userPredicate.Start(p => p.Lead.ActiveDirectoryPrincipalName == user)
                        .Or(p => p.KeyContact1.ActiveDirectoryPrincipalName == user)
                        .Or(p => p.KeyContact2.ActiveDirectoryPrincipalName == user)
                        .Or(p => p.KeyContact3.ActiveDirectoryPrincipalName == user)
                        .Or(p => p.People.Any(pp => pp.ActiveDirectoryPrincipalName == user))
                        ;
                }

                // ProjectType predicates
                if (!string.IsNullOrWhiteSpace(projectType))
                {
                    projectTypePredicate = projectTypePredicate.Start(p => p.ProjectType == projectType);
                }

                // Combine predicates to build project filter
                if (userPredicate.IsStarted && projectTypePredicate.IsStarted)
                {
                    projectFilter = userPredicate.And(projectTypePredicate);
                }
                else
                {
                    projectFilter = userPredicate.IsStarted ? 
                        userPredicate : 
                        projectTypePredicate.IsStarted ? 
                            projectTypePredicate : 
                            null;
                }
            }

            return projectFilter;
        }

        public async Task<PortfolioConfiguration> GetConfigAsync(string portfolioViewKey, bool includedOnly = false)
        {
            var query = (from c in ServiceContext.PortfolioContext.PortfolioConfigurations.IncludeFullConfiguration()
                         where c.Portfolio.ViewKey == portfolioViewKey
                         select c);
            return await query.SingleAsync();
        }

        public async Task<ProjectReservation> GetProjectReservationAsync(PortfolioConfiguration config)
        {
            var timestamp = DateTime.Now;
            int year = timestamp.Year;
            int month = timestamp.Month;

            var maxIndexQuery = ServiceContext.PortfolioContext.ProjectReservations
                .Where(r => r.Portfolio_Id == config.Portfolio_Id && r.Year == year && r.Month == month)
                .Select(r => (int?)r.Index);
            int maxIndex = (await maxIndexQuery.MaxAsync()) ?? 0;

            var reservation = new ProjectReservation()
            {
                Year = year,
                Month = month,
                Index = maxIndex + 1,
                ReservedAt = timestamp,
                Portfolio_Id = config.Portfolio_Id
            };

            ServiceContext.PortfolioContext.ProjectReservations.Add(reservation);
            return reservation;
        }

        public async Task<ProjectEditOptionsModel> GetNewProjectOptionsAsync(PortfolioConfiguration config, ProjectEditViewModel projectModel = null)
        {
            var options = PortfolioMapper.ProjectMapper.Map<ProjectEditOptionsModel>(config);
            await ProjectEditOptionsManualMaps.MapAsync(ServiceContext.PortfolioContext, config, options, projectModel);
            return options;
        }

        public PortfolioLabelConfig[] GetCustomFilterLabels(PortfolioConfiguration config)
        {
            var customLabels = new PortfolioLabelConfig[] {
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_ProjectTeam),
                        Label = FilterFieldConstants.TeamMemberNameName,
                        FieldName = FilterFieldConstants.TeamMemberNameFilter,
                        FieldType = PortfolioFieldType.FreeText
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_ProjectTeam),
                        Label = FilterFieldConstants.LeadTeamName,
                        FieldName = FilterFieldConstants.LeadTeamFilter,
                        FieldType = PortfolioFieldType.OptionList
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_Updates),
                        Label = FilterFieldConstants.LastUpdateName,
                        FieldName = FilterFieldConstants.LastUpdateFilter,
                        FieldType = PortfolioFieldType.Date
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_Updates),
                        Label = FilterFieldConstants.NoUpdatesName,
                        FieldName = FilterFieldConstants.NoUpdatesFilter,
                        FieldType = PortfolioFieldType.NullableBoolean
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_ProjectPlan),
                        Label = FilterFieldConstants.PastIntendedStartDateName,
                        FieldName = FilterFieldConstants.PastIntendedStartDateFilter,
                        FieldType = PortfolioFieldType.NullableBoolean
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_ProjectPlan),
                        Label = FilterFieldConstants.MissedEndDateName,
                        FieldName = FilterFieldConstants.MissedEndDateFilter,
                        FieldType = PortfolioFieldType.NullableBoolean
                    },
                    new PortfolioLabelConfig()
                    {
                        Included = true,
                        Group = config.LabelGroups.SingleOrDefault(g => g.Name == FieldGroupConstants.FieldGroupName_Prioritisation),
                        Label = FilterFieldConstants.PriorityGroupName,
                        FieldName = FilterFieldConstants.PriorityGroupFilter,
                        FieldType = PortfolioFieldType.OptionList
                    }

                };
            return customLabels;

        }

        public async Task<string[]> ArchiveProjectsAsync(string portfolioViewKey)
        {

            // Get the config and set the date cutoff
            var portfolioConfig = await ServiceContext.PortfolioContext.PortfolioConfigurations
                .Include(c => c.CompletedPhase)
                .Include(c => c.ArchivePhase)
                .SingleAsync(p => p.Portfolio.ViewKey == portfolioViewKey);

            var cutoff = DateTime.Today.AddDays(-portfolioConfig.ArchiveAgeDays);

            // Get all archivable projects last updated before the cutoff
            var projects = await (from p in ServiceContext.PortfolioContext.Projects.Include(p => p.Reservation)
                                  where p.LatestUpdate.Phase.Id == p.Reservation.Portfolio.Configuration.ArchivePhase.Id && p.LatestUpdate.Timestamp < cutoff
                                  select p)
                                  .ToListAsync();
            var latestUpdateIds = projects.Select(p => p.LatestUpdate_Id).ToArray();
            string[] archivedIds = projects.Select(p => p.Reservation.ProjectId).ToArray();

            // Use untracked entities to save explicit cloning
            var untracked_updates = await (from u in ServiceContext.PortfolioContext.ProjectUpdates
                                      .AsNoTracking() // Entities are not tracked
                                           where latestUpdateIds.Contains(u.Id)
                                           select u)
                                      .ToListAsync();

            var archiveTimestamp = DateTime.Now;
            var auditLogText = $"Project archived after {portfolioConfig.ArchiveAgeDays} days in the [{portfolioConfig.ArchivePhase.Name}] phase.";

            foreach (var update in untracked_updates)
            {
                // Get the project for the update
                var project = projects.Single(p => p.ProjectReservation_Id == update.Project_Id);

                // Set the phase to completed and add it again (entity is untracked so a new one is added: saves cloning it)
                update.Id = 0;
                update.Phase_Id = portfolioConfig.CompletedPhase.Id;
                update.SyncId = 0;
                update.Project = null;
                update.Timestamp = archiveTimestamp;
                ServiceContext.PortfolioContext.ProjectUpdates.Add(update);

                // Add an audit log
                var audit = new ProjectAuditLog()
                {
                    Timestamp = archiveTimestamp,
                    Text = auditLogText,
                    Project_Id = project.ProjectReservation_Id
                };
                ServiceContext.PortfolioContext.ProjectAuditLogs.Add(audit);

                // Set the new update as the latest
                project.LatestUpdate = update;
            }

            await ServiceContext.PortfolioContext.SaveChangesAsync();


            return archivedIds;
        }

    }
}