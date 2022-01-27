using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSAPortfolio.Application.Mapping.Organisation.Resolvers.Summaries
{
    public class PortfolioSummaryResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<ProjectSummaryModel>>
    {
        public static string SummaryTypeKey = $"{nameof(PortfolioSummaryResolver)}.SummaryTypeKey";
        public IEnumerable<ProjectSummaryModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<ProjectSummaryModel> destMember, ResolutionContext context)
        {
            IEnumerable<ProjectSummaryModel> result = null;
            if (context.Items.ContainsKey(SummaryTypeKey))
            {
                var summaryType = context.Items[SummaryTypeKey] as string;
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

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(people);

                        result = RemoveEmptySummaries(result);

                        break;
                    case PortfolioSummaryModel.ByTeam:
                    case PortfolioSummaryModel.NewProjectsByTeam:

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(
                            source.Teams.OrderBy(t => t.Order)
                            .Union(new Team[] { new Team() { Name = ProjectTeamConstants.NotSetName, Id = 0 } }));

                        result = RemoveEmptySummaries(result);
                        
                        break;
                    case PortfolioSummaryModel.ByUser:

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(ProjectUserCategory.All(source.Configuration.Labels));

                        result = RemoveEmptySummaries(result); 

                        break;
                    default:
                        throw new ArgumentException($"Unrecognised summary type: {summaryType}");
                }
            }
            return result;
        }

        private IEnumerable<ProjectSummaryModel> RemoveEmptySummaries(IEnumerable<ProjectSummaryModel> summaries)
        {
            return summaries.Where(u => u.PhaseProjects.Any(pp => pp.Projects.Count() > 0));
        }
    }
}