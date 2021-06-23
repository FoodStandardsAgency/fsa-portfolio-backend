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

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries
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
            var summaryType = context.Items[nameof(PortfolioSummaryModel)] as string;
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


    public class PortfolioSummaryResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<ProjectSummaryModel>>
    {
        public IEnumerable<ProjectSummaryModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<ProjectSummaryModel> destMember, ResolutionContext context)
        {
            IEnumerable<ProjectSummaryModel> result;
            var summaryType = context.Items[nameof(PortfolioSummaryModel)] as string;
            switch (summaryType)
            {
                case PortfolioSummaryModel.ByCategory:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.Categories.OrderBy(c => c.Name));
                    break;
                case PortfolioSummaryModel.ByPriorityGroup:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.PriorityGroups.OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByRagStatus:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.RAGStatuses.OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByPhase:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.Phases.Where(p => p.Id != source.Configuration.CompletedPhase.Id).OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByLead:
                    var people = source.Configuration.Portfolio.Projects
                        .Where(p => p.Lead_Id != null)
                        .Select(p => p.Lead)
                        .Distinct()
                        .OrderBy(p => p.Firstname)
                        .ThenBy(p => p.Surname)
                        .ToList();
                    people.Insert(0, new Person() { Id = 0, ActiveDirectoryDisplayName = ProjectTeamConstants.NotSetName });

                    var allResults = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(people);
                    result = allResults.Where(r => r.PhaseProjects.Any(pp => pp.Projects.Count() > 0)); // Only take leads with active projects
                    break;
                case PortfolioSummaryModel.ByTeam:
                case PortfolioSummaryModel.NewProjectsByTeam:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Teams.OrderBy(t => t.Order).Union(new Team[] { new Team() { Name = ProjectTeamConstants.NotSetName, Id = 0 } }));
                    break;
                default:
                    throw new ArgumentException($"Unrecognised summary type: {summaryType}");
            }
            return result;
        }
    }

    internal static class SummaryLinqQuery
    {
        public static IEnumerable<PhaseProjectsModel> GetQuery(PortfolioConfiguration config, Func<Project, bool> projectPredicate, ResolutionContext context)
        {
            var q = from ph in config.Phases.Where(p => p.Id != config.CompletedPhase.Id) // Non-completed phases...
                    join pr in config.Portfolio.Projects
                        .Where(p => p.LatestUpdate?.Phase != null)
                        .Where(projectPredicate)
                        on ph.Id equals pr.LatestUpdate.Phase.Id into projects // ... get projects joined to each phase ...
                    from pr in projects.DefaultIfEmpty() // ... need to get all phases ...
                    group pr by ph into phaseGroup // ... group projects by phase ...
                    select new PhaseProjectsModel()
                    {
                        ViewKey = phaseGroup.Key.ViewKey,
                        Order = phaseGroup.Key.Order,
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null).OrderByDescending(p => p.Priority))
                    };
            return q.OrderBy(p => p.Order);
        }
    }

    public class PhaseProjectsByPriorityGroupResolver : IValueResolver<PriorityGroup, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(PriorityGroup source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.PriorityGroup.ViewKey == source.ViewKey, context);
        }
    }

    public class PhaseProjectsByCategoryResolver : IValueResolver<ProjectCategory, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectCategory source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.ProjectCategory_Id == source.Id, context);
        }
    }

    public class PhaseProjectsByRAGResolver : IValueResolver<ProjectRAGStatus, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectRAGStatus source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.RAGStatus != null && p.LatestUpdate.RAGStatus.Id == source.Id, context);
        }
    }

    public class PhaseProjectsByPhaseResolver : IValueResolver<ProjectPhase, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectPhase source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.Phase.Id == source.Id, context);
        }
    }

    public class PhaseProjectsByTeamResolver : IValueResolver<Team, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(Team source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var portfolioConfiguration = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            var summaryType = context.Items[nameof(PortfolioSummaryModel)] as string;
            IEnumerable<PhaseProjectsModel> result;

            switch(summaryType)
            {
                case PortfolioSummaryModel.NewProjectsByTeam:
                    var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                    result = SummaryLinqQuery.GetQuery(portfolioConfiguration, p => 
                        (
                            (p.Lead?.Team != null && p.Lead.Team.ViewKey == source.ViewKey) ||
                            (p.Lead?.Team == null && source.Id == 0)) // No team set
                        && p.FirstUpdate.Timestamp > newCutoff, context);
                    break;
                case PortfolioSummaryModel.ByTeam:
                default:
                    result = SummaryLinqQuery.GetQuery(portfolioConfiguration, p => 
                        (p.Lead?.Team != null && p.Lead.Team.ViewKey == source.ViewKey) ||
                        (p.Lead?.Team == null && source.Id == 0) // No team set
                        , context);
                    break;
            }

            return result;
        }
    }

    public class PhaseProjectsByTeamLeadResolver : IValueResolver<Person, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(Person source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var config = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            return SummaryLinqQuery.GetQuery(config, p => source.Id == 0 ? !p.Lead_Id.HasValue : p.Lead_Id == source.Id, context);
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
                ProjectPropertyConstants.phase
            };
        public IEnumerable<PortfolioLabelModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<PortfolioLabelModel> destMember, ResolutionContext context)
        {
            var summaryLabels = source.Configuration.Labels.Where(l => summaryFields.Contains(l.FieldName));
            return context.Mapper.Map<IEnumerable<PortfolioLabelModel>>(summaryLabels);
        }
    }

}