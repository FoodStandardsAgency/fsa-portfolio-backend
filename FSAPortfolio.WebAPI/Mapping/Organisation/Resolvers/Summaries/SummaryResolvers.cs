using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Organisation.Resolvers.Summaries
{
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
                case PortfolioSummaryModel.ByTeam:
                case PortfolioSummaryModel.NewProjectsByTeam:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Teams.OrderBy(t => t.Order).Union(new Team[] { new Team() { Name = "None set", Id = 0 } }));
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
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null))
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
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.RAGStatus.Id == source.Id, context);
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
                    result = SummaryLinqQuery.GetQuery(portfolioConfiguration, p => p.Lead?.Team != null && p.Lead.Team.ViewKey == source.ViewKey, context);
                    break;
            }

            return result;
        }
    }
}