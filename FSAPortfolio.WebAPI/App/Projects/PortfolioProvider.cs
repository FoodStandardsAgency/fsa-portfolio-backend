using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.WebAPI.Models;
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

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class PortfolioProvider
    {
        private PortfolioContext context;
        private string portfolioViewKey;
        public PortfolioProvider(PortfolioContext context, string portfolioViewKey)
        {
            this.context = context;
            this.portfolioViewKey = portfolioViewKey;
        }

        public async Task<PortfolioConfiguration> GetConfigAsync(bool includedOnly = false)
        {
            var query = (from c in context.PortfolioConfigurations.IncludeFullConfiguration()
                         where c.Portfolio.ViewKey == portfolioViewKey
                         select c);
            return await query.SingleAsync();
        }

        public async Task<ProjectReservation> GetProjectReservationAsync(PortfolioConfiguration config)
        {
            var timestamp = DateTime.Now;
            int year = timestamp.Year;
            int month = timestamp.Month;

            var maxIndexQuery = context.ProjectReservations
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

            context.ProjectReservations.Add(reservation);
            return reservation;
        }

        public async Task<ProjectEditOptionsModel> GetNewProjectOptionsAsync(PortfolioConfiguration config, ProjectEditViewModel projectModel = null)
        {
            var options = PortfolioMapper.ProjectMapper.Map<ProjectEditOptionsModel>(config);
            await ProjectEditOptionsManualMaps.MapAsync(context, config, options, projectModel);
            return options;
        }

        internal PortfolioLabelConfig[] GetCustomFilterLabels(PortfolioConfiguration config)
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

        internal async Task<string[]> ArchiveProjectsAsync()
        {

            // Get the config and set the date cutoff
            var portfolioConfig = await context.PortfolioConfigurations
                .Include(c => c.CompletedPhase)
                .Include(c => c.ArchivePhase)
                .SingleAsync(p => p.Portfolio.ViewKey == portfolioViewKey);

            var cutoff = DateTime.Today.AddDays(-portfolioConfig.ArchiveAgeDays);

            // Get all archivable projects last updated before the cutoff
            var projects = await (from p in context.Projects.Include(p => p.Reservation)
                                  where p.LatestUpdate.Phase.Id == p.Reservation.Portfolio.Configuration.ArchivePhase.Id && p.LatestUpdate.Timestamp < cutoff
                                  select p)
                                  .ToListAsync();
            var latestUpdateIds = projects.Select(p => p.LatestUpdate_Id).ToArray();
            string[] archivedIds = projects.Select(p => p.Reservation.ProjectId).ToArray();

            // Use untracked entities to save explicit cloning
            var untracked_updates = await (from u in context.ProjectUpdates
                                      .AsNoTracking() // Entities are not tracked
                                      where latestUpdateIds.Contains(u.Id)
                                      select u)
                                      .ToListAsync();

            foreach (var update in untracked_updates)
            {
                // Get the project for the update
                var project = projects.Single(p => p.ProjectReservation_Id == update.Project_Id);

                // Set the phase to completed and add it again (entity is untracked so a new one is added: saves cloning it)
                update.Id = 0;
                update.Phase_Id = portfolioConfig.CompletedPhase.Id;
                update.Project = null;
                context.ProjectUpdates.Add(update);

                // Set the new update as the latest
                project.LatestUpdate = update;
            }

            await context.SaveChangesAsync();


            return archivedIds;
        }

    }
}