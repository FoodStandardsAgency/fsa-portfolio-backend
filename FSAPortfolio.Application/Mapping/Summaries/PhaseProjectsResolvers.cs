using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Application.Models;
using System.Collections.Generic;
using FSAPortfolio.Entities.Projects;
using System;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;

namespace FSAPortfolio.Application.Mapping.Organisation.Resolvers.Summaries
{
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

    public class PhaseProjectsByUserCategoryResolver : IValueResolver<ProjectUserCategory, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectUserCategory source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var config = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            var user = context.Items[PortfolioPersonResolver.PersonKey] as string;
            return SummaryLinqQuery.GetQuery(config, p => p.GetUserCategory(user) == source.CategoryType, context);
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
            var summaryType = context.Items[PortfolioSummaryResolver.SummaryTypeKey] as string;
            IEnumerable<PhaseProjectsModel> result;

            switch (summaryType)
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
 
}