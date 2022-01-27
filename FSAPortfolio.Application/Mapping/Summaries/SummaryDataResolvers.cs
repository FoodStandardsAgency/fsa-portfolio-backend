using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSAPortfolio.Common;

namespace FSAPortfolio.Application.Mapping.Organisation.Resolvers.Summaries
{
    public class PortfolioPersonResolver : IValueResolver<Portfolio, PortfolioSummaryModel, string>
    {
        public const string PersonKey = nameof(PortfolioSummaryModel.Person);
        public string Resolve(Portfolio source, PortfolioSummaryModel destination, string destMember, ResolutionContext context)
        {
            string result = null;
            if (context.Items.ContainsKey(PersonKey))
            {
                string userName = (string)context.Items[PersonKey];
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
                    var person = portfolioContext.People.SingleOrDefault(p => p.ActiveDirectoryPrincipalName == userName);
                    if (person != null) result = person.DisplayName;
                }
            }
            return result;   
        }
    }

    public class ProjectCountByPhaseResolver : IValueResolver<ProjectPhase, PhaseSummaryModel, int>
    {
        public int Resolve(ProjectPhase source, PhaseSummaryModel destination, int destMember, ResolutionContext context)
        {
            var summaryType = context.Items[PortfolioSummaryResolver.SummaryTypeKey] as string;
            switch (summaryType)
            {
                case PortfolioSummaryModel.NewProjectsByTeam:
                    var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                    return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate?.Phase?.Id == source.Id && p.FirstUpdate.Timestamp > newCutoff);
                default:
                    return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate?.Phase?.Id == source.Id);
            }
        }
    }

    public class ProjectSummaryLabelResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<PortfolioLabelModel>>
    {
        private static string[] summaryFields = new string[] {
                ProjectPropertyConstants.category,
                ProjectPropertyConstants.pgroup,
                ProjectPropertyConstants.g6team,
                ProjectPropertyConstants.ProjectLead,
                ProjectPropertyConstants.rag,
                ProjectPropertyConstants.phase,
                ProjectPropertyConstants.project_type
            };
        public IEnumerable<PortfolioLabelModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<PortfolioLabelModel> destMember, ResolutionContext context)
        {
            var summaryLabels = source.Configuration.Labels.Where(l => summaryFields.Contains(l.FieldName));
            return context.Mapper.Map<IEnumerable<PortfolioLabelModel>>(summaryLabels);
        }
    }

    public class ProjectSummaryProjectTypeResolver : IValueResolver<Portfolio, PortfolioSummaryModel, List<DropDownItemModel>>
    {
        public List<DropDownItemModel> Resolve(Portfolio source, PortfolioSummaryModel destination, List<DropDownItemModel> destMember, ResolutionContext context)
        {
            List<DropDownItemModel> options = null;
            var label = source.Configuration.Labels.SingleOrDefault(l => l.FieldName == ProjectPropertyConstants.project_type);
            if (label != null)
            {
                options = (label.FieldOptions ?? string.Empty).Split(',').Select((o, i) =>
                {
                    var ot = o.Trim();
                    return new DropDownItemModel()
                    {
                        Order = i + 1,
                        Display = ot,
                        Value = ot
                    };
                }).ToList();
                options.Insert(0, new DropDownItemModel() { Order = 0, Display = "All projects" });
            }
            return options;
        }
    }

    public class ProjectIndexDateResolver : IValueResolver<Project, ProjectIndexModel, ProjectDateIndexModel>
    {
        public const string OptionKey = nameof(ProjectIndexDateResolver);
        public ProjectDateIndexModel Resolve(Project source, ProjectIndexModel destination, ProjectDateIndexModel destMember, ResolutionContext context)
        {
            ProjectDateIndexModel result = null;

            if (context.Items.ContainsKey(OptionKey) && (bool)context.Items[OptionKey])
            {
                result = new ProjectDateIndexModel();
                if (source.LatestUpdate.Phase.ViewKey == PhaseConstants.BacklogViewKey)
                {
                    // Backlog: use intended start date
                    result.Label = "Start date";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.StartDate);
                }
                else if (source.LatestUpdate.Phase.ViewKey == PhaseConstants.ArchiveViewKey || source.LatestUpdate.Phase.ViewKey == PhaseConstants.CompletedViewKey)
                {
                    // Archive/Completed: use actual end date with custom label
                    result.Label = "Completed";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.ActualEndDate);
                }
                else
                {
                    // Use current phase expected end
                    result.Label = "Deadline";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.LatestUpdate.ExpectedCurrentPhaseEnd);
                }
            }
            return result;
        }
    }

    public class ProjectIndexPriorityResolver : IValueResolver<Project, ProjectIndexModel, ProjectPriorityIndexModel>
    {
        public const string OptionKey = nameof(ProjectIndexPriorityResolver);

        public ProjectPriorityIndexModel Resolve(Project source, ProjectIndexModel destination, ProjectPriorityIndexModel destMember, ResolutionContext context)
        {
            ProjectPriorityIndexModel result = null;
            if (context.Items.ContainsKey(OptionKey) && (bool)context.Items[OptionKey])
            {
                result = new ProjectPriorityIndexModel()
                {
                    Value = source.PriorityGroup.Name,
                    Label = "Priority"
                };
            }
            return result;
        }
    }
}