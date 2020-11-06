using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public async Task<ProjectEditOptionsModel> GetNewProjectOptionsAsync(PortfolioConfiguration config)
        {
            var options = PortfolioMapper.ProjectMapper.Map<ProjectEditOptionsModel>(config);

            var directorates = await context.Directorates.OrderBy(d => d.Order).Select(d => new DropDownItemModel() { Display = d.Name, Value = d.ViewKey, Order = d.Order }).ToListAsync();
            directorates.Insert(0, new DropDownItemModel() { Display = "None", Value = "", Order = 0 });
            options.Directorates = directorates;

            var projects = await context.Projects
                .IncludeProject()
                .Where(p => p.Reservation.Portfolio_Id == config.Portfolio_Id)
                .OrderBy(p => p.Reservation.ProjectId)
                .ToListAsync();

            options.RelatedProjects = new SelectPickerModel() {
                Header = "Select the related projects (enter a phase or RAG status to narrow list)...",
                Items = projects.Select((p, i) => new SelectPickerItemModel()
                {
                    Value = p.Reservation.ProjectId,
                    Display = $"{p.Reservation.ProjectId}: {p.Name}",
                    SearchTokens = $"{p.Category?.Name},{p.LatestUpdate?.Phase?.Name}",
                    Order = i
                }).ToList()
            };

            options.DependantProjects = new SelectPickerModel()
            {
                Header = "Select the dependencies (enter a phase or RAG status to narrow list)...",
                Items = options.RelatedProjects.Items
            };

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
                    }

                };
            return customLabels;

        }

    }
}